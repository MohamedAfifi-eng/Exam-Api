using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Api.Model
{
    public class ExamInstance
    {
        public int id { get; set; }
        public int examId_FK { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public bool isActive { get; set; }
        public byte privacy { get; set; }

        #region Relations
        [ForeignKey(nameof(examId_FK))]
        public Exam? exam { get; set; }
        public List<StudentExams>? studentExams { get; set; }
        #endregion

    }
}
