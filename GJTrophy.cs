using System;
using System.Collections.Generic;
using UnityEngine;


public class GJTrophy : GJObject
{ 
public enum TrophyDifficulty
{ 
Undefined,
Bronze,
Silver,
Gold,
Platinium
}

public uint Id
{ 
get
{ 
if (this.properties.ContainsKey("id"))
{ 
if (this.properties["id"] == string.Empty)
{ 
Debug.Log("Trophy ID is empty. Returning 0.");
return 0u;
}
try
{ 
uint result = Convert.ToUInt32(this.properties["id"]);
return result;
}
catch (Exception ex)
{ 
Debug.LogError("Error converting Trophy ID to uint. Returning 0. " + ex.Message);
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

public string Title
{ 
get
{ 
return (!this.properties.ContainsKey("title")) ? string.Empty : this.properties["title"];
}
set
{ 
this.properties["title"] = value;
}
}

public string Description
{ 
get
{ 
return (!this.properties.ContainsKey("description")) ? string.Empty : this.properties["description"];
}
set
{ 
this.properties["description"] = value;
}
}

public GJTrophy.TrophyDifficulty Difficulty
{ 
get
{ 
if (this.properties.ContainsKey("difficulty"))
{ 
try
{ 
GJTrophy.TrophyDifficulty result = (GJTrophy.TrophyDifficulty)((int)Enum.Parse(typeof(GJTrophy.TrophyDifficulty), this.properties["difficulty"]));
return result;
}
catch (Exception ex)
{ 
Debug.LogError("Error converting Trophy Difficulty to TrophyDifficulty. Returning Undefined. " + ex.Message);
GJTrophy.TrophyDifficulty result = GJTrophy.TrophyDifficulty.Undefined;
return result;
}
return GJTrophy.TrophyDifficulty.Undefined;
}
return GJTrophy.TrophyDifficulty.Undefined;
result
ex
}
set
{ 
this.properties["difficulty"] = value.ToString();
}
}

public bool Achieved
{ 
get
{ 
return this.properties.ContainsKey("achieved") && !(this.properties["achieved"] == "false");
}
set
{ 
this.properties["achieved"] = value.ToString();
}
}

public string AchievedTime
{ 
get
{ 
if (this.properties.ContainsKey("achieved") && !(this.properties["achieved"] == "false"))
{ 
return this.properties["achieved"];
}
return "NA";
}
set
{ 
this.properties["achieved"] = value;
}
}

public string ImageURL
{ 
get
{ 
return (!this.properties.ContainsKey("image_url")) ? string.Empty : this.properties["image_url"];
}
set
{ 
this.properties["image_url"] = value.ToString();
}
}

public GJTrophy()
{ 
}

public GJTrophy(uint id, string title, GJTrophy.TrophyDifficulty difficulty, bool achieved, string description = "", string imageURL = "")
{ 
base.AddProperty("id", id.ToString(), false);
base.AddProperty("title", title, false);
base.AddProperty("difficulty", difficulty.ToString(), false);
base.AddProperty("achieved", achieved.ToString(), false);
base.AddProperty("description", description, false);
base.AddProperty("image_url", imageURL, false);
}

public GJTrophy(Dictionary<string, string> properties)
{ 
base.AddProperties(properties, false);
}
}
