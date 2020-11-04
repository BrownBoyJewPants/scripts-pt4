using Assets.Scripts.Game.Global.MonoBehaviours.Buttons;
using Assets.Scripts.Game.MonoBehaviours.Buttons;
using Assets.Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Game.Gui
{ 
public class GuiBindings
{ 
private class Progressbar
{ 
public UISprite texture;

public float maxsize;
}

private ILookup<string, UILabel> labels;

private ILookup<string, GuiBindings.Progressbar> progressbars;

private ILookup<string, Transform> visibility;

private ILookup<string, UICheckbox> checkboxes;

private ILookup<string, UIPopupList> dropdowns;

private ILookup<string, UISlider> sliders;

private Dictionary<string, float> lastValue;

public static GuiBindings Instance
{ 
get;
private set;
}

public GuiBindings()
{ 
GuiBindings.Instance = this;
this.lastValue = new Dictionary<string, float>();
this.labels = this.Find<UILabel>("GameText").ToLookup((KeyValuePair<string, UILabel> i) => i.Key, (KeyValuePair<string, UILabel> i) => i.Value);
this.progressbars = this.Find<UISprite>("GameProgressbar").ToLookup((KeyValuePair<string, UISprite> i) => i.Key, (KeyValuePair<string, UISprite> i) => new GuiBindings.Progressbar
{ 
texture = i.Value,
maxsize = i.Value.transform.localScale.x
});
this.visibility = this.Find<Transform>("GameVisibility").ToLookup((KeyValuePair<string, Transform> i) => i.Key, (KeyValuePair<string, Transform> i) => i.Value);
this.checkboxes = this.Find<UICheckbox>("OptionsCheckbox").ToLookup((KeyValuePair<string, UICheckbox> i) => i.Key, (KeyValuePair<string, UICheckbox> i) => i.Value);
this.dropdowns = this.Find<UIPopupList>("OptionsDropdown").ToLookup((KeyValuePair<string, UIPopupList> i) => i.Key, (KeyValuePair<string, UIPopupList> i) => i.Value);
this.sliders = this.Find<UISlider>("OptionsSlider").ToLookup((KeyValuePair<string, UISlider> i) => i.Key, (KeyValuePair<string, UISlider> i) => i.Value);
}

public static void Dispose()
{ 
GuiBindings.Instance = null;
}

public void UpdateValue(string property, float value, Action<float> action)
{ 
foreach (UISlider current in this.sliders[property])
{ 
current.sliderValue = value;
SliderBehaviour sliderBehaviour = current.GetComponent<SliderBehaviour>();
if (sliderBehaviour == null)
{ 
sliderBehaviour = current.gameObject.AddComponent<SliderBehaviour>();
sliderBehaviour.slider = current;
sliderBehaviour.labels = this.labels[property];
}
sliderBehaviour.callback = action;
current.eventReceiver = current.gameObject;
}
foreach (UILabel current2 in this.labels[property])
{ 
current2.text = ((int)(value * 100f)).ToString();
}
enumerator
current
sliderBehaviour
enumerator2
current2
}

public void UpdateValue(string property, bool state, Action<bool> action)
{ 
foreach (UICheckbox current in this.checkboxes[property])
{ 
current.isChecked = state;
CheckBoxBehaviour checkBoxBehaviour = current.GetComponent<CheckBoxBehaviour>();
if (checkBoxBehaviour == null)
{ 
checkBoxBehaviour = current.gameObject.AddComponent<CheckBoxBehaviour>();
}
checkBoxBehaviour.callback = action;
current.eventReceiver = current.gameObject;
}
enumerator
current
checkBoxBehaviour
}

public void UpdateValue(string property, SettingsOption option, Action<string> action)
{ 
foreach (UIPopupList current in this.dropdowns[property])
{ 
current.items.Clear();
current.items.AddRange(option.Values);
current.selection = option.Value;
DropDownBehaviour dropDownBehaviour = current.GetComponent<DropDownBehaviour>();
if (dropDownBehaviour == null)
{ 
dropDownBehaviour = current.gameObject.AddComponent<DropDownBehaviour>();
}
dropDownBehaviour.callback = action;
current.eventReceiver = current.gameObject;
}
enumerator
current
dropDownBehaviour
}

public void SetVisible(string property, bool visible)
{ 
foreach (Transform current in this.visibility[property])
{ 
if (current.gameObject.activeSelf != visible)
{ 
current.gameObject.SetActive(visible);
}
}
enumerator
current
}

public void UpdateValue(string property, string value)
{ 
foreach (UILabel current in this.labels[property])
{ 
current.text = value;
}
enumerator
current
}

public void UpdateValue(string property, int value)
{ 
foreach (UILabel current in this.labels[property])
{ 
current.text = value.ToString();
}
enumerator
current
}

public void UpdateValue(string property, float value)
{ 
foreach (UILabel current in this.labels[property])
{ 
current.text = ((int)(value * 100f)).ToString();
}
foreach (UISlider current2 in this.sliders[property])
{ 
current2.sliderValue = value;
}
enumerator
current
enumerator2
current2
}

public void UpdateValue(string property, float value, float maxvalue, bool showMax = true)
{ 
float num;
if (this.lastValue.TryGetValue(property, out num) && num == value)
{ 
return;
}
this.lastValue[property] = value;
foreach (UILabel current in this.labels[property])
{ 
if (showMax)
{ 
current.text = string.Format("{0}/{1}", value, maxvalue);
}
else
{ 
current.text = string.Format("{0}", value);
}
}
foreach (GuiBindings.Progressbar current2 in this.progressbars[property])
{ 
Vector3 localScale = current2.texture.transform.localScale;
if ((double)maxvalue < 0.1 || (double)value < 0.001)
{ 
localScale.x = 0f;
}
else if (value >= current2.maxsize)
{ 
localScale.x = current2.maxsize;
}
else
{ 
localScale.x = Mathf.Min(current2.maxsize, value / maxvalue * current2.maxsize);
}
current2.texture.transform.localScale = localScale;
}
num
enumerator
current
enumerator2
current2
localScale
}

private IEnumerable<KeyValuePair<string, T>> Find<T>(string searchTag) where T : Component
{ 
List<KeyValuePair<string, T>> list = new List<KeyValuePair<string, T>>();
GameObject[] array = GameObject.FindGameObjectsWithTag(searchTag);
for (int i = 0; i < array.Length; i++)
{ 
GameObject gameObject = array[i];
T component = gameObject.GetComponent<T>();
if (!(component == null))
{ 
string tag = this.GetTag(gameObject.name);
if (tag != null)
{ 
list.Add(new KeyValuePair<string, T>(tag, component));
this.lastValue[tag] = -1f;
}
}
}
return list;
list
array
i
gameObject
component
tag
}

private string GetTag(string name)
{ 
int num = name.IndexOf('[');
if (num == -1)
{ 
return null;
}
int num2 = name.IndexOf(']', num);
if (num2 == -1)
{ 
return null;
}
num++;
return name.Substring(num, num2 - num).ToLower();
num
num2
}

public void SetColor(string property, Color32 color)
{ 
foreach (UILabel current in this.labels[property])
{ 
current.color = color;
}
enumerator
current
}
}
}
