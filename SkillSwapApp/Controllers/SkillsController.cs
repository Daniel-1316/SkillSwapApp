using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkillSwapApp.Data;
using SkillSwapApp.Helpers;
using SkillSwapApp.Models;

namespace SkillSwap.Controllers
{
    public class SkillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Skills
        [RoleAuthorization("Guest")]
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["CategorySortParm"] = sortOrder == "Category" ? "category_desc" : "Category";

            var skills = from s in _context.Skills.Include(s => s.User) select s;

            // Search
            if (!string.IsNullOrEmpty(searchString))
            {
                skills = skills.Where(s => s.Title.Contains(searchString)
                                        || s.Category.Contains(searchString));
            }

            // Sort
            switch (sortOrder)
            {
                case "title_desc":
                    skills = skills.OrderByDescending(s => s.Title);
                    break;
                case "Category":
                    skills = skills.OrderBy(s => s.Category);
                    break;
                case "category_desc":
                    skills = skills.OrderByDescending(s => s.Category);
                    break;
                default:
                    skills = skills.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Skill>.CreateAsync(skills.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Skills/Details/5
        [RoleAuthorization("User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var skill = await _context.Skills
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (skill == null) return NotFound();

            return View(skill);
        }

        // GET: Skills/Create
        [RoleAuthorization("Admin")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username");
            return View();
        }

        // POST: Skills/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Create([Bind("UserId,Title,Description,Category,Location,IsActive")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", skill.UserId);
            return View(skill);
        }

        // GET: Skills/Edit/5
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", skill.UserId);
            return View(skill);
        }

        // POST: Skills/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,Description,Category,Location,IsActive,CreatedAt")] Skill skill)
        {
            if (id != skill.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillExists(skill.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", skill.UserId);
            return View(skill);
        }

        // GET: Skills/Delete/5
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var skill = await _context.Skills
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (skill == null) return NotFound();

            return View(skill);
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SkillExists(int id)
        {
            return _context.Skills.Any(e => e.Id == id);
        }
    }
}