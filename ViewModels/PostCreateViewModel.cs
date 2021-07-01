using Microsoft.AspNetCore.Http;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.ViewModels
{
    public class PostCreateViewModel
    {
        [Required]
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

        public IFormFile AttachmentName { get; set; }

        public string CurrentAttachmentName { get; set; }

        public string  CurrentFeaturePhotoName { get; set; }

        public IFormFile FeauturePhotoName { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("Blog")]
        public int BlogId { get; set; }


        public Blog Blog { get; set; }
        public Category Category { get; set; }



    }
}
