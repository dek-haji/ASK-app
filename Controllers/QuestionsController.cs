using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASK_App.Data;
using ASK_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASK_App.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Questions
        public async Task<IActionResult> Index(int? questionTypeId)
        {
            var questions = _context.Question.Include(q => q.QuestionType).Include(q=> q.User).Include(w=> w.Answers).AsQueryable();
                                        
           
            if (questionTypeId != null)
            {
                questions = questions.Where(x => x.QuestionType.QuestionTypeId == questionTypeId);
            }

            var questionTypes = _context.QuestionType;
            
            var QuestionTypeMV = new QuestionTypeViewModel
            {
                QuestionTypes = new SelectList(await questionTypes.ToListAsync(), "QuestionTypeId", "Name", questionTypeId),
                Questions = await questions.ToListAsync(),
                
            };

            return View(QuestionTypeMV);
        }

      

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.Include(q => q.Answers).ThenInclude(a => a.User)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
          
            var productTypeList = _context.QuestionType.ToList();
            var questionTypeSelectList = productTypeList.Select(type => new SelectListItem
            {
                Text = type.Name,
                Value = type.QuestionTypeId.ToString()
            }).ToList();
            questionTypeSelectList.Insert(0, new SelectListItem
            {
                Text = "Choose Category...",
                Value = ""
            });

            ViewData["QuestionTypeId"] = questionTypeSelectList;

            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Name,UserId,QuestionTypeId")] Question question)
        {
            var user = await GetUserAsync();
            ModelState.Remove("User");
            ModelState.Remove("QuestionType");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                question.UserId = user.Id;



                _context.Add(question);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var QuestionTypeList = new SelectList(_context.QuestionType, "QuestionTypeId", "Name", question.QuestionTypeId);

            ViewData["QuestionTypeId"] = QuestionTypeList;
            return View(question);
        }
        private Task<ApplicationUser> GetUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var question = await _context.Question.Include(q => q.User).Include(q => q.QuestionType).Include(q => q.Answers).FirstOrDefaultAsync(q=> q.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,Name,Title,dateCreated,QuestionTypeId, UserId, Answers")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
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
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
       
    }
}
