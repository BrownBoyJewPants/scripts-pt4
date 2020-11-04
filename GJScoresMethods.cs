using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class GJScoresMethods
{ 
public delegate void _AddCallback(bool success);

public delegate void _GetMultipleCallback(GJScore[] scores);

public delegate void _GetTablesCallback(GJTable[] tables);

private const string SCORES_ADD = "scores/add/";

private const string SCORES_FETCH = "scores/";

private const string SCORES_TABLES = "scores/tables/";

public GJScoresMethods._AddCallback AddCallback;

public GJScoresMethods._GetMultipleCallback GetMultipleCallback;

public GJScoresMethods._GetTablesCallback GetTablesCallback;

~GJScoresMethods()
{ 
this.AddCallback = null;
this.GetMultipleCallback = null;
this.GetTablesCallback = null;
}

public void Add(string score, uint sort, uint table = 0u, string extraData = "")
{ 
if (score.Trim() == string.Empty || sort == 0u)
{ 
GJAPI.Instance.GJDebug("Either score is empty or sort equal to zero (or both). Can't add score.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Adding score for verified user: " + sort, LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("score", score);
dictionary.Add("sort", sort.ToString());
if (extraData.Trim() != string.Empty)
{ 
dictionary.Add("extra_data", extraData);
}
if (table != 0u)
{ 
dictionary.Add("table_id", table.ToString());
}
GJAPI.Instance.Request("scores/add/", dictionary, true, new Action<string>(this.ReadAddResponse));
dictionary
}

public void AddForGuest(string score, uint sort, string name = "Guest", uint table = 0u, string extraData = "")
{ 
if (score.Trim() == string.Empty || sort == 0u || name == string.Empty)
{ 
GJAPI.Instance.GJDebug("Either score is empty or sort equal to zero or name is empty (or all of them). Can't add score.", LogType.Error);
return;
}
GJAPI.Instance.GJDebug("Adding score for guest: " + sort, LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("score", score);
dictionary.Add("sort", sort.ToString());
dictionary.Add("guest", name);
if (extraData.Trim() != string.Empty)
{ 
dictionary.Add("extra_data", extraData);
}
if (table != 0u)
{ 
dictionary.Add("table_id", table.ToString());
}
GJAPI.Instance.Request("scores/add/", dictionary, false, new Action<string>(this.ReadAddResponse));
dictionary
}

private void ReadAddResponse(string response)
{ 
bool flag = GJAPI.Instance.IsResponseSuccessful(response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not add score.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.GJDebug("Score successfully added.", LogType.Log);
}
if (this.AddCallback != null)
{ 
this.AddCallback(flag);
}
flag
}

public void Get(bool ofVerifiedUserOnly = false, uint table = 0u, uint limit = 10u)
{ 
if (limit == 0u)
{ 
GJAPI.Instance.GJDebug("Limit can't be equal to zero. Limit will be set to 1.", LogType.Warning);
limit = 1u;
}
else if (limit > 100u)
{ 
GJAPI.Instance.GJDebug("Limit can't be greater than 100. Limit will be set to 100.", LogType.Warning);
limit = 100u;
}
Dictionary<string, string> dictionary = new Dictionary<string, string>();
dictionary.Add("limit", limit.ToString());
if (table != 0u)
{ 
dictionary.Add("table_id", table.ToString());
}
GJAPI.Instance.Request("scores/", dictionary, ofVerifiedUserOnly, new Action<string>(this.ReadGetResponse));
dictionary
}

private void ReadGetResponse(string response)
{ 
GJScore[] array;
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not fetch scores.\n" + response, LogType.Error);
array = null;
}
else
{ 
Dictionary<string, string>[] array2 = GJAPI.Instance.ResponseToDictionaries(response, false);
GJAPI.Instance.CleanDictionaries(ref array2, null);
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.Append("Scores successfully fetched.\n");
int num = array2.Length;
array = new GJScore[num];
for (int i = 0; i < num; i++)
{ 
array[i] = new GJScore(array2[i]);
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

public void GetTables()
{ 
GJAPI.Instance.GJDebug("Getting score tables.", LogType.Log);
GJAPI.Instance.Request("scores/tables/", null, false, new Action<string>(this.ReadGetTablesResponse));
}

private void ReadGetTablesResponse(string response)
{ 
GJTable[] array;
if (!GJAPI.Instance.IsResponseSuccessful(response))
{ 
GJAPI.Instance.GJDebug("Could not fetch score tables.\n" + response, LogType.Error);
array = null;
}
else
{ 
Dictionary<string, string>[] array2 = GJAPI.Instance.ResponseToDictionaries(response, false);
GJAPI.Instance.CleanDictionaries(ref array2, null);
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.Append("Score Tables successfully fetched.\n");
int num = array2.Length;
array = new GJTable[num];
for (int i = 0; i < num; i++)
{ 
array[i] = new GJTable(array2[i]);
stringBuilder.Append(array[i].ToString());
}
GJAPI.Instance.GJDebug(stringBuilder.ToString(), LogType.Log);
}
if (this.GetTablesCallback != null)
{ 
this.GetTablesCallback(array);
}
array
array2
stringBuilder
num
i
}
}
