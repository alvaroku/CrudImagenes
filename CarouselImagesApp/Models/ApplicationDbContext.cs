using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarouselImagesApp.Models
{
    public class ApplicationDbContext: DbContext
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            { }
            public DbSet<Carousel> Carousel { get; set; }
    }
}
