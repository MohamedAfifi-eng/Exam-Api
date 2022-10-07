using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Api.Model
{
    public class Answer
    {
        public int id { get; set; }
        public string answerBody { get; set; }
        public bool isTrue { get; set; }
        public int questionId_FK { get; set; }


        #region Relation
        [ForeignKey(nameof(questionId_FK))]
        public Question? question { get; set; }
        #endregion
    }
}
