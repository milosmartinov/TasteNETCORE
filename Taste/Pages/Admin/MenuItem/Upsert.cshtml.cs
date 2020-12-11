using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models.ViewModels;

namespace Taste.Pages.Admin.MenuItem
{
    public class Upsert : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public Upsert(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        [BindProperty] public MenuItemVM MenuItem { get; set; }

        public IActionResult OnGet(int? id)
        {
            MenuItem = new MenuItemVM()
            {
                Category = _unitOfWork.Category.GetCategoryListForDropDown(),
                FoodType = _unitOfWork.FoodType.GetFoodTypeListForDropdown(),
                MenuItem = new Models.MenuItem()
            };
            if (id == null)
            {
                return Page();
            }

            MenuItem.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
            if (MenuItem.MenuItem == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            string webRootPath = _environment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (MenuItem.MenuItem.Id == 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\menuItems");
                var extension = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                MenuItem.MenuItem.Image = @"\images\menuItems\" + fileName + extension;
                _unitOfWork.MenuItem.Add(MenuItem.MenuItem);
            }
            else
            {
                var objFromDb = _unitOfWork.MenuItem.Get(MenuItem.MenuItem.Id);
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\menuItems");
                    var extension = Path.GetExtension(files[0].FileName);
                    var imagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (var fileStream =
                        new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    MenuItem.MenuItem.Image = @"\images\menuItems\" + fileName + extension;
                    _unitOfWork.MenuItem.Update(MenuItem.MenuItem);
                }
                else
                {
                    MenuItem.MenuItem.Image = objFromDb.Image;
                }

                _unitOfWork.MenuItem.Update(MenuItem.MenuItem);
            }

            _unitOfWork.Save();
            return RedirectToPage("Index");
        }
    }
}