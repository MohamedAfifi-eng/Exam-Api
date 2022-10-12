using AutoMapper;
using Exam_Api.DTO;
using Exam_Api.Model;
using Exam_Api.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exam_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;
        private readonly IExamService _examService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string[] AcceptedFileExtentions = { "jpeg", "jpg", "png" };
        public QuestionsController(IQuestionService questionService,
                                   IMapper mapper,
                                   IExamService examService,
                                   UserManager<ApplicationUser> userManager
                                  )
        {
            _questionService = questionService;
            _mapper = mapper;
            _examService = examService;
            _userManager = userManager;
        }

        // GET: api/<QuestionsController>
        [HttpGet("examquestions")]
        public IActionResult GetExamQuestions(int examid)
        {
            Exam? exam = _examService.Find(examid);
            if (exam == null)
            {
                return NotFound();
            }
            string userid = _userManager.GetUserId(User);
            if (exam.createrId_FK != userid)
            {
                return Forbid();
            }
            List<QuestionDTO> model = _mapper.Map<List<QuestionDTO>>(_questionService.GetExamQuestions(examid).ToList());
            return Ok(model);
        }

        // GET api/<QuestionsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Question? question = _questionService.GetQuestionincloudExam(id);
            if (question == null)
            {
                return NotFound();
            }

            string userid = _userManager.GetUserId(User);
            if (question.exam?.createrId_FK != userid)
            {
                return Forbid();
            }
            QuestionDTO model = _mapper.Map<QuestionDTO>(question);

            return Ok(model);
        }

        // POST api/<QuestionsController>
        [HttpPost]
        public IActionResult Post( QuestionDTO value)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new {msh="modelstat is not valid"});
            }

            var exam= _examService.Find(value.examId);
            if (exam==null)
            {
                return BadRequest(new { msh = "exam not found" });
            }
            var user = _userManager.GetUserId(User);
            if (exam.createrId_FK!= user)
            {
                return Forbid();
            }
            var entity= _mapper.Map<Question>(value);
            _questionService.Create(entity);
            value = _mapper.Map<QuestionDTO>(entity);
            return Ok(value);
        }


        // PUT api/<QuestionsController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody] QuestionDTO value)
        {
            if (id!=value.id)
            {
                return BadRequest();
            }
            var question= _questionService.GetQuestionincloudExam(id);
            if (question==null)
            {
                return NotFound();
            }
            var userid = _userManager.GetUserId(User);
            if (question.exam?.createrId_FK!=userid)
            {
                return Forbid();
            }
            var entity = _mapper.Map<Question>(value);
            _questionService.Update(entity);
            return Ok(value);
        }

        // DELETE api/<QuestionsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Question? question = _questionService.GetQuestionincloudExam(id);
            if (question == null)
            {
                return NotFound();
            }
            string userid = _userManager.GetUserId(User);
            if (question.exam?.createrId_FK != userid)
            {
                return Forbid();
            }
            Boolean result = _questionService.Remove(id);
            return result ? NoContent() : UnprocessableEntity();
        }
       // [HttpPost("updateimg/{id}")]
        //public  IActionResult updateimg(IFormFile img ,int id) 
        //{
        //    if (img != null)
        //    {
        //        string path = $"wwwroot/{id}/{Guid.NewGuid().ToString()}";
        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            if (AcceptedFileExtentions.Contains(Path.GetExtension(img.FileName).ToLower())
        //                && img.Length < 5000
        //                )
        //            {
        //                img.CopyTo(stream);
        //                entity.imgPath = path;
        //            }

        //        }
        //    }


        //    return Ok(new {imgname=img.FileName,questionID=id});
        //}
    }
}
