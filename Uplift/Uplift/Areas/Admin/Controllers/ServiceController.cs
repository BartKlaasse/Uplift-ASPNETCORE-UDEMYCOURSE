using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.IRepository;
using Uplift.Models;
using Uplift.Models.ViewModels;

namespace Uplift.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {

        private readonly IUnitOfWork _unitOfWOrk;
        private readonly IWebHostEnvironment _hostingEnvironment;

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
            ServiceViewModel serviceVM = new ServiceViewModel()
            {
                Service = new Models.Service(),
                CategoryList = _unitOfWOrk.Category.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWOrk.Frequency.GetFrequencyListForDropDown()
            };
            if (id != null)
            {
                serviceVM.Service = _unitOfWOrk.Service.Get(id.GetValueOrDefault());
            }
            return View(serviceVM);
        }

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new
            {
                data = _unitOfWOrk.Service.GetAll(includeProperties: "Category,Frequency")
            });
        }
        #endregion

    }
}