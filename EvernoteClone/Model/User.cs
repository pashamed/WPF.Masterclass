﻿using System.ComponentModel.DataAnnotations;

namespace EvernoteClone.Model
{
    public class User : IHasId<string>
    {
        [MaxLength(150)]
        public string Id { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Lastname { get; set; }

        public string Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public interface IHasId<T>
    {
        public T Id { get; set; }
    }
}