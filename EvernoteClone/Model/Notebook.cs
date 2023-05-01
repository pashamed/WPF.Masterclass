using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvernoteClone.Model
{
    public class Notebook
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
    }
}
