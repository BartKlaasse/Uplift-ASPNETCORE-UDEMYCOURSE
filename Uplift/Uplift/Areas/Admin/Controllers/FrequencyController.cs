using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.IRepository;
using Uplift.Models;

namespace Uplift.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class FrequencyController : Controller
    {

        private readonly IUnitOfWork _unitOfWOrk;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWOrk = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();
            if (id == null)
            {
                return View(frequency);
            }
            frequency = _unitOfWOrk.Frequency.Get(id.GetValueOrDefault());
            if (frequency == null)
            {
                return NotFound();
            }
            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                if (frequency.Id == 0)
                {
                    _unitOfWOrk.Frequency.Add(frequency);
                }
                else
                {
                    _unitOfWOrk.Frequency.Update(frequency);
                }
                _unitOfWOrk.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWOrk.Frequency.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWOrk.Frequency.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "error while deleting frequency" });
            }
            _unitOfWOrk.Frequency.Remove(objFromDb);
            _unitOfWOrk.Save();
            return Json(new { success = true, message = "Deleting successful" });
        }
        #endregion
    }
}