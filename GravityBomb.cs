using System;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Data.Items
{ 
public class GravityBomb : Ammo
{ 
private new Ray ray;

private Vector3 normal;

private Vector3 hit;

private Vector3[] lasers;

private Vector3[] positions;

private LineRenderer renderer;

public GravityBomb(BattlescapeInfo game, GameObject model) : base(game, model)
{ 
}

public override void Start()
{ 
this.renderer = this.model.GetComponentInChildren<LineRenderer>();
this.target.y = this.target.y + 1.5f;
this.ray.origin = this.target;
this.lasers = new Vector3[10];
this.positions = new Vector3[this.lasers.Length];
this.renderer.SetVertexCount(this.lasers.Length * 2);
this.renderer.SetWidth(0.1f, 0.1f);
for (int i = 0; i < this.lasers.Length; i++)
{ 
this.lasers[i] = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
this.lasers[i].Normalize();
this.lasers[i] *= UnityEngine.Random.Range(3f, 4f);
this.positions[i] = this.lasers[i] * 100f;
this.renderer.SetPosition(i * 2, this.target);
this.renderer.SetPosition(i * 2 + 1, this.target);
}
i
}

public override void Update()
{ 
this.timeAlive += Time.deltaTime;
for (int i = 0; i < this.lasers.Length; i++)
{ 
this.positions[i] += this.lasers[i];
this.ray.direction = Quaternion.Euler(this.positions[i]) * Vector3.down;
float distance;
if (!this.game.Map.IntersectRay(ref this.ray, 10f, out distance, out this.normal, false, true))
{ 
this.hit = this.ray.GetPoint(10f);
this.renderer.SetPosition(i * 2 + 1, this.hit);
}
else
{ 
this.hit = this.ray.GetPoint(distance);
SphereShape shape = new SphereShape(this.hit, 0.1f);
this.game.Map.AddModification(shape);
this.renderer.SetPosition(i * 2 + 1, this.hit);
}
}
if (this.timeAlive > 5f)
{ 
this.alive = false;
this.OnDestruct();
}
i
distance
shape
}
}
}
