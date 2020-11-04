using System;
using System.Collections.Generic;
using UnityEngine;


public class GJScore : GJObject
{ 
public string Score
{ 
get
{ 
return (!this.properties.ContainsKey("score")) ? string.Empty : this.properties["score"];
}
set
{ 
this.properties["score"] = value;
}
}

public uint Sort
{ 
get
{ 
if (this.properties.ContainsKey("sort"))
{ 
if (this.properties["sort"] == string.Empty)
{ 
Debug.Log("Sort is empty. Returning 0.");
return 0u;
}
try
{ 
uint result = Convert.ToUInt32(this.properties["sort"]);
return result;
}
catch (Exception ex)
{ 
Debug.LogError("Error converting Score Sort to uint. Returning 0. " + ex.Message);
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
this.properties["sort"] = value.ToString();
}
}

public string ExtraData
{ 
get
{ 
return (!this.properties.ContainsKey("extra_data")) ? string.Empty : this.properties["extra_data"];
}
set
{ 
this.properties["extra_data"] = value;
}
}

public string Username
{ 
get
{ 
return (!this.properties.ContainsKey("user")) ? string.Empty : this.properties["user"];
}
set
{ 
this.properties["user"] = value;
}
}

public uint UserID
{ 
get
{ 
if (this.properties.ContainsKey("user_id"))
{ 
if (this.properties["user_id"] == string.Empty)
{ 
Debug.Log("User ID is empty. Returning 0.");
return 0u;
}
try
{ 
uint result = Convert.ToUInt32(this.properties["user_id"]);
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
this.properties["user_id"] = value.ToString();
}
}

public string Guestname
{ 
get
{ 
return (!this.properties.ContainsKey("guest")) ? string.Empty : this.properties["guest"];
}
set
{ 
this.properties["guest"] = value;
}
}

public string Name
{ 
get
{ 
return (!this.isUserScore) ? this.Guestname : this.Username;
}
}

public string Stored
{ 
get
{ 
return (!this.properties.ContainsKey("stored")) ? string.Empty : this.properties["stored"];
}
set
{ 
this.properties["stored"] = value;
}
}

public bool isUserScore
{ 
get
{ 
return this.properties.ContainsKey("user") && this.properties["user"] != string.Empty;
}
}

public GJScore()
{ 
}

public GJScore(string score, uint sort)
{ 
base.AddProperty("score", score, false);
base.AddProperty("sort", sort.ToString(), false);
}

public GJScore(Dictionary<string, string> properties)
{ 
base.AddProperties(properties, false);
}
}
