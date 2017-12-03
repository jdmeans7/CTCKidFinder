using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chirst_Temple_Kid_Finder.Models
{
    public class LocationModels
    {
        public string Name { get; set; }
        public List<CodeTable> Ctable { get; set; }
        public List<CodeAssignTable> CaTable { get; set; }
    }
}