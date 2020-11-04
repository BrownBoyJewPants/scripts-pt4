using System;
using System.Collections.Generic;
using UnityEngine;


public class GJTable : GJObject
{ 
public uint Id
{ 
get
{ 
if (this.properties.ContainsKey("id"))
{ 
if (this.properties["id"] == string.Empty)
{ 
Debug.Log("Table ID is empty. Returning 0.");
return 0u;
}
try
{ 
uint result = Convert.ToUInt32(this.properties["id"]);
return result;
}
catch (Exception ex)
{ 
Debug.LogError("Error converting Table ID to uint. Returning 0. " + ex.Message);
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

public string Name
{ 
get
{ 
return (!this.properties.ContainsKey("name")) ? string.Empty : this.properties["name"];
}
set
{ 
this.properties["name"] = value;
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

public bool Primary
{ 
get
{ 
return this.properties.ContainsKey("primary") && this.properties["primary"] == "1";
}
set
{ 
this.properties["primary"] = ((!value) ? "0" : "1");
}
}

public GJTable()
{ 
}

public GJTable(uint id, string name, bool primary, string description = "")
{ 
base.AddProperty("id", id.ToString(), false);
base.AddProperty("name", name, false);
base.AddProperty("primary", primary.ToString(), false);
base.AddProperty("description", description, false);
}

public GJTable(Dictionary<string, string> properties)
{ 
base.AddProperties(properties, false);
}
}
