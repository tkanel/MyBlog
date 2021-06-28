using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }



        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CreatedOn { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UnPublishOn { get; set; }


        public string Author { get; set; }

        public string  AttachmentName { get; set; }

        public string FeauturePhotoName { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("Blog")]
        public int BlogId { get; set; }


        public Blog Blog { get; set; }
        public Category Category { get; set; }


    }
}
