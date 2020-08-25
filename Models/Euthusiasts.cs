using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HobbyApp.Models
{
    public class Euthusiasts
    {
        [Key]
        public int EuthusiastsId {get;set;}
        public string Proficiency{get;set;}
        public int HobbyId{get;set;}
        public int UserID {get;set;}
        public User User {get;set;}
        public Hobby Hobbies {get;set;}
    }
}