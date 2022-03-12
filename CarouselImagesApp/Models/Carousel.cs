using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarouselImagesApp.Models
{
    public class Carousel
    {
        [Key]
        public int CarouselId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
