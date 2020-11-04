using System;
using System.Collections.Generic;
using System.Text;


public abstract class GJObject
{ 
protected Dictionary<string, string> properties;

public GJObject()
{ 
this.properties = new Dictionary<string, string>();
}

~GJObject()
{ 
this.properties = null;
}

public void AddProperty(string key, string value, bool overwrite = false)
{ 
if (this.properties.ContainsKey(key) && !overwrite)
{ 
return;
}
this.properties[key] = value;
}

public void AddProperties(Dictionary<string, string> properties, bool overwrite = false)
{ 
foreach (KeyValuePair<string, string> current in properties)
{ 
this.AddProperty(current.Key, current.Value, overwrite);
}
enumerator
current
}

public string GetProperty(string key)
{ 
return (!this.properties.ContainsKey(key)) ? string.Empty : this.properties[key];
}

public override string ToString()
{ 
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.AppendFormat(" [{0}]\n", base.GetType().ToString());
foreach (KeyValuePair<string, string> current in this.properties)
{ 
stringBuilder.AppendFormat("{0}: {1}\n", current.Key, current.Value);
}
return stringBuilder.ToString();
stringBuilder
enumerator
current
}
}
