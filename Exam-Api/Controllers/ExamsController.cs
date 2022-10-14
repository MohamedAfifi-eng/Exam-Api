using AutoMapper;
using Exam_Api.DTO;
using Exam_Api.Model;
using Exam_Api.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExamsController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        public ExamsController(
            IExamService examService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            AppDbContext db)
        {
            _examService = examService;
            _userManager = userManager;
            _mapper = mapper;
            _db = db;
        }
        [HttpGet]
        public IActionResult GetUserExams()
        {
            string id = _userManager.GetUserId(User);
            List<Exam> model = _examService.GetExamsForSpecificUser(id).ToList();
            IEnumerable<ExamDTO> result = _mapper.Map<IEnumerable<ExamDTO>>(model);
            ResponseDTO<IEnumerable<ExamDTO>> dto = new DTO.ResponseDTO<IEnumerable<ExamDTO>>() { Data = result };
            return Ok(dto);
        }
        [HttpGet("getexam")]
        public IActionResult GetExams(int id)
        {
            Exam? model = _examService.Find(id);
            string userid = _userManager.GetUserId(User);
            if (model.createrId_FK!=userid)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseDTO<int>() { Message = "You dont have permission on this resource" });
            }
            ExamDTO result = _mapper.Map<ExamDTO>(model);
            return Ok(new ResponseDTO<ExamDTO>() { Data = result });
        }

        [HttpPost]
        public IActionResult Create(ExamDTO dto)
        {
            string user = _userManager.GetUserId(User);
            Exam model = _mapper.Map<Exam>(dto);
            model.createrId_FK = user;
            _examService.Create(model);
            return Ok(new ResponseDTO<ExamDTO>() { Message="Exam Created Succesfully ",Data=_mapper.Map<ExamDTO>(model)});
        }

        [HttpPut]
        public IActionResult Update(ExamDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO<int>() { Message = "You sent Not Valid Data !" });
            }
            if (!_examService.Exist(dto.id))
            {
                return Conflict(new ResponseDTO<int>() { Message = "Data Not Found" });
            }
            string user = _userManager.GetUserId(User);
            string? ownerid = _examService.GetOwnerId(dto.id);
            if (user != ownerid)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseDTO<int>() { Message = "You try Updating data for another User" });
            }
            Exam entity = _mapper.Map<Exam>(dto);
            entity.createrId_FK = user;
            _examService.Update(entity);
            return Accepted(new ResponseDTO<ExamDTO>() { Data=dto });
        }

    }
}
