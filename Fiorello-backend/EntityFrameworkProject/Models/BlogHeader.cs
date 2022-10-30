using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Models
{
    public class BlogHeader : BaseEntity
    {
        public string Header { get; set; }
        public string HeaderDesc { get; set; }  


    }
}
