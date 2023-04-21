using KcalCalc.Models;
using KcalCalc.Data;

namespace KcalCalc.Services
{
    public class KcalDayService : IService<KcalDay>
    {
        private readonly ApplicationDbContext _dbContext;

        public KcalDayService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<KcalDay> GetAll()
        {
            return _dbContext.KcalDays.ToList();
        }

        public KcalDay GetByID(int id)
        {
            KcalDay kcalDay = _dbContext.KcalDays.FirstOrDefault(p => p.ID == id);
            return kcalDay;
        }

        public void Add(KcalDay kcalDay)
        {
            if(kcalDay != null)
            {
                _dbContext.KcalDays.Add(kcalDay);
                _dbContext.SaveChanges();
            }
        }

        public void Update(KcalDay kcalDay)
        {
            if(kcalDay != null)
            {
                _dbContext.KcalDays.Update(kcalDay);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(KcalDay kcalDay)
        {
            if(kcalDay != null)
            {
                _dbContext.KcalDays.Remove(kcalDay);
                _dbContext.SaveChanges();
            }
        }
    }
}