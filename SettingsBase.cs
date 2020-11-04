using System;
using System.Reflection;
using UnityEngine;


namespace Assets.Scripts.Settings
{ 
public abstract class SettingsBase
{ 
public abstract void Apply();

public void LoadSettings()
{ 
string str = base.GetType().Name + ".";
FieldInfo[] fields = base.GetType().GetFields();
for (int i = 0; i < fields.Length; i++)
{ 
FieldInfo fieldInfo = fields[i];
string key = str + fieldInfo.Name;
if (PlayerPrefs.HasKey(key))
{ 
if (typeof(SettingsOption).IsAssignableFrom(fieldInfo.FieldType))
{ 
SettingsOption settingsOption = (SettingsOption)fieldInfo.GetValue(this);
settingsOption.SetValue(PlayerPrefs.GetString(key));
}
else if (typeof(bool) == fieldInfo.FieldType)
{ 
fieldInfo.SetValue(this, PlayerPrefs.GetInt(key) != 0);
}
else if (typeof(float) == fieldInfo.FieldType)
{ 
fieldInfo.SetValue(this, PlayerPrefs.GetFloat(key));
}
}
}
this.Apply();
str
fields
i
fieldInfo
key
settingsOption
}

public void SaveSettings()
{ 
string str = base.GetType().Name + ".";
FieldInfo[] fields = base.GetType().GetFields();
for (int i = 0; i < fields.Length; i++)
{ 
FieldInfo fieldInfo = fields[i];
string key = str + fieldInfo.Name;
if (typeof(SettingsOption).IsAssignableFrom(fieldInfo.FieldType))
{ 
SettingsOption settingsOption = (SettingsOption)fieldInfo.GetValue(this);
PlayerPrefs.SetString(key, settingsOption.Value);
}
else if (typeof(bool) == fieldInfo.FieldType)
{ 
PlayerPrefs.SetInt(key, (!(bool)fieldInfo.GetValue(this)) ? 0 : 1);
}
else if (typeof(float) == fieldInfo.FieldType)
{ 
PlayerPrefs.SetFloat(key, (float)fieldInfo.GetValue(this));
}
}
PlayerPrefs.Save();
str
fields
i
fieldInfo
key
settingsOption
}
}
}
