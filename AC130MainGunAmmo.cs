using Game.Global;
using Game.Gui;
using Game.Zombiescape.States;
using System;
using TileEngine.Particles;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Data.Items
{ 
public class AC130MainGunAmmo : Ammo
{ 
private int particles = 200;

public AC130MainGunAmmo(IGameInfo game, GameObject model) : base(game, model)
{ 
this.speed = 75f;
this.maxDistance = 120f;
this.maxTimeAlive = 3f;
this.damage = 500f;
}

protected override void SetFiringRay()
{ 
float x = Mathf.Sin(Time.timeSinceLevelLoad * 0.5f) * 20f;
float z = Mathf.Cos(Time.timeSinceLevelLoad * 0.5f) * 20f;
this.ray.origin = this.unit.Model.transform.position + new Vector3(x, 60f, z);
this.ray.direction = HelperFunctions.CreateRandomDirection(this.target - this.ray.origin, 2f, 5f);
if (this.nrInGroup == 0)
{ 
this.game.Services.Get<AudioGui>().PlaySound("Cannon", this.ray.origin, 2f);
}
x
z
}

protected override void OnDestruct()
{ 
float magnitude = 0.3f;
float num = 6f;
int count = 0;
Vector3 center = this.model.transform.position;
SphereShape shape = new SphereShape(center, magnitude);
float magnitude4 = (this.model.transform.position - this.unit.Model.transform.position).magnitude;
float magnitude2 = Mathf.Max(0.3f, magnitude * 2f - Mathf.Sqrt(magnitude4)) * 1f;
this.game.Services.Get<SpecialEffects>().DrawSmokeRing(this.model.transform.position + new Vector3(0f, 0.5f, 0f), Vector3.up, 0.8f, 100);
if (ChunkManager.usePhysicsObjects)
{ 
Collider[] array = Physics.OverlapSphere(this.model.transform.position, 2f);
for (int i = 0; i < array.Length; i++)
{ 
Collider collider = array[i];
Rigidbody rigidbody = collider.rigidbody;
if (rigidbody != null)
{ 
rigidbody.AddExplosionForce(3000f, this.model.transform.position, num * 4f, 1.02f);
}
}
}
if (this.nrInGroup == 0)
{ 
this.game.Services.Get<CameraEffects>().Shake(magnitude2, 0.25f, 6f);
this.game.Services.Get<AudioGui>().PlaySound("rocketExplosion", this.model.transform.position, 1f);
Bounds bounds = new Bounds(this.target, new Vector3(num * 2f, num * 2f, num * 2f));
IUnit[] unitsAt = this.game.Services.Get<IUnitManager>().GetUnitsAt(bounds);
for (int j = 0; j < unitsAt.Length; j++)
{ 
Vector3 direction = unitsAt[j].Model.transform.position - this.target;
float magnitude3 = direction.magnitude;
if (magnitude3 < num)
{ 
direction.Normalize();
unitsAt[j].TakeDamage(DamageType.Explosive, (float)((int)this.damage), direction);
this.BloodExplosion(unitsAt[j]);
this.game.Units.Remove(unitsAt[j]);
unitsAt[j].Model.SetActive(false);
}
}
}
TEParticleSystem tEParticleSystem = this.game.Services.Get<TEParticleSystem>();
int downSample;
if (tEParticleSystem.MaxParticles >= 30000)
{ 
downSample = 1;
}
else
{ 
downSample = 2;
}
bool painter = PlayingState.painterEdition;
Color32 color = new Color32((byte)UnityEngine.Random.Range(50, 255), (byte)UnityEngine.Random.Range(50, 255), (byte)UnityEngine.Random.Range(50, 255), 255);
Vector3 dir;
float dist;
float fac;
this.game.Map.ConvertToParticles(tEParticleSystem, shape, 1, delegate(TEParticle p)
{ 
dir.x = p.position.x - center.x;
dir.y = p.position.y - center.y;
dir.z = p.position.z - center.z;
dist = dir.magnitude;
dir.y = 1f;
dir = HelperFunctions.CreateRandomDirection(dir, 0f, 1f);
fac = Mathf.Sqrt(3f + magnitude - dist) * 1f;
fac *= UnityEngine.Random.Range(0.9f, 1f);
p.velocity.x = dir.x * fac;
p.velocity.y = dir.y * fac;
p.velocity.z = dir.z * fac;
if (p.position.y < 1.55f)
{ 
if (count % downSample != 0)
{ 
p.flags &= ~TEParticleFlag.Alive;
}
p.velocity.y = p.velocity.y + UnityEngine.Random.Range(3f, 5f);
}
p.size *= 1.5f;
p.position.x = p.position.x + p.velocity.x * 0.02f;
p.position.y = p.position.y + p.velocity.y * 0.02f;
p.position.z = p.position.z + p.velocity.z * 0.02f;
if (painter)
{ 
p.flags |= TEParticleFlag.Splatter;
p.color = ColorHelper.GetSplatterColor(color);
p.splatterSize = UnityEngine.Random.Range(0.1f, 0.15f);
}
else
{ 
p.color = ColorHelper.GetGroundColor(p.color);
}
count++;
}, 2);
this.game.Map.AddModification(shape);
<OnDestruct>c__AnonStorey
num
shape
magnitude4
magnitude2
array
i
collider
rigidbody
bounds
unitsAt
j
direction
magnitude3
tEParticleSystem
}

private void BloodExplosion(IUnit unit)
{ 
TEParticleSystem tEParticleSystem = this.game.Services.Get<TEParticleSystem>();
int num = this.particles;
if (tEParticleSystem.LiveSplatter > num * 5)
{ 
num /= 10;
}
else if (tEParticleSystem.LiveSplatter > num * 3)
{ 
num /= 5;
}
else if (tEParticleSystem.LiveSplatter > num)
{ 
num /= 2;
}
for (int i = 0; i < num; i++)
{ 
TEParticle tEParticle = tEParticleSystem.Create();
Vector3 a = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-0.1f, 1f), UnityEngine.Random.Range(-1f, 1f));
a.Normalize();
tEParticle.flags = (TEParticleFlag.Alive | TEParticleFlag.DestroyOnCollision | TEParticleFlag.CollidesWithProps);
if (i % 2 == 0)
{ 
tEParticle.flags |= TEParticleFlag.Splatter;
}
tEParticle.color = ColorHelper.GetSplatterColor(unit.BloodColor);
tEParticle.position = unit.Model.transform.position;
TEParticle expr_EB_cp_0 = tEParticle;
expr_EB_cp_0.position.y = expr_EB_cp_0.position.y + 0.5f;
tEParticle.lifetime = 2f;
tEParticle.size = 0.07f;
tEParticle.velocity = a * UnityEngine.Random.Range(5f, 7f);
tEParticle.drag = 0.015f;
if (UnityEngine.Random.Range(0, 10) == 0)
{ 
tEParticle.splatterSize = UnityEngine.Random.Range(0.12f, 0.14f);
}
else
{ 
tEParticle.splatterSize = UnityEngine.Random.Range(0.06f, 0.11f);
}
TEParticle expr_17B_cp_0 = tEParticle;
expr_17B_cp_0.position.x = expr_17B_cp_0.position.x + UnityEngine.Random.Range(0f, 0.1f);
TEParticle expr_19C_cp_0 = tEParticle;
expr_19C_cp_0.position.y = expr_19C_cp_0.position.y + UnityEngine.Random.Range(0f, 0.1f);
TEParticle expr_1BD_cp_0 = tEParticle;
expr_1BD_cp_0.position.z = expr_1BD_cp_0.position.z + UnityEngine.Random.Range(0f, 0.1f);
}
tEParticleSystem
num
i
tEParticle
a
expr_EB_cp_0
expr_17B_cp_0
expr_19C_cp_0
expr_1BD_cp_0
}
}
}
