using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwaj.DAL.Extend;
using Zwaj.DAL.Entity;

namespace Zwaj.DAL.DataBase
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {}

        public DbSet<Photo> MyProperty { get; set; }
    }
}
