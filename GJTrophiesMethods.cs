using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class GJTrophiesMethods
{ 
public delegate void _AddTrophyCallback(bool success);

public delegate void _GetOneCallback(GJTrophy trophy);

public delegate void _GetMultipleCallback(GJTrophy[] trophies);

public delegate void _GetAllCallback(GJTrophy[] trophies);

private const string TROPHIES_ADD = "trophies/add-achieved/";

private const string TROPHIES_FETCH = "trophies/";

public GJTrophiesMethods._AddTrophyCallback AddCallback;

public GJTrophiesMethods._GetOneCallback GetOneCallback;

public GJTrophiesMethods._GetMultipleCallback GetMultipleCallback;

public GJTrophiesMethods._GetAllCallback GetAllCallback;

~GJTrophiesMethods()
{ 
this.AddCallback = null;
this.GetOneCallback = null;
this.GetMultipleCallback = null;
this.GetAllCallback = null;
}

public void Add(uint id)
{ 
if (id == 0u)
{ 
GJAPI.Instance.GJDebug("ID is null. Can't add Trophy.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Adding Trophy: " + id + ".", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("trophy_id", id.ToString());
GJAPI.Instance.Request("trophies/add-achieved/", dictionary, true, new Action<string>(this.ReadAddResponse));
dictionary
}

private void ReadAddResponse(string response)
{ 
bool flag = GJAPI.Instance.IsResponseSuccessful(response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not add Trophy.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.GJDebug("Trophy successfully added.", LogType.Log);
}
if (this.AddCallback != null)
{ 
this.AddCallback(flag);
}
flag
}

public void Get(uint id)
{ 
if (id == 0u)
{ 
GJAPI.Instance.GJDebug("ID is null. Can't get trophy.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Getting Trophy: " + id + ".", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("trophy_id", id.ToString());
GJAPI.Instance.Request("trophies/", dictionary, true, new Action<string>(this.ReadGetOneResponse));
dictionary
}

private void ReadGetOneResponse(string response)
{ 
GJTrophy gJTrophy;
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not get the trophy.\n" + response, LogType.Error);
gJTrophy = null;
}
else
{ 
Dictionary<string, string> properties = GJAPI.Instance.ResponseToDictionary(response, false);
GJAPI.Instance.CleanDictionary(ref properties, null);
gJTrophy = new GJTrophy(properties);
GJAPI.Instance.GJDebug("Trophy successfully fetched.\n" + gJTrophy.ToString(), LogType.Log);
}
if (this.GetOneCallback != null)
{ 
this.GetOneCallback(gJTrophy);
}
gJTrophy
properties
}

public void Get(uint[] ids)
{ 
if (ids == null)
{ 
GJAPI.Instance.GJDebug("IDs are null. Can't get trophies.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Getting trophies.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
string value = string.Join(",", new List<uint>(ids).ConvertAll<string>((uint i) => i.ToString()).ToArray());
dictionary.Add("trophy_id", value);
GJAPI.Instance.Request("trophies/", dictionary, true, new Action<string>(this.ReadGetMultipleResponse));
dictionary
value
}

private void ReadGetMultipleResponse(string response)
{ 
GJTrophy[] array;
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not get the trophies.\n" + response, LogType.Error);
array = null;
}
else
{ 
Dictionary<string, string>[] array2 = GJAPI.Instance.ResponseToDictionaries(response, false);
GJAPI.Instance.CleanDictionaries(ref array2, null);
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.Append("Trophies successfully fetched.\n");
int num = array2.Length;
array = new GJTrophy[num];
for (int i = 0; i < num; i++)
{ 
array[i] = new GJTrophy(array2[i]);
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

public void GetAll()
{ 
GJAPI.Instance.GJDebug("Getting all trophies.", LogType.Log);
GJAPI.Instance.Request("trophies/", null, true, new Action<string>(this.ReadGetAllResponse));
}

public void GetAll(bool achieved)
{ 
if (achieved)
{ 
GJAPI.Instance.GJDebug("Getting all trophies the verified user has achieved.", LogType.Log);
}
else
{ 
GJAPI.Instance.GJDebug("Getting all trophies the verified user hasn't achieved.", LogType.Log);
}
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("achieved", achieved.ToString().ToLower());
GJAPI.Instance.Request("trophies/", dictionary, true, new Action<string>(this.ReadGetAllResponse));
dictionary
}

private void ReadGetAllResponse(string response)
{ 
GJTrophy[] array;
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not get the trophies.\n" + response, LogType.Error);
array = null;
}
else
{ 
Dictionary<string, string>[] array2 = GJAPI.Instance.ResponseToDictionaries(response, false);
GJAPI.Instance.CleanDictionaries(ref array2, null);
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.Append("Trophies successfully fetched.\n");
int num = array2.Length;
array = new GJTrophy[num];
for (int i = 0; i < num; i++)
{ 
array[i] = new GJTrophy(array2[i]);
stringBuilder.Append(array[i].ToString());
}
GJAPI.Instance.GJDebug(stringBuilder.ToString(), LogType.Log);
}
if (this.GetAllCallback != null)
{ 
this.GetAllCallback(array);
}
array
array2
stringBuilder
num
i
}
}
