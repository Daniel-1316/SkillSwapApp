using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkillSwapApp.Data;
using SkillSwapApp.Models;

namespace SkillSwap.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["RatingSortParm"] = String.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            var reviews = from r in _context.Reviews
                         .Include(r => r.Reviewer)
                         .Include(r => r.ReviewedUser)
                         .Include(r => r.Skill)
                          select r;

            // Search
            if (!String.IsNullOrEmpty(searchString))
            {
                reviews = reviews.Where(r => r.Reviewer.Username.Contains(searchString)
                                          || r.ReviewedUser.Username.Contains(searchString));
            }

            // Sort
            switch (sortOrder)
            {
                case "rating_desc":
                    reviews = reviews.OrderByDescending(r => r.Rating);
                    break;
                case "Date":
                    reviews = reviews.OrderBy(r => r.CreatedAt);
                    break;
                case "date_desc":
                    reviews = reviews.OrderByDescending(r => r.CreatedAt);
                    break;
                default:
                    reviews = reviews.OrderBy(r => r.Rating);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Review>.CreateAsync(reviews.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewedUser)
                .Include(r => r.Skill)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null) return NotFound();

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["ReviewerId"] = new SelectList(_context.Users, "Id", "Username");
            ViewData["ReviewedUserId"] = new SelectList(_context.Users, "Id", "Username");
            ViewData["SkillId"] = new SelectList(_context.Skills, "Id", "Title");
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewerId,ReviewedUserId,SkillId,Rating,Comment")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();

                // Update user rating
                await UpdateUserRating(review.ReviewedUserId);

                return RedirectToAction(nameof(Index));
            }
            ViewData["ReviewerId"] = new SelectList(_context.Users, "Id", "Username", review.ReviewerId);
            ViewData["ReviewedUserId"] = new SelectList(_context.Users, "Id", "Username", review.ReviewedUserId);
            ViewData["SkillId"] = new SelectList(_context.Skills, "Id", "Title", review.SkillId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            ViewData["ReviewerId"] = new SelectList(_context.Users, "Id", "Username", review.ReviewerId);
            ViewData["ReviewedUserId"] = new SelectList(_context.Users, "Id", "Username", review.ReviewedUserId);
            ViewData["SkillId"] = new SelectList(_context.Skills, "Id", "Title", review.SkillId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReviewerId,ReviewedUserId,SkillId,Rating,Comment,CreatedAt")] Review review)
        {
            if (id != review.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();

                    // Update user rating
                    await UpdateUserRating(review.ReviewedUserId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReviewerId"] = new SelectList(_context.Users, "Id", "Username", review.ReviewerId);
            ViewData["ReviewedUserId"] = new SelectList(_context.Users, "Id", "Username", review.ReviewedUserId);
            ViewData["SkillId"] = new SelectList(_context.Skills, "Id", "Title", review.SkillId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewedUser)
                .Include(r => r.Skill)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null) return NotFound();

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            int reviewedUserId = review.ReviewedUserId;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Update user rating after deletion
            await UpdateUserRating(reviewedUserId);

            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }

        private async Task UpdateUserRating(int userId)
        {
            var reviews = await _context.Reviews.Where(r => r.ReviewedUserId == userId).ToListAsync();
            if (reviews.Any())
            {
                var avgRating = reviews.Average(r => r.Rating);
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.Rating = avgRating;
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}