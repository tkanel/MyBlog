using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class Blog
    {
        public int BlogId { get; set; }

        [Required]
        public string Name { get; set; }

       

        public ICollection<Post> Posts { get; set; }


    }
}
