using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Api.Model
{
    public class Question
    {
        public int id { get; set; }
        public string questionBody { get; set; }
        public string? imgPath { get; set; }
        public byte degreeIfTrue { get; set; }
        public byte degreeIfFalse { get; set; }
        public byte questionType { get; set; }
        public byte numberOfRightAnswers { get; set; }
        public int examId { get; set; }

        #region Relations
        [ForeignKey(nameof(examId))]
        public Exam? exam { get; set; }
        public List<Answer>? Answers { get; set; }
        #endregion
    }
}
