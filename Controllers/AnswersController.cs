using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASK_App.Data;
using ASK_App.Models;
using ASK_App.Models.QuestionAnswersViewModel;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Identity;

namespace ASK_App.Controllers
{
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswersController(ApplicationDbContext context, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _config = config;

        }


        // GET: Answers
        public async Task<IActionResult> Index()
        {
            var user = await GetUserAsync();

            var applicationDbContext = _context.Answer
                .Where(a => a.UserId == user.Id);
               
                
           // return View(await applicationDbContext.ToListAsync());
            return View(await _context.Answer.ToListAsync());
        }

        private Task<ApplicationUser> GetUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        [HttpGet]
        public IActionResult Create(int QuestionId)
        {
            var viewModel = new AnswerCreateViewModel();
            viewModel.Question = _context.Question.Find(QuestionId);
            viewModel.Answer = new Answer()
            {
                QuestionId = QuestionId
            };
            return View(viewModel);
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind("Id,Answers,QuestionId,UserId")] Answer answer)
        {
            ApplicationUser User = await GetCurrentUserAsync();
            answer.UserId = User.Id;
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            { 
                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
            }
            return View(answer);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Answers,QuestionId")] Answer answer)
        {
            
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
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
            return View(answer);
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answer.FindAsync(id);
            _context.Answer.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
            return _context.Answer.Any(e => e.Id == id);
        }
    }
   
}
