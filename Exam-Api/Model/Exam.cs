using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Api.Model
{
    public class Exam
    {
        public int id { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string title { get; set; }
        [StringLength(maximumLength: 200, MinimumLength = 3)]
        public string descreption { get; set; }
        public string createrId_FK { get; set; }

        #region Relations
        [ForeignKey(nameof(createrId_FK))]
        public ApplicationUser? creater_ApplicationUser { get; set; }
        public List<Question>? questions { get; set; }
        public List<ExamInstance>? examInstances { get; set; }
        #endregion
    }
}
