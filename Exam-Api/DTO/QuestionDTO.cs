using System.ComponentModel.DataAnnotations;

namespace Exam_Api.DTO
{
    public class QuestionDTO
    {
        public int id { get; set; }

        [Required]
        public string questionBody { get; set; }

        public string? imgPath { get; set; }

        [Required]
        public byte degreeIfTrue { get; set; }

        [Required]
        public byte degreeIfFalse { get; set; }

        [Required]
        public byte questionType { get; set; }

        [Required]
        public byte numberOfRightAnswers { get; set; }

        [Required]
        public int examId { get; set; }
    }
}
