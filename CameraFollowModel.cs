using System;
using UnityEngine;


public class CameraFollowModel : CameraBase
{ 
public GameObject model;

private void Update()
{ 
if (this.model == null)
{ 
return;
}
Vector3 position = this.model.transform.position;
position.y = 1.5f;
this.targetPosition = new Vector3?(position);
base.MoveToTarget(true);
position
}
}
