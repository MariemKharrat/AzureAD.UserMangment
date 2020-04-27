using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAD.UserMangment.Models
{
    public class AAD_User
    {


        public string objectType { get; set; }
        public string objectId { get; set; }
        public bool accountEnabled { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public DateTime createdDateTime { get; set; }
        public object creationType { get; set; }

        public string displayName { get; set; }
        public string givenName { get; set; }
        public string jobTitle { get; set; }
        public string mail { get; set; }
        public string mailNickname { get; set; }
        public object mobile { get; set; }
        public object passwordPolicies { get; set; }
        public PasswordProfile passwordProfile { get; set; }

        public object showInAddressList { get; set; }

        public object surname { get; set; }

        public string userPrincipalName { get; set; }

        public string userType { get; set; }


    }
    public class PasswordProfile
    {
        public string password { get; set; }
        public bool forceChangePasswordNextLogin { get; set; }
    }
    public class GraphResponse
    {
        public string odatametadata { get; set; }
        public List<Value> value { get; set; }
    }
    public class Value
    {
        public string objectType { get; set; }
        public string objectId { get; set; }
        public bool accountEnabled { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public DateTime createdDateTime { get; set; }
        public object creationType { get; set; }

        public string displayName { get; set; }
        public string givenName { get; set; }
        public string jobTitle { get; set; }
        public string mail { get; set; }
        public string mailNickname { get; set; }
        public object mobile { get; set; }
        public object passwordPolicies { get; set; }
        public PasswordProfile passwordProfile { get; set; }

        public object showInAddressList { get; set; }

        public object surname { get; set; }

        public string userPrincipalName { get; set; }

        public string userType { get; set; }




    }
}
