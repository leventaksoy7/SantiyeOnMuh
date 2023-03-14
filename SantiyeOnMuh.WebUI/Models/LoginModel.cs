using System.ComponentModel.DataAnnotations;

namespace SantiyeOnMuh.WebUI.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
