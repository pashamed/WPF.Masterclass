using EvernoteClone.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    internal class DatabaseHelperContext : DbContext
    {
        DbSet<Note> notes { get; set; }
        DbSet<User> users { get; set; }
        DbSet<Notebook> notebooks { get; set; }

        public DatabaseHelperContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=pashamed\sqlexpress;Initial Catalog=EvernoteClone;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
