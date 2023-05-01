using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvernoteClone.Model
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
