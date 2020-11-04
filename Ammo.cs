using Assets.Scripts.Settings;
using Game.Gui;
using System;
using System.Collections;
using System.Diagnostics;
using TileEngine.Particles;
using UnityEngine;
using VoxelEngine;
using VoxelEngine.Models;


namespace Game.Data.Items
{ 
public abstract class Ammo
{ 
protected IGameInfo game;

protected Vector3 target;

public float maxDistance;

public float maxTimeAlive;

public float speed;

public float damage;

public bool alive;

protected int nrInGroup;

protected IUnit unit;

protected Weapon weapon;

protected float traveledDistance;

protected float timeAlive;

protected Ray ray;

private PooledPrefab prefab;

public GameObject model;

private float orgEmissionRate;

private VideoSettings video;

public Ammo(IGameInfo game, GameObject prefab)
{ 
this.game = game;
if (prefab != null)
{ 
this.prefab = new PooledPrefab(prefab);
}
}

public Ammo Spawn(IUnit unit, Weapon weapon, Vector3 target, int nrInGroup)
{ 
if (this.video == null)
{ 
this.video = this.game.Services.Get<VideoSettings>();
}
Ammo ammo = this.Clone();
ammo.unit = unit;
if (this.prefab != null)
{ 
ammo.model = this.prefab.Instantiate();
Light componentInChildren = ammo.model.GetComponentInChildren<Light>();
if (componentInChildren != null)
{ 
componentInChildren.enabled = this.video.pointlights;
}
}
ammo.alive = true;
ammo.target = target;
ammo.weapon = weapon;
ammo.nrInGroup = nrInGroup;
ammo.Start();
return ammo;
ammo
componentInChildren
}

protected virtual Ammo Clone()
{ 
return (Ammo)base.MemberwiseClone();
}

protected virtual void OnDestruct()
{ 
}

protected virtual void SetFiringRay()
{ 
this.ray = this.unit.GetFiringRay(this.weapon, ref this.target);
}

public virtual void Start()
{ 
if (this.model == null)
{ 
return;
}
this.SetFiringRay();
if (this.weapon.fireCircle == 0f)
{ 
this.model.transform.position = this.ray.origin;
}
else
{ 
float f = Time.timeSinceLevelLoad * 60f;
float x = Mathf.Sin(f) * this.weapon.fireCircle;
float y = Mathf.Cos(f) * this.weapon.fireCircle;
this.model.transform.position = this.ray.origin + Quaternion.LookRotation(this.ray.direction) * new Vector3(x, y, 0f);
}
this.model.transform.localRotation = Quaternion.LookRotation(this.ray.direction);
f
x
y
}

public virtual void TimeAliveElapsed()
{ 
}

public virtual void MaxDistanceTravelled()
{ 
}

public virtual void HitWorld(ref Vector3 normal)
{ 
this.alive = false;
}

public virtual void HitUnit(IUnit unit, Vector3 fromDirection)
{ 
TEParticleSystem tEParticleSystem = this.game.Services.Get<TEParticleSystem>();
int num = (tEParticleSystem.MaxParticles >= 10000) ? 200 : 75;
bool flag = tEParticleSystem.MaxParticles < 4000;
this.game.Services.Get<AudioGui>().PlaySound("bodyhit", this.model.transform.position, 1f);
Color32 color = Color.black;
if (!unit.IsAlive)
{ 
num /= 5;
}
if (unit.IsFriendly)
{ 
num /= 5;
}
Vector3 a = unit.Model.transform.position + new Vector3(0f, 0.5f, 0f);
if (tEParticleSystem.LiveSplatter > num * 3)
{ 
num /= 5;
}
else if (tEParticleSystem.LiveSplatter > num)
{ 
num /= 2;
}
if (unit.Model.rigidBody != null)
{ 
unit.Model.rigidBody.AddForce(fromDirection * this.speed * 100f);
}
for (int i = 0; i < num; i++)
{ 
TEParticle tEParticle = tEParticleSystem.Create();
tEParticle.flags = (TEParticleFlag.Alive | TEParticleFlag.DestroyOnCollision | TEParticleFlag.CollidesWithProps);
if (!flag || i % 3 != 0)
{ 
tEParticle.flags |= TEParticleFlag.Splatter;
}
tEParticle.color = ColorHelper.GetSplatterColor(unit.BloodColor);
tEParticle.position = a + new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), UnityEngine.Random.Range(-0.2f, 0.3f), UnityEngine.Random.Range(-0.3f, 0.3f));
tEParticle.lifetime = 2f;
tEParticle.size = 0.07f;
tEParticle.velocity = HelperFunctions.CreateRandomDirection(fromDirection, 0.5f, 20f) * UnityEngine.Random.Range(0f, 5f + this.speed * 0.25f);
tEParticle.velocity.y = UnityEngine.Random.Range(0f, 5f);
tEParticle.drag = 0.15f;
tEParticle.splatterSize = UnityEngine.Random.Range(0.06f, 0.11f);
TEParticle expr_23F_cp_0 = tEParticle;
expr_23F_cp_0.position.x = expr_23F_cp_0.position.x + UnityEngine.Random.Range(0f, 0.1f);
TEParticle expr_261_cp_0 = tEParticle;
expr_261_cp_0.position.y = expr_261_cp_0.position.y + UnityEngine.Random.Range(0f, 0.1f);
TEParticle expr_283_cp_0 = tEParticle;
expr_283_cp_0.position.z = expr_283_cp_0.position.z + UnityEngine.Random.Range(0f, 0.1f);
}
if (unit.IsAlive)
{ 
Vector3 b = fromDirection;
b.y = 0f;
unit.Model.transform.LookAt(unit.Model.transform.position - b);
}
if (unit.IsFriendly)
{ 
this.game.Behaviour.StartCoroutine(this.Bleeding(unit, this.model.transform.position, fromDirection * this.speed));
}
else
{ 
this.FallApart(unit);
}
unit.TakeDamage(DamageType.Normal, (float)((int)this.damage), fromDirection);
this.alive = false;
tEParticleSystem
num
flag
color
a
i
tEParticle
expr_23F_cp_0
expr_261_cp_0
expr_283_cp_0
b
}

private void FallApart(IUnit unit)
{ 
VoxelModelPart[] parts = unit.Model.parts;
for (int i = 0; i < parts.Length; i++)
{ 
VoxelModelPart voxelModelPart = parts[i];
TEParticleSystem tEParticleSystem = this.game.Services.Get<TEParticleSystem>();
VoxelData voxeldata = voxelModelPart.voxeldata;
Vector3 vector = new Vector3((float)voxeldata.Dx, (float)voxeldata.Dy, (float)voxeldata.Dz) * 0.5f;
int num = 0;
for (int j = 0; j < voxeldata.Dy; j += 3)
{ 
Vector3 a;
a.y = (float)j;
for (int k = 0; k < voxeldata.Dz; k += 3)
{ 
a.z = (float)k;
for (int l = 0; l < voxeldata.Dx; l += 3)
{ 
a.x = (float)l;
int data = voxeldata.GetData(l, j, k);
if (data != 0)
{ 
TEParticle tEParticle = tEParticleSystem.Create();
tEParticle.flags = TEParticleFlag.Alive;
num++;
tEParticle.color = ColorHelper.GetSplatterColor(unit.BloodColor);
if (num % 4 == 0)
{ 
tEParticle.flags |= TEParticleFlag.Splatter;
}
tEParticle.position = voxelModelPart.transform.TransformPoint(a - voxelModelPart.center);
tEParticle.lifetime = UnityEngine.Random.Range(0.5f, 1.5f);
tEParticle.size = 0.075f;
tEParticle.velocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 5f), UnityEngine.Random.Range(-1f, 1f)) * 0.2f + this.ray.direction * UnityEngine.Random.Range(0f, 5f);
tEParticle.drag = 0.1f;
tEParticle.splatterSize = UnityEngine.Random.Range(0.15f, 0.2f);
}
}
}
}
}
parts
i
voxelModelPart
tEParticleSystem
voxeldata
vector
num
j
a
k
l
data
tEParticle
}

[DebuggerHidden]
private IEnumerator Bleeding(IUnit unit, Vector3 hitPosition, Vector3 impactVelocity)
{ 
Ammo.<Bleeding>c__IteratorA <Bleeding>c__IteratorA = new Ammo.<Bleeding>c__IteratorA();
<Bleeding>c__IteratorA.hitPosition = hitPosition;
<Bleeding>c__IteratorA.impactVelocity = impactVelocity;
<Bleeding>c__IteratorA.unit = unit;
<Bleeding>c__IteratorA.<$>hitPosition = hitPosition;
<Bleeding>c__IteratorA.<$>impactVelocity = impactVelocity;
<Bleeding>c__IteratorA.<$>unit = unit;
<Bleeding>c__IteratorA.<>f__this = this;
return <Bleeding>c__IteratorA;
<Bleeding>c__IteratorA
}

protected void InternalUpdate()
{ 
float num = this.speed * Time.deltaTime;
if (this.traveledDistance + num >= this.maxDistance)
{ 
this.alive = false;
this.MaxDistanceTravelled();
}
this.ray.origin = this.model.transform.position;
float num2 = 3.40282347E+38f;
float num3 = 3.40282347E+38f;
IUnit unit = null;
RaycastHit raycastHit;
if (Physics.Raycast(this.ray, out raycastHit, num))
{ 
UnitBehaviour component = raycastHit.collider.gameObject.GetComponent<UnitBehaviour>();
if (component != null && component.unit != this.unit && component.unit.IsFriendly != this.unit.IsFriendly)
{ 
unit = component.unit;
num2 = raycastHit.distance;
}
else if (raycastHit.rigidbody != null && raycastHit.rigidbody != this.unit.Model.rigidBody)
{ 
num3 = raycastHit.distance;
raycastHit.rigidbody.AddForce(this.ray.direction * this.speed * 20f, ForceMode.Force);
}
}
float num4 = 3.40282347E+38f;
float num5;
Vector3 vector;
if (this.game.Map.IntersectRay(ref this.ray, num, out num5, out vector, false, true))
{ 
num4 = num5;
}
if (num4 != 3.40282347E+38f && num4 < num2 && num4 < num3)
{ 
num = num4;
this.model.transform.position += this.ray.direction * num;
this.HitWorld(ref vector);
}
else if (num2 != 3.40282347E+38f)
{ 
num = num2;
this.model.transform.position = raycastHit.point;
this.HitUnit(unit, this.ray.direction);
}
else if (num3 != 3.40282347E+38f)
{ 
num = num3;
this.model.transform.position = raycastHit.point;
vector = -this.ray.direction;
this.HitWorld(ref vector);
}
else
{ 
this.model.transform.position += this.ray.direction * num;
}
this.traveledDistance += num;
this.timeAlive += Time.deltaTime;
if (this.timeAlive > this.maxTimeAlive)
{ 
this.alive = false;
this.TimeAliveElapsed();
}
if (!this.alive)
{ 
this.PrepareDestroy();
}
num
num2
num3
unit
raycastHit
component
num4
num5
vector
}

public virtual void Update()
{ 
this.InternalUpdate();
}

protected void PrepareDestroy()
{ 
ParticleSystem componentInChildren = this.model.GetComponentInChildren<ParticleSystem>();
if (componentInChildren == null)
{ 
this.prefab.Destroy(this.model);
}
else
{ 
this.orgEmissionRate = componentInChildren.emissionRate;
componentInChildren.loop = false;
componentInChildren.emissionRate = 0f;
Renderer[] componentsInChildren = this.model.GetComponentsInChildren<Renderer>();
for (int i = 0; i < componentsInChildren.Length; i++)
{ 
Renderer renderer = componentsInChildren[i];
if (renderer.gameObject != componentInChildren.gameObject)
{ 
renderer.gameObject.SetActive(false);
}
}
Light[] componentsInChildren2 = this.model.GetComponentsInChildren<Light>();
for (int j = 0; j < componentsInChildren2.Length; j++)
{ 
Light light = componentsInChildren2[j];
light.gameObject.SetActive(false);
}
AudioSource[] componentsInChildren3 = this.model.GetComponentsInChildren<AudioSource>();
for (int k = 0; k < componentsInChildren3.Length; k++)
{ 
AudioSource audioSource = componentsInChildren3[k];
audioSource.gameObject.SetActive(false);
}
this.game.Behaviour.StartCoroutine(this.WaitForParticleSystem(componentInChildren));
}
this.OnDestruct();
componentInChildren
componentsInChildren
i
renderer
componentsInChildren2
j
light
componentsInChildren3
k
audioSource
}

[DebuggerHidden]
private IEnumerator WaitForParticleSystem(ParticleSystem particleSystem)
{ 
Ammo.<WaitForParticleSystem>c__IteratorB <WaitForParticleSystem>c__IteratorB = new Ammo.<WaitForParticleSystem>c__IteratorB();
<WaitForParticleSystem>c__IteratorB.particleSystem = particleSystem;
<WaitForParticleSystem>c__IteratorB.<$>particleSystem = particleSystem;
<WaitForParticleSystem>c__IteratorB.<>f__this = this;
return <WaitForParticleSystem>c__IteratorB;
<WaitForParticleSystem>c__IteratorB
}
}
}
