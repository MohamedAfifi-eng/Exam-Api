namespace Exam_Api.Model
{
    public class StudentExams
    {
        public int id { get; set; }
        public string studentId_FK { get; set; }
        public int examInstanceId_FK { get; set; }
        public int result { get; set; }

        #region Relation
        public ApplicationUser? student { get; set; }
        public ExamInstance? examInstance { get; set; }
        #endregion
    }
}
