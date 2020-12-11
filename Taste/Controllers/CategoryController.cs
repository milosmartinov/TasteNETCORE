using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new
            {
                data = _unitOfWork.Category.GetAll()
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (entity == null)
            {
                return Json(new
                {
                    error = true, message = "Error while deleting!"
                });
            }

            _unitOfWork.Category.Remove(entity);
            _unitOfWork.Save();
            return Json(new
            {
                error = false, message = "Category deleted!"
            });
        }
    }
}