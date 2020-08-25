using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HobbyApp.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="*** Username required to Log in ***")]
        [MinLength(3, ErrorMessage = "*** Usernname a valid Email address ***")]
        public string LoginUserName{get;set;}
        

        [Required(ErrorMessage="*** Password is required to Log in ***")]
        [MinLength(8, ErrorMessage = "*** Password must contain at least 8 characters ***")]
        [DataType(DataType.Password, ErrorMessage = "*** Invalid Password ***")]
        public string LoginPassword{get;set;}
    }
}