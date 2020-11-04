using Game.Data;
using Game.Zombiescape.Data;
using System;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Zombiescape.Monobehaviours
{ 
public class GravityBehaviour : MonoBehaviour
{ 
private bool hitGround;

private float fallSpeed;

private IUnit unit;

public Map map;

private new Collider collider;

private Bounds bounds;

private void Start()
{ 
this.collider = base.GetComponentInChildren<Collider>();
UnitBehaviour componentInChildren = base.GetComponentInChildren<UnitBehaviour>();
if (componentInChildren != null)
{ 
this.unit = componentInChildren.unit;
}
this.bounds = new Bounds(this.collider.bounds.center, new Vector3(0.5f, this.collider.bounds.size.y, 0.5f));
componentInChildren
}

private void Update()
{ 
if (this.unit != null && this.unit.IsDisabled)
{ 
return;
}
this.fallSpeed -= 9.8f * Time.deltaTime;
float y = this.fallSpeed * Time.deltaTime;
Vector3 b = new Vector3(0f, y, 0f);
this.bounds.center = this.collider.bounds.center + b;
if (!this.map.IsAir(this.bounds) || this.bounds.min.y < -2f)
{ 
this.fallSpeed = 0f;
}
else
{ 
base.transform.position += new Vector3(0f, y, 0f);
}
if (this.unit is Enemy)
{ 
((Enemy)this.unit).fallSpeed = this.fallSpeed;
}
this.bounds.center = this.bounds.center + new Vector3(0f, -0.3f, 0f);
if (Input.GetKeyDown(KeyCode.Space) && !this.map.IsAir(this.bounds) && this.fallSpeed <= 0.1f)
{ 
this.fallSpeed += 4f;
}
y
b
}
}
}
