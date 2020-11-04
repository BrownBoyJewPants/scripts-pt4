using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class GJUsersMethods
{ 
public delegate void _VerifyCallback(bool verified);

public delegate void _GetVerifiedCallback(GJUser user);

public delegate void _GetOneCallback(GJUser user);

public delegate void _GetMultipleCallback(GJUser[] users);

private const string USERS_AUTH = "users/auth/";

private const string USERS_FETCH = "users/";

public GJUsersMethods._VerifyCallback VerifyCallback;

public GJUsersMethods._GetVerifiedCallback GetVerifiedCallback;

public GJUsersMethods._GetOneCallback GetOneCallback;

public GJUsersMethods._GetMultipleCallback GetMultipleCallback;

~GJUsersMethods()
{ 
this.VerifyCallback = null;
this.GetVerifiedCallback = null;
this.GetOneCallback = null;
this.GetMultipleCallback = null;
}

public void Verify(string name, string token)
{ 
if (name.Trim() == string.Empty || token.Trim() == string.Empty)
{ 
GJAPI.Instance.GJDebug("Either name or token is empty. Can't verify user.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Verifying user.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("username", name);
dictionary.Add("user_token", token);
GJAPI.User = new GJUser();
GJAPI.User.Name = name;
GJAPI.User.Token = token;
GJAPI.Instance.Request("users/auth/", dictionary, false, new Action<string>(this.ReadVerifyResponse));
dictionary
}

private void ReadVerifyResponse(string response)
{ 
bool flag = GJAPI.Instance.IsResponseSuccessful(response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not verify the user.\n" + response, LogType.Error);
GJAPI.User = null;
}
else
{ 
GJAPI.Instance.GJDebug("User successfully verified.\n" + GJAPI.User.ToString(), LogType.Log);
}
if (this.VerifyCallback != null)
{ 
this.VerifyCallback(flag);
}
flag
}

public void GetVerified()
{ 
if (GJAPI.User == null)
{ 
GJAPI.Instance.GJDebug("There is no verified user. Please verify a user before calling GetVerifiedUser.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Getting verified user.", LogType.Log);
GJAPI.Instance.Request("users/", null, true, new Action<string>(this.ReadGetVerifiedResponse));
}

private void ReadGetVerifiedResponse(string response)
{ 
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not get the verified user.\n" + response, LogType.Error);
GJAPI.User = null;
}
else
{ 
Dictionary<string, string> properties = GJAPI.Instance.ResponseToDictionary(response, false);
GJAPI.Instance.CleanDictionary(ref properties, null);
GJAPI.User.AddProperties(properties, true);
GJAPI.Instance.GJDebug("Verified user successfully fetched.\n" + GJAPI.User.ToString(), LogType.Log);
}
if (this.GetVerifiedCallback != null)
{ 
this.GetVerifiedCallback(GJAPI.User);
}
properties
}

public void Get(string name)
{ 
if (name.Trim() == string.Empty)
{ 
GJAPI.Instance.GJDebug("Name is empty. Can't get user.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Getting user.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("username", name);
GJAPI.Instance.Request("users/", dictionary, false, new Action<string>(this.ReadGetOneResponse));
dictionary
}

public void Get(uint id)
{ 
GJAPI.Instance.GJDebug("Getting user.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("user_id", id.ToString());
GJAPI.Instance.Request("users/", dictionary, false, new Action<string>(this.ReadGetOneResponse));
dictionary
}

private void ReadGetOneResponse(string response)
{ 
GJUser gJUser;
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not get the user.\n" + response, LogType.Error);
gJUser = null;
}
else
{ 
Dictionary<string, string> properties = GJAPI.Instance.ResponseToDictionary(response, false);
GJAPI.Instance.CleanDictionary(ref properties, null);
gJUser = new GJUser(properties);
GJAPI.Instance.GJDebug("User successfully fetched.\n" + gJUser.ToString(), LogType.Log);
}
if (this.GetOneCallback != null)
{ 
this.GetOneCallback(gJUser);
}
gJUser
properties
}

public void Get(uint[] ids)
{ 
if (ids == null)
{ 
GJAPI.Instance.GJDebug("IDs are null. Can't get users.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Getting users.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
string value = string.Join(",", new List<uint>(ids).ConvertAll<string>((uint i) => i.ToString()).ToArray());
dictionary.Add("user_id", value);
GJAPI.Instance.Request("users/", dictionary, false, new Action<string>(this.ReadGetMultipleResponse));
dictionary
value
}

private void ReadGetMultipleResponse(string response)
{ 
GJUser[] array;
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not get the users.\n" + response, LogType.Error);
array = null;
}
else
{ 
Dictionary<string, string>[] array2 = GJAPI.Instance.ResponseToDictionaries(response, false);
GJAPI.Instance.CleanDictionaries(ref array2, null);
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.Append("Users successfully fetched.\n");
int num = array2.Length;
array = new GJUser[num];
for (int i = 0; i < num; i++)
{ 
array[i] = new GJUser(array2[i]);
stringBuilder.Append(array[i].ToString());
}
GJAPI.Instance.GJDebug(stringBuilder.ToString(), LogType.Log);
}
if (this.GetMultipleCallback != null)
{ 
this.GetMultipleCallback(array);
}
array
array2
stringBuilder
num
i
}
}
