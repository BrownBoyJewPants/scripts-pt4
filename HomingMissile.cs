using System;
using UnityEngine;
using VoxelEngine;


namespace Game.Data.Items
{ 
public class HomingMissile : RocketLauncherAmmo
{ 
private Light light;

private float h;

private HSVColor hsv;

public HomingMissile(IGameInfo game, GameObject model) : base(game, model)
{ 
this.speed = 30f;
this.maxTimeAlive = 2f;
this.maxDistance = 60f;
}

public override void Start()
{ 
this.model.transform.position = this.unit.Model.transform.position + new Vector3(0f, 0.7f, 0f);
Vector3 vector = this.target - this.model.transform.position;
vector.Normalize();
Vector3 vector2 = vector;
this.model.transform.forward = vector2;
this.ray.direction = vector2;
vector
vector2
}

public override void Update()
{ 
IUnit closestTo = this.game.Units.GetClosestTo(this.model.transform.position, -5f + this.model.transform.position.y * 10f);
Quaternion? quaternion = null;
if (closestTo != null)
{ 
Vector3 forward = closestTo.Model.transform.position - this.model.transform.position;
forward.Normalize();
quaternion = new Quaternion?(Quaternion.LookRotation(forward));
}
else if (this.model.transform.forward.y < 0f)
{ 
quaternion = new Quaternion?(Quaternion.LookRotation(Vector3.up));
}
if (quaternion.HasValue)
{ 
this.model.transform.rotation = Quaternion.RotateTowards(this.model.transform.rotation, quaternion.Value, Time.deltaTime * 360f);
this.ray.direction = this.model.transform.forward;
}
base.InternalUpdate();
closestTo
quaternion
forward
}
}
}
