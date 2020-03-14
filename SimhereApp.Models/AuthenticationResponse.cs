using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimhereApp.Models
{
    public class AuthenticationResponse
    {
        public Guid id { get; set; }
        public string auth_token { get; set; }
        public int expires_in { get; set; }
    }
}
