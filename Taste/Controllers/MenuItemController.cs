using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public MenuItemController(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new
            {
                data = _unitOfWork.MenuItem.GetAll(null, null, "Category,FoodType")
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objFromDb = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if (objFromDb == null)
                {
                    return NotFound();
                }

                var pathFromDb = objFromDb.Image.TrimStart('\\');
                var imagePath = Path.Combine(_environment.WebRootPath, pathFromDb);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                Console.WriteLine(imagePath);

                _unitOfWork.MenuItem.Remove(objFromDb);
                _unitOfWork.Save();
            }
            catch (Exception e)
            {
                return Json(new
                {
                    error = true,
                    message = e.Message
                });
            }

            return Ok();
        }
    }
}