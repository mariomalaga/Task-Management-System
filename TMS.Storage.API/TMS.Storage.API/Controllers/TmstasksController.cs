using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace API.Controllers
{
    public class TmstasksController : Controller
    {
        private readonly TMSTasksContext _context;
        private readonly TMSSubTasksContext _contextSub;
        IConfiguration _configuration;

        public TmstasksController(TMSTasksContext context, TMSSubTasksContext contextSub, IConfiguration configuration)
        {
            _context = context;
            _contextSub = contextSub;
            _configuration = configuration;
        }

        // GET: Tmstasks
        public async Task<IActionResult> Index()
        {
            var tmstasks = await _context.Tmstask.ToListAsync();
            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmstasks) : (IActionResult)Ok(tmstasks);
        }

        [Route("/Tasks")]
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var tmstasks = await _context.Tmstask.ToListAsync();
            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmstasks) : (IActionResult)Ok(tmstasks);
        }

        [Route("/Tasks/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tmstask = await _context.Tmstask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tmstask == null)
            {
                return NotFound();
            }

            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmstask) : (IActionResult)Ok(tmstask);
        }

        [Route("/Subtasks/{id}")]
        [HttpGet]
        public async Task<IActionResult> DetailsSubtasks(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tmssubtask = await _contextSub.Tmssubtask
                .Where(m => m.Id == id).ToListAsync();
            if (tmssubtask == null)
            {
                return NotFound();
            }

            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmssubtask) : (IActionResult)Ok(tmssubtask);
        }

        [Route("/Task/{id}/Subtasks")]
        [HttpGet]
        public async Task<IActionResult> DetailsSubtasksByTask(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tmssubtask = await _contextSub.Tmssubtask
                .Where(m => m.IdTask == id).ToListAsync();
            if (tmssubtask == null)
            {
                return NotFound();
            }

            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmssubtask) : (IActionResult)Ok(tmssubtask);
        }

        [Route("/Task/Create")]
        [HttpPost]
        public async Task<IActionResult> CreateTask(TMStask tmstask)
        {
            if (ModelState.IsValid)
            {
                tmstask.Id = Guid.NewGuid();
                tmstask.State = "Planned";
                tmstask.StartDate = DateTime.Now;
                _context.Add(tmstask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmstask) : (IActionResult)Ok(tmstask);
        }

        [Route("/Subtask/Create")]
        [HttpPost]
        public async Task<IActionResult> CreateSubtask(TMSSubtask tmssubtask)
        {
            if (ModelState.IsValid)
            {
                tmssubtask.Id = Guid.NewGuid();
                tmssubtask.State = "Planned";
                tmssubtask.StartDate = DateTime.Now;
                _contextSub.Add(tmssubtask);
                await _contextSub.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmssubtask) : (IActionResult)Ok(tmssubtask);
        }

        [Route("/Tasks/Edit/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditTask(Guid id, TMStask tmstask)
        {
            tmstask.Id = id;
            var okResultTask = await Details(id) as OkObjectResult;
            var taskState = okResultTask.Value as TMStask;
            var oldState = taskState.State;
            if (tmstask.Id == null)
            {
                return NotFound();
            }
            var okResultList = await DetailsSubtasksByTask(id) as OkObjectResult;
            var listSubtasks = okResultList.Value as List<TMSSubtask>;
            if (ModelState.IsValid)
            {
                try
                {
                    if (listSubtasks.Count > 0)
                    {
                        if(tmstask.State != oldState)
                        {
                            return BadRequest("You can not change the State of the Task because it has Subtasks");
                        }
                        else
                        {
                            _context.Update(tmstask);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        if(tmstask.State == "Completed")
                        {
                            tmstask.FinishDate = DateTime.Now;
                        }
                        _context.Update(tmstask);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TmsTaskExists(tmstask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmstask) : (IActionResult)Ok(tmstask);
        }

        [Route("/Subtasks/Edit/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditSubtask(Guid id, TMSSubtask tmsSubtask)
        {
            tmsSubtask.Id = id;


            if (tmsSubtask.Id == null)
            {
                return NotFound();
            }

            var okResult = await DetailsSubtasks(id) as OkObjectResult;
            var oldSubtask = okResult.Value as List<TMSSubtask>;
            tmsSubtask.IdTask = oldSubtask.First().IdTask;

            if (ModelState.IsValid)
            {
                try
                {
                    _contextSub.Update(tmsSubtask);
                    await _contextSub.SaveChangesAsync();
                    var okResultList = await DetailsSubtasksByTask(tmsSubtask.IdTask) as OkObjectResult;
                    var listSubtasks = okResultList.Value as List<TMSSubtask>;
                    var okResultTask = await Details(tmsSubtask.IdTask) as OkObjectResult;
                    var taskForSubtask = okResultTask.Value as TMStask;
                    await CheckTaskStateAsync(listSubtasks, taskForSubtask);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TmsSubtaskExists(tmsSubtask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View(tmsSubtask) : (IActionResult)Ok(tmsSubtask);
        }

        // GET: Tmstasks/Delete/5
        [Route("/Tasks/Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tmstask = await _context.Tmstask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tmstask == null)
            {
                return NotFound();
            }

            _context.Tmstask.Remove(tmstask);
            await _context.SaveChangesAsync();

            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View() : (IActionResult)Ok();
        }

        [Route("/Subtasks/Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSubtask(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tmsSubtask = await _contextSub.Tmssubtask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tmsSubtask == null)
            {
                return NotFound();
            }

            _contextSub.Tmssubtask.Remove(tmsSubtask);
            await _contextSub.SaveChangesAsync();

            return _configuration.GetValue<bool>("UseView") ? (IActionResult)View() : (IActionResult)Ok();
        }

        private bool TmsTaskExists(Guid id)
        {
            return _context.Tmstask.Any(e => e.Id == id);
        }

        private bool TmsSubtaskExists(Guid id)
        {
            return _contextSub.Tmssubtask.Any(e => e.Id == id);
        }
        public async Task CheckTaskStateAsync(List<TMSSubtask> subtasks, TMStask tmstask)
        {
            bool completeComprobation = true;
            tmstask.State = "Comprobation";
            for (var i = 0; i < subtasks.Count; i++)
            {
                if (subtasks[i].State == "inProgress")
                {
                    tmstask.State = "inProgress";
                }
                else if (subtasks[i].State == "Planned")
                {
                    completeComprobation = false;
                }
            }
            if (tmstask.State != "inProgress" && completeComprobation == true)
            {
                tmstask.State = "Completed";
                tmstask.FinishDate = DateTime.Now;
            }
            else if (tmstask.State != "inProgress" && completeComprobation == false)
            {
                tmstask.State = "Planned";
            }

            await EditTask(tmstask.Id, tmstask);
        }
    }
}
