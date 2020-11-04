using Assets.Scripts.Settings;
using System;
using UnityEngine;


public class CameraBase : MonoBehaviour, ICameraShake
{ 
public static CameraBase instance;

protected Vector3? targetPosition;

protected Vector3 prevTargetPosition;

private static Plane hPlane = new Plane(Vector3.up, Vector3.zero);

public Vector3 Offset
{ 
get;
set;
}

public static void Dispose()
{ 
CameraBase.instance = null;
}

public static void ScrollTo(Vector3 position, bool instantly = false)
{ 
CameraBase.instance.targetPosition = new Vector3?(position);
if (instantly)
{ 
CameraBase.instance.MoveToTarget(true);
}
}

private void Awake()
{ 
CameraBase.instance = this;
}

private void Start()
{ 
this.UpdateCameraStats();
}

private void LateUpdate()
{ 
if (VideoSettings.Dirty)
{ 
this.UpdateCameraStats();
VideoSettings.Dirty = false;
}
base.transform.position += this.Offset;
}

private void OnPostRender()
{ 
base.transform.position -= this.Offset;
}

public static Vector3 RayToGround(ref Ray ray, float height = 1.5f)
{ 
CameraBase.hPlane.distance = -height;
float distance;
if (CameraBase.hPlane.Raycast(ray, out distance))
{ 
return ray.GetPoint(distance);
}
return ray.GetPoint(500f);
distance
}

public static Vector3 ViewportToGround(Camera camera, Vector2 viewportPos)
{ 
Ray ray = camera.ViewportPointToRay(viewportPos);
return CameraBase.RayToGround(ref ray, 1.5f);
ray
}

public Vector3 ViewportToGround(Vector2 viewportPos)
{ 
Ray ray = base.camera.ViewportPointToRay(viewportPos);
return CameraBase.RayToGround(ref ray, 1.5f);
ray
}

public Vector3 ScreenToGround(Vector2 screenPos, float height = 0f)
{ 
Ray ray = base.camera.ScreenPointToRay(screenPos);
return CameraBase.RayToGround(ref ray, height);
ray
}

public static Vector3 ScreenToGround(Camera camera, Vector2 screenPos, float height = 0f)
{ 
Ray ray = camera.ScreenPointToRay(screenPos);
return CameraBase.RayToGround(ref ray, height);
ray
}

protected void UpdateCameraStats()
{ 
Vector3 b = this.ViewportToGround(new Vector2(0.5f, 0.45f));
DepthOfFieldScatter component = base.GetComponent<DepthOfFieldScatter>();
if (component != null)
{ 
component.focalLength = (base.transform.position - b).magnitude;
}
b = this.ViewportToGround(new Vector2(1f, 1f));
QualitySettings.shadowDistance = (base.transform.position - b).magnitude * 1.1f;
b
component
}

protected Vector3 GetOffset()
{ 
Vector3 a = this.ViewportToGround(new Vector2(0.5f, 0.6f));
Vector3 result = a - base.camera.transform.position;
result.y = 0f;
return result;
a
result
}

protected void MoveToTarget(bool instantly = false)
{ 
Vector3 offset = this.GetOffset();
Vector3 vector = this.targetPosition.Value - base.transform.position - offset;
vector.y = 0f;
if (instantly)
{ 
base.transform.Translate(vector, Space.World);
this.targetPosition = null;
return;
}
vector /= 2f;
if ((double)vector.magnitude < 0.01)
{ 
CameraBase.instance.targetPosition = null;
}
base.transform.Translate(vector, Space.World);
offset
vector
}
}
