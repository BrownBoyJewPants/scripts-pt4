using System;
using UnityEngine;


[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Component")]
public class UICheckboxControlledComponent : MonoBehaviour
{ 
public MonoBehaviour target;

public bool inverse;

private bool mUsingDelegates;

private void Start()
{ 
UICheckbox component = base.GetComponent<UICheckbox>();
if (component != null)
{ 
this.mUsingDelegates = true;
UICheckbox expr_1B = component;
expr_1B.onStateChange = (UICheckbox.OnStateChange)Delegate.Combine(expr_1B.onStateChange, new UICheckbox.OnStateChange(this.OnActivateDelegate));
}
component
expr_1B
}

private void OnActivateDelegate(bool isActive)
{ 
if (base.enabled && this.target != null)
{ 
this.target.enabled = ((!this.inverse) ? isActive : (!isActive));
}
}

private void OnActivate(bool isActive)
{ 
if (!this.mUsingDelegates)
{ 
this.OnActivateDelegate(isActive);
}
}
}
