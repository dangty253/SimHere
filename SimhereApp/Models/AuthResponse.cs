using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace SimhereApp.Portable.Models
{
    [Preserve(AllMembers = true)]
    public class AuthResponse
    {
        public string id { get; set; }
        public string auth_token { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public bool? Sex { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
