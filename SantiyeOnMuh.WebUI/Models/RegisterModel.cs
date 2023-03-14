using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models
{
    public class RegisterModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }

        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }
    }
}
