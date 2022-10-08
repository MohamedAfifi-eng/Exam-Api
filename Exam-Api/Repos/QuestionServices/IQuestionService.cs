using Exam_Api.Model;

namespace Exam_Api.Repos
{
    public interface IQuestionService:IGeneralService<Question>
    {
        public IQueryable<Question> GetExamQuestions(int examid);
        public Question? GetQuestionincloudExam(int questionId);
    }
}
