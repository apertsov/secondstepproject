using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//for attributes
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DistanceLessons.Models
{
    [MetadataType(typeof(LoginMetaData))]
    public partial class Login
    {
    }

    public class LoginMetaData
    {
        [Required]
        [DisplayName("Логін")]
        public string Login { get; set; }

        [Required]
        [DisplayName("Пароль")]
        public string Password { get; set; }
            
        //etc...
        //...
    } 
}