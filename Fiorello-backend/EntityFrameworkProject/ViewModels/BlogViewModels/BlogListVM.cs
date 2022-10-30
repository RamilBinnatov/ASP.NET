using EntityFrameworkProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels.BlogViewModels
{
    public class BlogListVM
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public BlogHeader BlogHeader { get; set; }      
    }
}
