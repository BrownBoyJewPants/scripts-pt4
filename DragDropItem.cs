using System;
using UnityEngine;


[AddComponentMenu("NGUI/Examples/Drag and Drop Item")]
public class DragDropItem : MonoBehaviour
{ 
public GameObject prefab;

private Transform mTrans;

private bool mIsDragging;

private bool mSticky;

private Transform mParent;

private void UpdateTable()
{ 
UITable uITable = NGUITools.FindInParents<UITable>(base.gameObject);
if (uITable != null)
{ 
uITable.repositionNow = true;
}
uITable
}

private void Drop()
{ 
Collider collider = UICamera.lastHit.collider;
DragDropContainer dragDropContainer = (!(collider != null)) ? null : collider.gameObject.GetComponent<DragDropContainer>();
if (dragDropContainer != null)
{ 
this.mTrans.parent = dragDropContainer.transform;
Vector3 localPosition = this.mTrans.localPosition;
localPosition.z = 0f;
this.mTrans.localPosition = localPosition;
}
else
{ 
this.mTrans.parent = this.mParent;
}
this.UpdateTable();
NGUITools.MarkParentAsChanged(base.gameObject);
collider
dragDropContainer
localPosition
}

private void Awake()
{ 
this.mTrans = base.transform;
}

private void OnDrag(Vector2 delta)
{ 
if (base.enabled && UICamera.currentTouchID > -2)
{ 
if (!this.mIsDragging)
{ 
this.mIsDragging = true;
this.mParent = this.mTrans.parent;
this.mTrans.parent = DragDropRoot.root;
Vector3 localPosition = this.mTrans.localPosition;
localPosition.z = 0f;
this.mTrans.localPosition = localPosition;
NGUITools.MarkParentAsChanged(base.gameObject);
}
else
{ 
this.mTrans.localPosition += delta;
}
}
localPosition
}

private void OnPress(bool isPressed)
{ 
if (base.enabled)
{ 
if (isPressed)
{ 
if (!UICamera.current.stickyPress)
{ 
this.mSticky = true;
UICamera.current.stickyPress = true;
}
}
else if (this.mSticky)
{ 
this.mSticky = false;
UICamera.current.stickyPress = false;
}
this.mIsDragging = false;
Collider collider = base.collider;
if (collider != null)
{ 
collider.enabled = !isPressed;
}
if (!isPressed)
{ 
this.Drop();
}
}
collider
}
}
