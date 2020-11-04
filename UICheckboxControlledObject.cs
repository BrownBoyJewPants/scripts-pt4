using System;
using UnityEngine;


[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Object")]
public class UICheckboxControlledObject : MonoBehaviour
{ 
public GameObject target;

public bool inverse;

private void OnEnable()
{ 
UICheckbox component = base.GetComponent<UICheckbox>();
if (component != null)
{ 
this.OnActivate(component.isChecked);
}
component
}

private void OnActivate(bool isActive)
{ 
if (this.target != null)
{ 
NGUITools.SetActive(this.target, (!this.inverse) ? isActive : (!isActive));
UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(this.target);
if (uIPanel != null)
{ 
uIPanel.Refresh();
}
}
uIPanel
}
}
