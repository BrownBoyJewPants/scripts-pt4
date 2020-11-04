using System;
using UnityEngine;


public class CameraShootBoxes : MonoBehaviour
{ 
public Material material;

private float time;

private Transform parent;

private void Update()
{ 
if (!Input.GetMouseButton(0))
{ 
return;
}
if ((double)(Time.time - this.time) > 0.03)
{ 
if (this.parent == null)
{ 
this.parent = new GameObject("Cubes").transform;
}
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
gameObject.transform.position = ray.origin;
gameObject.transform.localScale = new Vector3(0.2f, 1f, 0.2f);
gameObject.transform.rotation = base.camera.transform.rotation;
gameObject.transform.parent = this.parent;
Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
rigidbody.AddForce(ray.direction * 700f);
gameObject.renderer.material = this.material;
this.time = Time.time;
}
ray
gameObject
rigidbody
}
}
