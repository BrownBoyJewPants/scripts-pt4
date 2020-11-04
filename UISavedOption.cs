using System;
using UnityEngine;


[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{ 
public string keyName;

private UIPopupList mList;

private UICheckbox mCheck;

private string key
{ 
get
{ 
return (!string.IsNullOrEmpty(this.keyName)) ? this.keyName : ("NGUI State: " + base.name);
}
}

private void Awake()
{ 
this.mList = base.GetComponent<UIPopupList>();
this.mCheck = base.GetComponent<UICheckbox>();
if (this.mList != null)
{ 
UIPopupList expr_2F = this.mList;
expr_2F.onSelectionChange = (UIPopupList.OnSelectionChange)Delegate.Combine(expr_2F.onSelectionChange, new UIPopupList.OnSelectionChange(this.SaveSelection));
}
if (this.mCheck != null)
{ 
UICheckbox expr_67 = this.mCheck;
expr_67.onStateChange = (UICheckbox.OnStateChange)Delegate.Combine(expr_67.onStateChange, new UICheckbox.OnStateChange(this.SaveState));
}
expr_2F
expr_67
}

private void OnDestroy()
{ 
if (this.mCheck != null)
{ 
UICheckbox expr_17 = this.mCheck;
expr_17.onStateChange = (UICheckbox.OnStateChange)Delegate.Remove(expr_17.onStateChange, new UICheckbox.OnStateChange(this.SaveState));
}
if (this.mList != null)
{ 
UIPopupList expr_4F = this.mList;
expr_4F.onSelectionChange = (UIPopupList.OnSelectionChange)Delegate.Remove(expr_4F.onSelectionChange, new UIPopupList.OnSelectionChange(this.SaveSelection));
}
expr_17
expr_4F
}

private void OnEnable()
{ 
if (this.mList != null)
{ 
string @string = PlayerPrefs.GetString(this.key);
if (!string.IsNullOrEmpty(@string))
{ 
this.mList.selection = @string;
}
return;
}
if (this.mCheck != null)
{ 
this.mCheck.isChecked = (PlayerPrefs.GetInt(this.key, 1) != 0);
}
else
{ 
string string2 = PlayerPrefs.GetString(this.key);
UICheckbox[] componentsInChildren = base.GetComponentsInChildren<UICheckbox>(true);
int i = 0;
int num = componentsInChildren.Length;
while (i < num)
{ 
UICheckbox uICheckbox = componentsInChildren[i];
uICheckbox.isChecked = (uICheckbox.name == string2);
i++;
}
}
string
string2
componentsInChildren
i
num
uICheckbox
}

private void OnDisable()
{ 
if (this.mCheck == null && this.mList == null)
{ 
UICheckbox[] componentsInChildren = base.GetComponentsInChildren<UICheckbox>(true);
int i = 0;
int num = componentsInChildren.Length;
while (i < num)
{ 
UICheckbox uICheckbox = componentsInChildren[i];
if (uICheckbox.isChecked)
{ 
this.SaveSelection(uICheckbox.name);
break;
}
i++;
}
}
componentsInChildren
i
num
uICheckbox
}

private void SaveSelection(string selection)
{ 
PlayerPrefs.SetString(this.key, selection);
}

private void SaveState(bool state)
{ 
PlayerPrefs.SetInt(this.key, (!state) ? 0 : 1);
}
}
