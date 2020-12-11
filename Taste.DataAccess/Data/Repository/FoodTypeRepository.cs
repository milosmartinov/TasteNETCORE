using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public FoodTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetFoodTypeListForDropdown()
        {
            return _db.FoodType.Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
        }

        public void Update(FoodType entity)
        {
            var objFromDb = _db.FoodType.FirstOrDefault(u => u.Id == entity.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = entity.Name;
                _db.SaveChanges();
            }
        }
    }
}