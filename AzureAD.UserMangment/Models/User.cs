using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAD.UserMangment.Models
{
    public class User
    {
        public string objectId { get; set; }
        public bool accountEnabled { get; set; }
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public Passwordprofile passwordProfile { get; set; }
        public string userPrincipalName { get; set; }
        public User()
        {

        }

    }
    public class Passwordprofile
    {
        public string password { get; set; }
        public bool forceChangePasswordNextLogin { get; set; }
    }
}
