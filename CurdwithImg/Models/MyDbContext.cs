using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CurdwithImg.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("MyCon")
        {

        }
        public DbSet<Student> Students { get; set; }
    }
}