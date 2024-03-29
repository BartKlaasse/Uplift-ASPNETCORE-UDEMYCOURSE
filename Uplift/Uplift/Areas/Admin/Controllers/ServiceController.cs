using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.IRepository;
using Uplift.Models;
using Uplift.Models.ViewModels;

namespace Uplift.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ServiceController : Controller
    {

        private readonly IUnitOfWork _unitOfWOrk;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public ServiceViewModel ServiceVM { get; set; }

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWOrk = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServiceVM = new ServiceViewModel()
            {
                Service = new Models.Service(),
                CategoryList = _unitOfWOrk.Category.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWOrk.Frequency.GetFrequencyListForDropDown()
            };
            if (id != null)
            {
                ServiceVM.Service = _unitOfWOrk.Service.Get(id.GetValueOrDefault());
            }
            return View(ServiceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (ServiceVM.Service.Id == 0)
                {
                    // new service
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    ServiceVM.Service.ImageUrl = @"images\services\" + fileName + extension;
                    _unitOfWOrk.Service.Add(ServiceVM.Service);
                }
                else
                {
                    //Edit service

                    var serviceFromDb = _unitOfWOrk.Service.Get(ServiceVM.Service.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        ServiceVM.Service.ImageUrl = @"images\services\" + fileName + extension_new;
                    }
                    else
                    {
                        ServiceVM.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }
                    _unitOfWOrk.Service.Update(ServiceVM.Service);
                }
                _unitOfWOrk.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ServiceVM.CategoryList = _unitOfWOrk.Category.GetCategoryListForDropDown();
                ServiceVM.FrequencyList = _unitOfWOrk.Frequency.GetFrequencyListForDropDown();
                return View(ServiceVM);
            }
        }

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new
            {
                data = _unitOfWOrk.Service.GetAll(includeProperties: "Category,Frequency")
            });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var serviceFromDb = _unitOfWOrk.Service.Get(id);
            string webRootPath = _hostingEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (serviceFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWOrk.Service.Remove(serviceFromDb);
            _unitOfWOrk.Save();

            return Json(new { success = true, message = "Delete success" });

        }
        #endregion

    }
}