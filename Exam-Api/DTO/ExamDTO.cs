using System.ComponentModel.DataAnnotations;

namespace Exam_Api.DTO
{
    public class ExamDTO
    {
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string title { get; set; }
        [Required]
        [StringLength(maximumLength: 200, MinimumLength = 3)]
        public string descreption { get; set; }

    }
}
