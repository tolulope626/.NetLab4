using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;

namespace Lab4.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public StudentsController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? Id)
        {
            var viewModel = new CommunityViewModel();
            viewModel.Students = await _context.Students
                  .Include(i => i.CommunityMemberships).ThenInclude(i => i.Community)
                  .AsNoTracking()
                  .OrderBy(i => i.LastName)
                  .ToListAsync();

            if (Id != null)
            {
                ViewData["StudentId"] = Id;
                viewModel.CommunityMemberships = viewModel.Students.Where(
                    x => (x.Id == Id)).Single().CommunityMemberships;

            }

            return View(viewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.CommunityMemberships)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        public async Task<IActionResult> EditMembership(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var viewModel = new StudentMembershipViewModel();
            viewModel.Student = await _context.Students
                .Include(s => s.CommunityMemberships).ThenInclude(s => s.Community)
                .FirstOrDefaultAsync(m => m.Id == Id);

            if (viewModel == null)
            {
                return NotFound();
            }

            var communities = _context.Communities;
            var list = new List<CommunityMembershipViewModel>();

            foreach (var com in communities)
            {
                var mem = new CommunityMembershipViewModel();
                mem.CommunityId = com.Id;
                mem.Title = com.Title;
                if (com.CommunityMemberships != null)
                {
                    mem.IsMember = true;
                }
                else
                {
                    mem.IsMember = false;
                }

                list.Add(mem);

            }
            viewModel.Memberships = list;

            return View(viewModel);
        }

        public async Task<IActionResult> GetStudentID(int? id)
        {
            Student students = await _context.Students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }
            return Ok(students);
        }

        public async Task<IActionResult> GetCommunityID(string id)
        {
            Community communities = await _context.Communities.FindAsync(id);
            if (communities == null)
            {
                return NotFound();
            }
            return Ok(communities);
        }

        
        public async Task<IActionResult> AddMembership(string Id)
        {

            string[] ID = Id.Split(',');
            var cid = ID[0];
            var sid = ID[1];


            if ((Id != null) && (sid != null))
            {
                var communityMembership = new CommunityMembership();
                communityMembership.CommunityId = cid;
                communityMembership.StudentId = Convert.ToInt32(sid);

                _context.CommunityMemberships.Add(communityMembership);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        

        public async Task<IActionResult> RemoveMembership(string Id)
        {
            string[] ID = Id.Split(',');
            var cid = ID[0];
            var sid = ID[1];
                        
           
            if ((Id != null) && (sid != null))
            {
                var communityMembership = await _context.CommunityMemberships.Include(i=> i.Community).FirstOrDefaultAsync(i => i.StudentId.ToString() == sid);
                
                _context.CommunityMemberships.Remove(communityMembership);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        


        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}


