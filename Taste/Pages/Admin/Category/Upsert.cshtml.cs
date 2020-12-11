using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Pages.Admin.Category
{
    public class Upsert : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Upsert(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Models.Category categoryObj;

        public IActionResult OnGet(int? id)
        {
            categoryObj = new Models.Category();
            if (id != null)
            {
                categoryObj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
                if (categoryObj == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost(Models.Category categoryObj)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (categoryObj.Id == 0)
            {
                _unitOfWork.Category.Add(categoryObj);
            }
            else
            {
                _unitOfWork.Category.Update(categoryObj);
            }

            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }
}