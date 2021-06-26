using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }


        [Display(Name = "Full Name")]
        public String FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public ICollection<CommunityMembership> CommunityMemberships { get; set; }
    }
}
