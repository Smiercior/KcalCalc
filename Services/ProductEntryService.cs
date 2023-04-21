using KcalCalc.Models;
using KcalCalc.Data;

namespace KcalCalc.Services
{
    public class ProductEntryService : IService<ProductEntry>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductEntryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ProductEntry> GetAll()
        {
            return _dbContext.ProductEntries.ToList();
        }

        public ProductEntry GetByID(int id)
        {
            ProductEntry productEntry = _dbContext.ProductEntries.FirstOrDefault(p => p.ID == id);
            return productEntry;
        }

        public void Add(ProductEntry productEntry)
        {
            if(productEntry != null)
            {
                _dbContext.ProductEntries.Add(productEntry);
                _dbContext.SaveChanges();
            }
        }

        public void Update(ProductEntry productEntry)
        {
            if(productEntry != null)
            {
                _dbContext.ProductEntries.Update(productEntry);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(ProductEntry productEntry)
        {
            if(productEntry != null)
            {
                _dbContext.ProductEntries.Remove(productEntry);
                _dbContext.SaveChanges();
            }
        }
    }
}