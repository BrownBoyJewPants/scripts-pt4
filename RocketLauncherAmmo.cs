using Game.Global;
using Game.Gui;
using Game.Zombiescape.States;
using System;
using TileEngine.Particles;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Data.Items
{ 
public class RocketLauncherAmmo : Ammo
{ 
protected float explosionRadius = 3f;

private int particles = 750;

public RocketLauncherAmmo(IGameInfo game, GameObject model) : base(game, model)
{ 
this.speed = 1f;
this.maxDistance = 500f;
this.maxTimeAlive = 3f;
this.damage = 150f;
}

public override void Start()
{ 
this.game.Services.Get<CameraEffects>().Shake(0.2f, 0.2f, 1f);
base.Start();
}

public override void Update()
{ 
if (this.timeAlive < 1.5f)
{ 
this.speed += (1.5f - this.timeAlive) * 150f * Time.deltaTime;
}
base.Update();
}

protected virtual void ShakeCamera()
{ 
float magnitude = (this.model.transform.position - this.unit.Model.transform.position).magnitude;
float magnitude2 = Mathf.Max(0.3f, 5f - Mathf.Sqrt(magnitude)) * 0.5f;
this.game.Services.Get<CameraEffects>().Shake(magnitude2, 0.25f, 6f);
magnitude
magnitude2
}

protected override void OnDestruct()
{ 
float magnitude = this.explosionRadius;
Vector3 center = this.model.transform.position;
SphereShape sphereShape = new SphereShape(center, magnitude);
int count = 0;
this.ShakeCamera();
this.game.Services.Get<AudioGui>().PlaySound("RocketExplosion", this.model.transform.position, 1f);
this.game.Services.Get<SpecialEffects>().DrawSmokeRing(this.model.transform.position + new Vector3(0f, 0.5f, 0f), Vector3.up, magnitude, 500);
if (ChunkManager.usePhysicsObjects)
{ 
Collider[] array = Physics.OverlapSphere(this.model.transform.position, magnitude);
for (int i = 0; i < array.Length; i++)
{ 
Collider collider = array[i];
Rigidbody rigidbody = collider.rigidbody;
if (rigidbody != null)
{ 
rigidbody.AddExplosionForce(5000f, this.model.transform.position, magnitude * 4f, 1.1f);
}
}
}
bool flag = false;
IUnit[] unitsAt = this.game.Units.GetUnitsAt(sphereShape.bounds);
for (int j = 0; j < unitsAt.Length; j++)
{ 
Vector3 direction = unitsAt[j].Model.transform.position - sphereShape.bounds.center;
float magnitude2 = direction.magnitude;
if (magnitude2 < magnitude + 0.3f)
{ 
direction.Normalize();
unitsAt[j].TakeDamage(DamageType.Explosive, (float)((int)this.damage), direction);
if (direction.magnitude < 2f)
{ 
if (!flag)
{ 
this.BloodExplosion(unitsAt[j]);
}
unitsAt[j].Model.SetActive(false);
this.game.Units.Remove(unitsAt[j]);
flag = true;
}
}
}
TEParticleSystem tEParticleSystem = this.game.Services.Get<TEParticleSystem>();
int downSample;
if (!flag && tEParticleSystem.MaxParticles > 30000)
{ 
downSample = 1;
}
else
{ 
downSample = 2;
}
bool painter = PlayingState.painterEdition;
Color32 color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
bool downsampleMore = tEParticleSystem.MaxParticles < 10000 || painter;
Vector3 dir;
float dist;
float fac;
this.game.Map.ConvertToParticles(tEParticleSystem, sphereShape, 1, delegate(TEParticle p)
{ 
dir.x = p.position.x - center.x;
dir.y = p.position.y - center.y;
dir.z = p.position.z - center.z;
dist = dir.magnitude;
dir = HelperFunctions.CreateRandomDirection(dir, 0f, 30f);
fac = Mathf.Sqrt(3f + magnitude - dist) * 4f;
fac *= UnityEngine.Random.Range(0.9f, 1f);
p.velocity.x = dir.x * fac;
p.velocity.y = dir.y * fac;
p.velocity.z = dir.z * fac;
if (downsampleMore && count % 2 == 0)
{ 
p.flags &= ~TEParticleFlag.Alive;
}
if (p.position.y < 1.55f)
{ 
if ((downsampleMore || downSample == 1) && count % 2 == 0)
{ 
p.flags &= ~TEParticleFlag.Alive;
}
if (painter)
{ 
p.velocity.y = p.velocity.y + UnityEngine.Random.Range(1f, 5f);
}
else
{ 
p.velocity.y = p.velocity.y + UnityEngine.Random.Range(3f, 5f);
}
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
}, downSample);
this.game.Map.AddModification(sphereShape);
<OnDestruct>c__AnonStorey
sphereShape
array
i
collider
rigidbody
flag
unitsAt
j
direction
magnitude2
tEParticleSystem
}

private void BloodExplosion(IUnit unit)
{ 
TEParticleSystem tEParticleSystem = this.game.Services.Get<TEParticleSystem>();
for (int i = 0; i < this.particles; i++)
{ 
TEParticle tEParticle = tEParticleSystem.Create();
Vector3 a = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-0.1f, 1f), UnityEngine.Random.Range(-1f, 1f));
a.Normalize();
tEParticle.flags = (TEParticleFlag.Alive | TEParticleFlag.DestroyOnCollision | TEParticleFlag.CollidesWithProps);
if (i % 2 == 0)
{ 
tEParticle.flags |= TEParticleFlag.Splatter;
}
tEParticle.color = Color32.Lerp(unit.BloodColor, Color.black, UnityEngine.Random.Range(0f, 0.5f));
tEParticle.position = unit.Model.transform.position;
TEParticle expr_BE_cp_0 = tEParticle;
expr_BE_cp_0.position.y = expr_BE_cp_0.position.y + 0.5f;
tEParticle.lifetime = 2f;
tEParticle.size = 0.07f;
tEParticle.velocity = a * UnityEngine.Random.Range(8f, 13f);
tEParticle.drag = 0.015f;
tEParticle.splatterSize = UnityEngine.Random.Range(0.06f, 0.11f);
TEParticle expr_126_cp_0 = tEParticle;
expr_126_cp_0.position.x = expr_126_cp_0.position.x + UnityEngine.Random.Range(0f, 0.1f);
TEParticle expr_147_cp_0 = tEParticle;
expr_147_cp_0.position.y = expr_147_cp_0.position.y + UnityEngine.Random.Range(0f, 0.1f);
TEParticle expr_168_cp_0 = tEParticle;
expr_168_cp_0.position.z = expr_168_cp_0.position.z + UnityEngine.Random.Range(0f, 0.1f);
}
tEParticleSystem
i
tEParticle
a
expr_BE_cp_0
expr_126_cp_0
expr_147_cp_0
expr_168_cp_0
}
}
}
