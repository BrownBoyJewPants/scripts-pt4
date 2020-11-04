using System;
using UnityEngine;


namespace Assets.Scripts.Game.MonoBehaviours.Buttons
{ 
public class CheckBoxBehaviour : MonoBehaviour
{ 
public Action<bool> callback;

private void OnActivate()
{ 
if (this.callback != null)
{ 
this.callback(base.GetComponent<UICheckbox>().isChecked);
}
}
}
}
