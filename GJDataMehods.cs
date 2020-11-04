using System;
using System.Collections.Generic;
using UnityEngine;


public class GJDataMehods
{ 
public enum UpdateOperation
{ 
Add,
Subtract,
Multiply,
Divide,
Append,
Prepend
}

public delegate void _SetCallback(bool success);

public delegate void _UpdateSuccessCallback(bool success);

public delegate void _UpdateCallback(string data);

public delegate void _GetCallback(string data);

public delegate void _RemoveKey(bool success);

public delegate void _GetKeysCallback(string[] keys);

private const string DATA_FETCH = "data-store/";

private const string DATA_SET = "data-store/set/";

private const string DATA_UPDATE = "data-store/update/";

private const string DATA_REMOVE = "data-store/remove/";

private const string DATA_KEYS = "data-store/get-keys/";

public GJDataMehods._SetCallback SetCallback;

public GJDataMehods._UpdateSuccessCallback UpdateSuccessCallback;

public GJDataMehods._UpdateCallback UpdateCallback;

public GJDataMehods._GetCallback GetCallback;

public GJDataMehods._RemoveKey RemoveKeyCallback;

public GJDataMehods._GetKeysCallback GetKeysCallback;

~GJDataMehods()
{ 
this.SetCallback = null;
this.UpdateSuccessCallback = null;
this.UpdateCallback = null;
this.GetCallback = null;
this.RemoveKeyCallback = null;
this.GetKeysCallback = null;
}

public void Set(string key, string val, bool userData = false)
{ 
if (key.Trim() == string.Empty)
{ 
GJAPI.Instance.GJDebug("Key is empty. Can't add data.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Adding data.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("key", key);
Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
dictionary2.Add("data", val);
GJAPI.Instance.Request("data-store/set/", dictionary, dictionary2, userData, new Action<string>(this.ReadSetResponse));
dictionary
dictionary2
}

private void ReadSetResponse(string response)
{ 
bool flag = GJAPI.Instance.IsResponseSuccessful(response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not add data.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.GJDebug("Data successfully added.", LogType.Log);
}
if (this.SetCallback != null)
{ 
this.SetCallback(flag);
}
flag
}

public void Update(string key, string val, GJDataMehods.UpdateOperation operation, bool userData = false)
{ 
if (key.Trim() == string.Empty)
{ 
GJAPI.Instance.GJDebug("Key is empty. Can't get data.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Updating data.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("key", key);
dictionary.Add("operation", operation.ToString().ToLower());
dictionary.Add("format", "dump");
Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
dictionary2.Add("value", val);
GJAPI.Instance.Request("data-store/update/", dictionary, dictionary2, userData, new Action<string>(this.ReadUpdateResponse));
dictionary
dictionary2
}

private void ReadUpdateResponse(string response)
{ 
string empty = string.Empty;
bool flag = GJAPI.Instance.IsDumpResponseSuccessful(ref response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not update data.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.DumpResponseToString(ref response, out empty);
if (empty == string.Empty)
{ 
GJAPI.Instance.GJDebug("Data successfully updated. However data is empty.", LogType.Warning);
}
else
{ 
GJAPI.Instance.GJDebug("Data successfully updated.\n" + empty, LogType.Log);
}
}
if (this.UpdateSuccessCallback != null)
{ 
this.UpdateSuccessCallback(flag);
}
if (this.UpdateCallback != null)
{ 
this.UpdateCallback(empty);
}
empty
flag
}

public void Get(string key, bool userData = false)
{ 
if (key.Trim() == string.Empty)
{ 
GJAPI.Instance.GJDebug("Key is empty. Can't get data.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Getting data.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("key", key);
dictionary.Add("format", "dump");
GJAPI.Instance.Request("data-store/", dictionary, userData, new Action<string>(this.ReadGetResponse));
dictionary
}

private void ReadGetResponse(string response)
{ 
string empty = string.Empty;
if (!GJAPI.Instance.IsDumpResponseSuccessful(ref response))
{ 
GJAPI.Instance.GJDebug("Could not fetch data.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.DumpResponseToString(ref response, out empty);
if (empty == string.Empty)
{ 
GJAPI.Instance.GJDebug("Data successfully fetched. However data is empty.", LogType.Warning);
}
else
{ 
GJAPI.Instance.GJDebug("Data successfully fetched.\n" + empty, LogType.Log);
}
}
if (this.GetCallback != null)
{ 
this.GetCallback(empty);
}
empty
}

public void RemoveKey(string key, bool userData = false)
{ 
if (key.Trim() == string.Empty)
{ 
GJAPI.Instance.GJDebug("Key is empty. Can't remove key.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Removing key.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("key", key);
GJAPI.Instance.Request("data-store/remove/", dictionary, userData, new Action<string>(this.ReadRemoveKeyResponse));
dictionary
}

private void ReadRemoveKeyResponse(string response)
{ 
bool flag = GJAPI.Instance.IsResponseSuccessful(response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not remove key.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.GJDebug("Key successfully removed.", LogType.Log);
}
if (this.RemoveKeyCallback != null)
{ 
this.RemoveKeyCallback(flag);
}
flag
}

public void GetKeys(bool userKeys = false)
{ 
GJAPI.Instance.GJDebug("Getting data keys.", LogType.Log);
GJAPI.Instance.Request("data-store/get-keys/", null, userKeys, new Action<string>(this.ReadGetKeysResponse));
}

private void ReadGetKeysResponse(string response)
{ 
GJAPI.Instance.GJDebug(response, LogType.Log);
string[] array;
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not get the data keys.\n" + response, LogType.Error);
array = null;
}
else
{ 
Dictionary<string, string> dictionary = GJAPI.Instance.ResponseToDictionary(response, true);
GJAPI.Instance.CleanDictionary(ref dictionary, new string[]
{ 
"success0"
});
array = new string[dictionary.Count];
dictionary.Values.CopyTo(array, 0);
GJAPI.Instance.GJDebug("Keys successfully fetched.\n" + string.Join("\n", array), LogType.Log);
}
if (this.GetKeysCallback != null)
{ 
this.GetKeysCallback(array);
}
array
dictionary
}
}
