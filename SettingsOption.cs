using System;
using System.Collections.Generic;
using System.Linq;


namespace Assets.Scripts.Settings
{ 
public abstract class SettingsOption
{ 
private string[] values;

protected string value;

public string[] Values
{ 
get
{ 
return this.values;
}
}

public string Value
{ 
get
{ 
return this.value;
}
set
{ 
this.value = value;
}
}

public SettingsOption()
{ 
this.values = this.GetValues().ToArray<string>();
}

public void SetValue(string value)
{ 
this.value = value;
}

public abstract void Apply();

protected abstract IEnumerable<string> GetValues();
}
}
