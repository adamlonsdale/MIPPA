using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIPPA.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
        public bool Valid { get; set; }
        public int Id { get; set; }
        public string ServerDate { get; set; }
    }
}
