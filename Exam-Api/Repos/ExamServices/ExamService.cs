using Exam_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Exam_Api.Repos
{
    public class ExamService : IExamService
    {
        private readonly AppDbContext _db;

        public ExamService(AppDbContext db)
        {
            _db = db;
        }

        public Exam Create(Exam entity)
        {
            _db.Exams.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public async Task<Exam> CreateAsync(Exam entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public Exam? Find(int id)
        {
            return GetAll().AsNoTracking().FirstOrDefault(x => x.id == id);
        }

        public async Task<Exam?> FindAsync(int id)
        {
            return await GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
        }

        public IQueryable<Exam> GetAll()
        {
            return _db.Exams;
        }

        public async Task<IEnumerable<Exam>> GetAllAsync()
        {
            return await _db.Exams.ToListAsync();
        }

        public IQueryable<Exam> GetAllByPages(int take, int skip)
        {
            return GetAll().OrderByDescending(a => a.id).Skip(skip).Take(take);
        }

        public async Task<IEnumerable<Exam>> GetAllByPagesAsync(int take, int skip)
        {
            return await GetAll().OrderByDescending(a => a.id).Skip(skip).Take(take).ToListAsync();
        }

        public bool Remove(int id)
        {
            bool result = false;
            Exam? entity = Find(id);
            if (entity != null)
            {
                _db.Exams.Remove(entity);
                result = _db.SaveChanges() > 0 ? true : false;
            }
            return result;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            bool result = false;
            Exam? entity = await FindAsync(id);
            if (entity != null)
            {
                _db.Exams.Remove(entity);
                result = await _db.SaveChangesAsync() > 0 ? true : false;
            }
            return result;
        }

        public Exam Update(Exam entity)
        {
            _db.Exams.Update(entity);
            _db.SaveChanges();
            return entity;
        }

        public async Task<Exam> UpdateAsync(Exam entity)
        {
            _db.Exams.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public IQueryable<Exam> GetExamsForSpecificUser(string ownerId)
        {
            return GetAll().Where(x => x.createrId_FK == ownerId);
        }
        public bool Exist(int id)
        {
            return GetAll().Any(x => x.id == id);
        }
        public string? GetOwnerId(int examId)
        {
            return Find(examId)?.createrId_FK;
        }

    }
}
