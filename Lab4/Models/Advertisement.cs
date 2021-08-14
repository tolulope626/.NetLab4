using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace Lab4.Models
{
    public class Advertisement : Community
    {
        public int AdvertisementId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "File Name")]
        public String FileName { get; set; }

        [Required]
        [Url]
        public String Url { get; set; }

    }
}