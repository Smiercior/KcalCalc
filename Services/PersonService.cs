using KcalCalc.Models;
using KcalCalc.Data;

namespace KcalCalc.Services
{
    public class PersonService : IService<Person>
    {
        private readonly ApplicationDbContext _dbContext;

        public PersonService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Person> GetAll()
        {
            return _dbContext.Persons.ToList();
        }

        public Person GetByID(int id)
        {
            Person person = _dbContext.Persons.FirstOrDefault(p => p.ID == id);
            return person;
        }

        public void Add(Person person)
        {
            if(person != null)
            {
                _dbContext.Persons.Add(person);
                _dbContext.SaveChanges();
            }
        }

        public void Update(Person person)
        {
            if(person != null)
            {
                _dbContext.Persons.Update(person);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Person person)
        {
            if(person != null)
            {
                _dbContext.Persons.Remove(person);
                _dbContext.SaveChanges();
            }
        }
    }
}