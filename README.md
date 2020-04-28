# AzureAD.UserMangment
Manage your Azure AD users

1. Intialiaze the Graph Client
```csharp
 var graphClient = new GraphClient("clientId",
                "clientSecret",
                "tenant");
```
2. GET all users on azure active directory

```csharp
            var users = graphClient.GetAllUsers().Result.ToString();
            List<User> usersLi = JsonConvert.DeserializeObject<List<User>>(users.ToString());
```

3. GET user by email 

```csharp
            var userResult = graphClient.GetAllUsers("achraf123@gridnex.co").Result.ToString();
            List<User> user = JsonConvert.DeserializeObject<List<User>>(userResult.ToString());
```

4. Disable user

```csharp
         var Disablereslt = graphClient.DisableUser(user.First().objectId).Result;
```
5. Enable user

```csharp
        var enablereslt = graphClient.EnableUser(user.First().objectId).Result;
```
6. Create new user

```csharp
          var user = new User()
            {
                accountEnabled = true,
                displayName = "displayName",
                mailNickname = "mailNickname",
                passwordProfile = new Passwordprofile()
                {
                    forceChangePasswordNextLogin = false,
                    password = "password"

                },
                userPrincipalName = "john.doe@domain.com"


            };
            var d = graphClient.CreateUser(c).Result;
```
