using System;
using System.Collections.Generic;
using UnityEngine;


public class GJUser : GJObject
{ 
public enum UserType
{ 
Undefined,
User,
Developer,
Moderator,
Admin
}

public enum UserStatus
{ 
Undefined,
Active,
Banned
}

public string Name
{ 
get
{ 
return (!this.properties.ContainsKey("username")) ? string.Empty : this.properties["username"];
}
set
{ 
this.properties["username"] = value;
}
}

public string Token
{ 
get
{ 
return (!this.properties.ContainsKey("user_token")) ? string.Empty : this.properties["user_token"];
}
set
{ 
this.properties["user_token"] = value;
}
}

public uint Id
{ 
get
{ 
if (this.properties.ContainsKey("id"))
{ 
if (this.properties["id"] == string.Empty)
{ 
Debug.Log("User ID is empty. Returning 0.");
return 0u;
}
try
{ 
uint result = Convert.ToUInt32(this.properties["id"]);
return result;
}
catch (Exception ex)
{ 
Debug.LogError("Error converting User ID to uint. Returning 0. " + ex.Message);
uint result = 0u;
return result;
}
return 0u;
}
return 0u;
result
ex
}
set
{ 
this.properties["id"] = value.ToString();
}
}

public GJUser.UserType Type
{ 
get
{ 
if (this.properties.ContainsKey("type"))
{ 
try
{ 
GJUser.UserType result = (GJUser.UserType)((int)Enum.Parse(typeof(GJUser.UserType), this.properties["type"]));
return result;
}
catch (Exception ex)
{ 
Debug.LogError("Error converting User Type to UserType. Value will be Undefined. " + ex.Message);
GJUser.UserType result = GJUser.UserType.Undefined;
return result;
}
return GJUser.UserType.Undefined;
}
return GJUser.UserType.Undefined;
result
ex
}
set
{ 
this.properties["type"] = value.ToString();
}
}

public GJUser.UserStatus Status
{ 
get
{ 
if (this.properties.ContainsKey("status"))
{ 
try
{ 
GJUser.UserStatus result = (GJUser.UserStatus)((int)Enum.Parse(typeof(GJUser.UserStatus), this.properties["status"]));
return result;
}
catch (Exception ex)
{ 
Debug.LogError("Error converting User Status to UserStatus. Value will be Undefined. " + ex.Message);
GJUser.UserStatus result = GJUser.UserStatus.Undefined;
return result;
}
return GJUser.UserStatus.Undefined;
}
return GJUser.UserStatus.Undefined;
result
ex
}
set
{ 
this.properties["status"] = value.ToString();
}
}

public string AvatarURL
{ 
get
{ 
return (!this.properties.ContainsKey("avatar_url")) ? string.Empty : this.properties["avatar_url"];
}
set
{ 
this.properties["avatar_url"] = value;
}
}

public GJUser()
{ 
}

public GJUser(string username, string user_token)
{ 
base.AddProperty("username", username, false);
base.AddProperty("user_token", user_token, false);
}

public GJUser(Dictionary<string, string> properties)
{ 
base.AddProperties(properties, false);
}
}
