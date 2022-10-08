using Exam_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Exam_Api.Repos
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _db;

        public QuestionService(AppDbContext db)
        {
            _db = db;
        }

        public Question Create(Question entity)
        {
            _db.Questions.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public async Task<Question> CreateAsync(Question entity)
        {
            await _db.Questions.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public bool Exist(int id)
        {
            return Find(id)!=null;
        }

        public Question? Find(int id)
        {
            return _db.Questions.Find(id);
        }

        public async Task<Question?> FindAsync(int id)
        {
            return await _db.Questions.FindAsync(id);
        }

        public IQueryable<Question> GetAll()
        {
            return _db.Questions;
        }

        public  async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await  _db.Questions.ToListAsync();
        }

        public IQueryable<Question> GetAllByPages(int take, int skip)
        {
            return GetAll().OrderByDescending(a=>a.id).Skip(skip).Take(take);
        }

        public async Task<IEnumerable<Question>> GetAllByPagesAsync(int take, int skip)
        {
            return await GetAll().OrderByDescending(a => a.id).Skip(skip).Take(take).ToListAsync();
        }

        public IQueryable<Question> GetExamQuestions(int examid)
        {
            
            return GetAll().Where(x=>x.examId == examid);
        }

        public bool Remove(int id)
        {
            Boolean result = false;
           var model= _db.Questions.Find(id);
            if (model != null)
            {
                _db.Questions.Remove(model);
                result= _db.SaveChanges() > 0;

            }
            return result;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            Boolean result = false;
            var model = _db.Questions.Find(id);
            if (model != null)
            {
                _db.Questions.Remove(model);
                result = await _db.SaveChangesAsync() > 0;

            }
            return  result;
        }

        public Question Update(Question entity)
        {
            _db.Questions.Update(entity);
            _db.SaveChanges();
            return entity;
        }

        public async Task<Question> UpdateAsync(Question entity)
        {
            _db.Questions.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public Question? GetQuestionincloudExam(int questionId)
        {
            return GetAll().AsNoTracking().Where(x => x.id == questionId).Include(x => x.exam).FirstOrDefault();
        }

    }
}
