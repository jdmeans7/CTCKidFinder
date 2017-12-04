using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chirst_Temple_Kid_Finder.Models
{
    public class MessageViewModel
    {
        [Display(Name = "Child Code: ")]
        [Required]
        [MinLength(5), MaxLength(5)]
        public String KidCode { get; set; }
        [Display(Name = "Subject: ")]
        public String Subject { get; set; }
        [Display(Name = "Message: ")]
        public String Message { get; set; }
    }
}