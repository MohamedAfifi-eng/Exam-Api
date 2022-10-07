using AutoMapper;
using Exam_Api.DTO;
using Exam_Api.Model;

namespace ExamApi
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Exam, ExamDTO>().ReverseMap();
            CreateMap<Question, QuestionDTO>().ReverseMap();
        }
    }
}
