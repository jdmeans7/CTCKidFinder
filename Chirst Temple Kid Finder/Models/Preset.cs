using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chirst_Temple_Kid_Finder.Models
{
    public class Preset
    {
        [Key]
        [Display (Name = "Preset Name")]
        public string Name { get; set; }
        public List<CodeTable> Tables { get; set; }
    }

    /*
    public class Preset
    {
        public List<CodeTable> Tables { get; set; }
    }
    */
}