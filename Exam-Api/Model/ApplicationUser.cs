using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Exam_Api.Model
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50, MinimumLength = 3)]
        public string FullName { get; set; }

        #region Relations
        public List<Exam>? createdExams_Exams { get; set; }
        public List<StudentExams>? studentExams { get; set; }
        #endregion
    }
}