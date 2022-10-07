namespace Exam_Api.Repos
{
    public interface IGeneralService<T>
    {
        public T? Find(int id);
        public Task<T?> FindAsync(int id);
        public IQueryable<T> GetAll();
        public Task<IEnumerable<T>> GetAllAsync();
        public IQueryable<T> GetAllByPages(int take, int skip);
        public Task<IEnumerable<T>> GetAllByPagesAsync(int take, int skip);
        public T Update(T entity);
        public Task<T> UpdateAsync(T entity);
        public bool Remove(int id);
        public Task<bool> RemoveAsync(int id);
        public T Create(T entity);
        public Task<T> CreateAsync(T entity);

        public bool Exist(int id);

    }
}
