using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


public class GJAPI : MonoBehaviour
{ 
private const string PROTOCOL = "http://";

private const string API_ROOT = "gamejolt.com/api/game/";

private static GJAPI instance;

private int gameId;

private string privateKey = string.Empty;

private bool verbose = true;

private int version;

private float timeout = 5f;

private GJUser user;

private GJUsersMethods users;

private GJSessionsMethods sessions;

private GJTrophiesMethods trophies;

private GJScoresMethods scores;

private GJDataMehods data;

public static GJAPI Instance
{ 
get
{ 
if (GJAPI.instance == null)
{ 
GJAPI.instance = new GameObject("_GameJoltAPI").AddComponent<GJAPI>();
UnityEngine.Object.DontDestroyOnLoad(GJAPI.instance.gameObject);
}
return GJAPI.instance;
}
}

public static int GameID
{ 
get
{ 
return GJAPI.Instance.gameId;
}
}

public static string PrivateKey
{ 
get
{ 
return GJAPI.Instance.privateKey;
}
}

public static bool Verbose
{ 
get
{ 
return GJAPI.Instance.verbose;
}
set
{ 
GJAPI.Instance.verbose = value;
}
}

public static int Version
{ 
get
{ 
return GJAPI.Instance.version;
}
set
{ 
GJAPI.Instance.version = value;
}
}

public static float Timeout
{ 
get
{ 
return GJAPI.Instance.timeout;
}
set
{ 
GJAPI.Instance.timeout = value;
}
}

public static GJUser User
{ 
get
{ 
return GJAPI.Instance.user;
}
set
{ 
GJAPI.Instance.user = value;
}
}

public static GJUsersMethods Users
{ 
get
{ 
if (GJAPI.Instance.users == null)
{ 
GJAPI.Instance.users = new GJUsersMethods();
}
return GJAPI.Instance.users;
}
}

public static GJSessionsMethods Sessions
{ 
get
{ 
if (GJAPI.Instance.sessions == null)
{ 
GJAPI.Instance.sessions = new GJSessionsMethods();
}
return GJAPI.Instance.sessions;
}
}

public static GJTrophiesMethods Trophies
{ 
get
{ 
if (GJAPI.Instance.trophies == null)
{ 
GJAPI.Instance.trophies = new GJTrophiesMethods();
}
return GJAPI.Instance.trophies;
}
}

public static GJScoresMethods Scores
{ 
get
{ 
if (GJAPI.Instance.scores == null)
{ 
GJAPI.Instance.scores = new GJScoresMethods();
}
return GJAPI.Instance.scores;
}
}

public static GJDataMehods Data
{ 
get
{ 
if (GJAPI.Instance.data == null)
{ 
GJAPI.Instance.data = new GJDataMehods();
}
return GJAPI.Instance.data;
}
}

private void OnDestroy()
{ 
base.StopAllCoroutines();
this.user = null;
this.users = null;
this.sessions = null;
this.trophies = null;
this.scores = null;
this.data = null;
GJAPI.instance = null;
UnityEngine.Debug.Log("Quit");
}

public static void Init(int gameId, string privateKey, bool verbose = true, int version = 1)
{ 
GJAPI.Instance.gameId = gameId;
GJAPI.Instance.privateKey = privateKey.Trim();
GJAPI.Instance.verbose = verbose;
GJAPI.Instance.version = version;
GJAPI.Instance.user = null;
GJAPI.Instance.GJDebug("Initialisation complete.\n" + GJAPI.Instance.ToString(), LogType.Log);
}

public override string ToString()
{ 
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.AppendLine(" [GJAPI]");
stringBuilder.AppendFormat("Game ID: {0}\n", GJAPI.Instance.gameId.ToString());
stringBuilder.Append("Private Key: [FILTERED]\n");
stringBuilder.AppendFormat("Verbose: {0}\n", GJAPI.Instance.verbose.ToString());
stringBuilder.AppendFormat("Version: {0}\n", GJAPI.Instance.version.ToString());
return stringBuilder.ToString();
stringBuilder
}

public void Request(string method, Dictionary<string, string> parameters, bool requireVerified = false, Action<string> OnResponseComplete = null)
{ 
if (this.gameId == 0 || this.privateKey == string.Empty || this.version == 0)
{ 
this.GJDebug("Please initialise GameJolt API first.", LogType.Error);
if (OnResponseComplete != null)
{ 
OnResponseComplete("Error:\nAPI needs to be initialised first.");
}
return;
}
if (parameters == null)
{ 
parameters = new Dictionary<string, string>();
}
if (requireVerified)
{ 
if (this.user == null)
{ 
this.GJDebug("Authentification required for " + method, LogType.Error);
if (OnResponseComplete != null)
{ 
OnResponseComplete("Error:\nThe method " + method + " requires authentification.");
}
return;
}
parameters.Add("username", this.user.Name);
parameters.Add("user_token", this.user.Token);
}
string requestURL = this.GetRequestURL(method, parameters);
base.StartCoroutine(this.OpenURLAndGetResponse(requestURL, OnResponseComplete));
requestURL
}

public void Request(string method, Dictionary<string, string> parameters, Dictionary<string, string> postParameters, bool requireVerified = false, Action<string> OnResponseComplete = null)
{ 
if (this.gameId == 0 || this.privateKey == string.Empty || this.version == 0)
{ 
this.GJDebug("Please initialise GameJolt API first.", LogType.Error);
if (OnResponseComplete != null)
{ 
OnResponseComplete("Error:\nAPI needs to be initialised first.");
}
return;
}
if (parameters == null)
{ 
parameters = new Dictionary<string, string>();
}
if (requireVerified)
{ 
if (this.user == null)
{ 
this.GJDebug("Authentification required for " + method, LogType.Error);
if (OnResponseComplete != null)
{ 
OnResponseComplete("Error:\nThe method " + method + " requires authentification.");
}
return;
}
parameters.Add("username", this.user.Name);
parameters.Add("user_token", this.user.Token);
}
string requestURL = this.GetRequestURL(method, parameters);
base.StartCoroutine(this.OpenURLAndGetResponse(requestURL, postParameters, OnResponseComplete));
requestURL
}

private string GetRequestURL(string method, Dictionary<string, string> parameters)
{ 
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.Append("http://");
stringBuilder.Append("gamejolt.com/api/game/");
stringBuilder.Append("v");
stringBuilder.Append(this.version);
stringBuilder.Append("/");
stringBuilder.Append(method);
stringBuilder.Append("?game_id=");
stringBuilder.Append(this.gameId);
foreach (KeyValuePair<string, string> current in parameters)
{ 
stringBuilder.Append("&");
stringBuilder.Append(current.Key);
stringBuilder.Append("=");
stringBuilder.Append(current.Value.Replace(" ", "%20"));
}
string signature = this.GetSignature(stringBuilder.ToString());
stringBuilder.Append("&signature=");
stringBuilder.Append(signature);
return stringBuilder.ToString();
stringBuilder
enumerator
current
signature
}

private string GetSignature(string input)
{ 
string text = this.MD5(input + this.privateKey);
if (text.Length != 32)
{ 
text += new string('0', 32 - text.Length);
}
return text;
text
}

private string MD5(string input)
{ 
MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
byte[] array = Encoding.ASCII.GetBytes(input);
array = mD5CryptoServiceProvider.ComputeHash(array);
string text = string.Empty;
for (int i = 0; i < array.Length; i++)
{ 
text += array[i].ToString("x2").ToLower();
}
return text;
mD5CryptoServiceProvider
array
text
i
}

[DebuggerHidden]
private IEnumerator OpenURLAndGetResponse(string url, Action<string> OnResponseComplete = null)
{ 
GJAPI.<OpenURLAndGetResponse>c__Iterator0 <OpenURLAndGetResponse>c__Iterator = new GJAPI.<OpenURLAndGetResponse>c__Iterator0();
<OpenURLAndGetResponse>c__Iterator.url = url;
<OpenURLAndGetResponse>c__Iterator.OnResponseComplete = OnResponseComplete;
<OpenURLAndGetResponse>c__Iterator.<$>url = url;
<OpenURLAndGetResponse>c__Iterator.<$>OnResponseComplete = OnResponseComplete;
<OpenURLAndGetResponse>c__Iterator.<>f__this = this;
return <OpenURLAndGetResponse>c__Iterator;
<OpenURLAndGetResponse>c__Iterator
}

[DebuggerHidden]
private IEnumerator OpenURLAndGetResponse(string url, Dictionary<string, string> postParameters, Action<string> OnResponseComplete = null)
{ 
GJAPI.<OpenURLAndGetResponse>c__Iterator1 <OpenURLAndGetResponse>c__Iterator = new GJAPI.<OpenURLAndGetResponse>c__Iterator1();
<OpenURLAndGetResponse>c__Iterator.url = url;
<OpenURLAndGetResponse>c__Iterator.postParameters = postParameters;
<OpenURLAndGetResponse>c__Iterator.OnResponseComplete = OnResponseComplete;
<OpenURLAndGetResponse>c__Iterator.<$>url = url;
<OpenURLAndGetResponse>c__Iterator.<$>postParameters = postParameters;
<OpenURLAndGetResponse>c__Iterator.<$>OnResponseComplete = OnResponseComplete;
<OpenURLAndGetResponse>c__Iterator.<>f__this = this;
return <OpenURLAndGetResponse>c__Iterator;
<OpenURLAndGetResponse>c__Iterator
}

public bool IsResponseSuccessful(string response)
{ 
string[] array = response.Split(new char[]
{ 
'\n'
});
return array[0].Trim().Equals("success:\"true\"");
array
}

public bool IsDumpResponseSuccessful(ref string response)
{ 
int num = response.IndexOf('\n');
if (num == -1)
{ 
this.GJDebug("Wrong response format. Can't read response.", LogType.Error);
return false;
}
string a = response.Substring(0, num).Trim();
return a == "SUCCESS";
num
a
}

public Dictionary<string, string> ResponseToDictionary(string response, bool addIndexToKey = false)
{ 
Dictionary<string, string> dictionary = new Dictionary<string, string>();
string text = string.Empty;
string text2 = string.Empty;
string[] array = response.Split(new char[]
{ 
'\n'
});
int num = array.Length;
for (int i = 0; i < num; i++)
{ 
if (array[i] != string.Empty)
{ 
int num2 = array[i].IndexOf(':');
if (num2 == -1)
{ 
this.GJDebug("Wrong line format. The following line of the response will be skipped: " + array[i], LogType.Warning);
}
else
{ 
text = array[i].Substring(0, num2);
text2 = array[i].Substring(num2 + 1);
text2 = text2.Trim().Trim(new char[]
{ 
'"'
});
if (addIndexToKey)
{ 
dictionary.Add(text + i, text2);
}
else
{ 
dictionary.Add(text, text2);
}
}
}
}
return dictionary;
dictionary
text
text2
array
num
i
num2
}

public Dictionary<string, string>[] ResponseToDictionaries(string response, bool addIndexToKey = false)
{ 
int num = 0;
int num2 = 0;
string text = string.Empty;
string text2 = string.Empty;
string a = string.Empty;
string[] array = response.Split(new char[]
{ 
'\n'
});
int num3 = array.Length;
for (int i = 0; i < num3; i++)
{ 
if (array[i] != string.Empty)
{ 
int num4 = array[i].IndexOf(':');
if (num4 != -1)
{ 
text = array[i].Substring(0, num4);
if (text != "success" && text != "message")
{ 
if (a == string.Empty)
{ 
a = text;
num++;
}
else if (a == text)
{ 
num++;
}
}
}
}
}
a = string.Empty;
Dictionary<string, string>[] array2 = new Dictionary<string, string>[num];
for (int j = 0; j < num; j++)
{ 
array2[j] = new Dictionary<string, string>();
}
for (int k = 0; k < num3; k++)
{ 
if (array[k] != string.Empty)
{ 
int num4 = array[k].IndexOf(':');
if (num4 == 1)
{ 
this.GJDebug("Wrong line format. The following line of the response will be skipped: " + array[k], LogType.Warning);
}
else
{ 
text = array[k].Substring(0, num4);
text2 = array[k].Substring(num4 + 1);
text2 = text2.Trim().Trim(new char[]
{ 
'"'
});
if (text != "success" && text != "message")
{ 
if (a == string.Empty)
{ 
a = text;
}
else if (a == text)
{ 
num2++;
}
}
if (addIndexToKey)
{ 
array2[num2].Add(text + k, text2);
}
else
{ 
array2[num2].Add(text, text2);
}
}
}
}
return array2;
num
num2
text
text2
a
array
num3
i
num4
array2
j
k
}

public void CleanDictionary(ref Dictionary<string, string> dictionary, string[] keysToClean = null)
{ 
if (keysToClean == null)
{ 
keysToClean = new string[]
{ 
"success",
"message"
};
}
int num = keysToClean.Length;
for (int i = 0; i < num; i++)
{ 
if (dictionary.ContainsKey(keysToClean[i]))
{ 
dictionary.Remove(keysToClean[i]);
}
}
num
i
}

public void CleanDictionaries(ref Dictionary<string, string>[] dictionaries, string[] keysToClean = null)
{ 
int num = dictionaries.Length;
for (int i = 0; i < num; i++)
{ 
this.CleanDictionary(ref dictionaries[i], keysToClean);
}
num
i
}

public void DumpResponseToString(ref string response, out string data)
{ 
int num = response.IndexOf('\n');
if (num == -1)
{ 
this.GJDebug("Wrong response format. Can't read response.", LogType.Error);
data = string.Empty;
}
else
{ 
data = response.Substring(num + 1);
}
num
}

public void GJDebug(string message, LogType type = LogType.Log)
{ 
if (!this.verbose)
{ 
return;
}
switch (type)
{ 
case LogType.Error: 
UnityEngine.Debug.LogError("GJAPI: " + message);
return;
case LogType.Assert:
case LogType.Log: 
IL_29:
UnityEngine.Debug.Log("GJAPI: " + message);
return;
case LogType.Warning: 
UnityEngine.Debug.LogWarning("GJAPI: " + message);
return;
 }
goto IL_29;
}
}
