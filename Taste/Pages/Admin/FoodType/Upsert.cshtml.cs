using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Pages.Admin.FoodType
{
    public class Upsert : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Upsert(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Models.FoodType FoodTypeObj;

        public IActionResult OnGet(int? id)
        {
            FoodTypeObj = new Models.FoodType();
            if (id != null)
            {
                FoodTypeObj = _unitOfWork.FoodType.GetFirstOrDefault(u => u.Id == id);
                if (FoodTypeObj == null)
                {
                    return NotFound();
                }

                return Page();
            }

            return Page();
        }

        public IActionResult OnPost(Models.FoodType FoodTypeObj)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (FoodTypeObj.Id == 0)
            {
                _unitOfWork.FoodType.Add(FoodTypeObj);
            }

            _unitOfWork.FoodType.Update(FoodTypeObj);
            _unitOfWork.Save();
            return RedirectToPage("Index");
        }
    }
}