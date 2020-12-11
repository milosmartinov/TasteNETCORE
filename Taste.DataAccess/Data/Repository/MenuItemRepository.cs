using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDbContext _db;

        public MenuItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MenuItem entity)
        {
            var objFromDb = _db.MenuItem.FirstOrDefault(u => u.Id == entity.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = entity.Name;
                objFromDb.Description = entity.Description;
                objFromDb.Price = entity.Price;
                if (entity.Image != null)
                {
                    objFromDb.Image = entity.Image;
                }

                objFromDb.CategoryId = entity.CategoryId;
                objFromDb.FoodTypeId = entity.FoodTypeId;
                _db.SaveChanges();
            }
        }
    }
}