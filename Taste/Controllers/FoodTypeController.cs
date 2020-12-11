using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FoodTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new
            {
                data = _unitOfWork.FoodType.GetAll()
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.FoodType.GetFirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Food type does not exist!"
                });
            }

            _unitOfWork.FoodType.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new
            {
                error = false,
                message = "Food type deleted!"
            });
        }
    }
}