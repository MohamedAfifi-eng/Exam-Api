using AutoMapper;
using Exam_Api.DTO;
using Exam_Api.Model;
using Exam_Api.Repos;
using Microsoft.AspNetCore.Identity;
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
                ResponseDTO<bool> resp = new ResponseDTO<bool>() { Message = "Exam Not Found" };
                return NotFound(resp);
            }
            string userid = _userManager.GetUserId(User);
            if (exam.createrId_FK != userid)
            {
                ResponseDTO<bool> resp = new ResponseDTO<bool>() { Message = "You Have Not Permission to Access this Resources" };
                return StatusCode(StatusCodes.Status403Forbidden, resp);
            }
            else
            {
                List<QuestionDTO> model = _mapper.Map<List<QuestionDTO>>(_questionService.GetExamQuestions(examid).ToList());
                ResponseDTO<List<QuestionDTO>> resp = new ResponseDTO<List<QuestionDTO>>() { Data = model };
                return Ok(model);
            }
        }

        // GET api/<QuestionsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Question? question = _questionService.GetQuestionincloudExam(id);
            ResponseDTO<QuestionDTO> resp = new ResponseDTO<QuestionDTO>();
            if (question == null)
            {
                resp.Message = "Question Not Found";
                return NotFound(resp);
            }

            string userid = _userManager.GetUserId(User);
            if (question.exam?.createrId_FK != userid)
            {
                 resp . Message = "You Have Not Permission to Access this Resources" ;

                return StatusCode(StatusCodes.Status403Forbidden, resp);
            }
            QuestionDTO model = _mapper.Map<QuestionDTO>(question);
            resp.Data = model;
            return Ok(resp);
        }

        // POST api/<QuestionsController>
        [HttpPost]
        public IActionResult Post(QuestionDTO value)
        {
            ResponseDTO<QuestionDTO> resp = new ResponseDTO<QuestionDTO>();
            if (!ModelState.IsValid)
            {
                return BadRequest(resp.Data=value);
            }

            Exam? exam = _examService.Find(value.examId);
            if (exam == null)
            {
                return NotFound(resp.Message= "exam not found" );
            }
            string user = _userManager.GetUserId(User);
            if (exam.createrId_FK != user)
            {
                return StatusCode(StatusCodes.Status403Forbidden, resp.Message = "You are not the owner of this Exam");
            }
            Question entity = _mapper.Map<Question>(value);
            _questionService.Create(entity);
            value = _mapper.Map<QuestionDTO>(entity);
            resp.Data = value;
            return Ok(resp);
        }


        // PUT api/<QuestionsController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] QuestionDTO value)
        {
            var resp = new ResponseDTO<QuestionDTO>();
            if (id != value.id)
            {
                return BadRequest(resp.Message="Please Check The Data");
            }
            Question? question = _questionService.GetQuestionincloudExam(id);
            if (question == null)
            {
                return NotFound(resp.Message = "Question Not Found");
            }
            string userid = _userManager.GetUserId(User);
            if (question.exam?.createrId_FK != userid)
            {
                return StatusCode(StatusCodes.Status403Forbidden, resp.Message = "You are not the Owner Of this Exam");
            }
            Question entity = _mapper.Map<Question>(value);
            _questionService.Update(entity);
            resp.Data = value;
            return Ok(resp);
        }

        // DELETE api/<QuestionsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Question? question = _questionService.GetQuestionincloudExam(id);
            var resp =new ResponseDTO<Boolean>();
            if (question == null)
            {
                return NotFound(resp.Message="Question Not Found");
            }
            string userid = _userManager.GetUserId(User);
            if (question.exam?.createrId_FK != userid)
            {
                return StatusCode(StatusCodes.Status403Forbidden, resp.Message = "You are not the owner of this exam");
            }
            Boolean result = _questionService.Remove(id);
            return result ? Ok(resp.Data=true) : StatusCode(StatusCodes.Status500InternalServerError,resp.Message="internal Error try again later or call the admin");
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
