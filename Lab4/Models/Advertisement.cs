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
        public Advertisement advertisement { get; set; }
    }
}