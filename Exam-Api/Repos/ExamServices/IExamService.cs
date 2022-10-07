using Exam_Api.Model;

namespace Exam_Api.Repos;
public interface IExamService : IGeneralService<Exam>
{
    public IQueryable<Exam> GetExamsForSpecificUser(string ownerId);
    public string? GetOwnerId(int examId);
}

