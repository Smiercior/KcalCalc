using KcalCalc.Models;
using KcalCalc.Data;

namespace KcalCalc.Services
{
    public class ProductService : IService<Product>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Product> GetAll()
        {
            return _dbContext.Products.ToList();
        }

        public Product GetByID(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(p => p.ID == id);
            return product;
        }

        public void Add(Product product)
        {
            if(product != null)
            {
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
            }
        }

        public void Update(Product product)
        {
            if(product != null)
            {
                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Product product)
        {
            if(product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
        }
    }
}