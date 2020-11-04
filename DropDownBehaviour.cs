using System;
using UnityEngine;


namespace Assets.Scripts.Game.MonoBehaviours.Buttons
{ 
public class DropDownBehaviour : MonoBehaviour
{ 
public Action<string> callback;

private void OnSelectionChange()
{ 
if (this.callback != null)
{ 
this.callback(base.GetComponent<UIPopupList>().selection);
}
}
}
}
