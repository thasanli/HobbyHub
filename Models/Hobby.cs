using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HobbyApp.Models
{
    public class Hobby
    {
        [Key]
        public int HobbyId {get;set;}
        [Required(ErrorMessage="*** Hobby name is required ***")]
        public string HobbyName {get;set;}
        [Required(ErrorMessage="*** Hobby description is required ***")]
        public string Description {get;set;}
        public int UserId {get;set;}
        public List<Euthusiasts> Euthusiasts {get;set;}
        
        
        
    }
}