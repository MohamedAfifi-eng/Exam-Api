using AutoMapper;
using Exam_Api.DTO;
using Exam_Api.Model;
using Exam_Api.Repos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exam_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;
        public QuestionsController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        // GET: api/<QuestionsController>
        [HttpGet("examquestions")]
        public IActionResult GetExamQuestions(int examid)
        {
            var model = _mapper.Map<List<QuestionDTO>>(_questionService.GetExamQuestions(examid).ToList());
            return Ok(model);
        }

        // GET api/<QuestionsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question =_mapper.Map<QuestionDTO>(_questionService.Find(id)) ;

            return question!=null?Ok(question):NotFound();
        }

        // POST api/<QuestionsController>
        [HttpPost]
        public void Post([FromBody] QuestionDTO value)
        {

        }

        // PUT api/<QuestionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] QuestionDTO value)
        {
        }

        // DELETE api/<QuestionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
