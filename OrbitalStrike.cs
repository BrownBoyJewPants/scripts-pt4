using Game.Gui;
using System;
using System.Collections;
using System.Diagnostics;
using TileEngine.Particles;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Data.Items
{ 
public class OrbitalStrike : Ammo
{ 
private new Ray ray = default(Ray);

private Vector3 hit;

private Vector3 normal;

private Vector3 source;

private ParticleSystem system;

private LensFlare flare;

private Light[] lights;

private Bounds bounds;

private float magnitude = 6.5f;

public OrbitalStrike(IGameInfo game, GameObject model) : base(game, model)
{ 
}

public override void Start()
{ 
this.source = this.target;
this.source.y = 10f;
this.model.transform.position = this.target;
this.system = this.model.GetComponentInChildren<ParticleSystem>();
this.flare = this.model.GetComponentInChildren<LensFlare>();
this.lights = this.model.GetComponentsInChildren<Light>();
this.flare.brightness = 0f;
this.system.emissionRate = 0f;
this.system.transform.localScale = Vector3.one * 0.9f * (this.magnitude / 3.5f);
this.bounds.center = new Vector3(this.source.x, 1.7f, this.source.z);
this.bounds.size = Vector3.one * 15f;
}

public override void Update()
{ 
this.timeAlive += Time.deltaTime;
float num = Mathf.Pow(this.timeAlive / 3.9f, 2f) * this.magnitude;
this.flare.brightness = Mathf.Pow(num / this.magnitude, 2f);
this.flare.transform.position = this.model.transform.position + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 2f + UnityEngine.Random.Range(0f, 0.2f), UnityEngine.Random.Range(-0.1f, 0.1f));
if ((double)this.timeAlive < 3.95)
{ 
this.system.emissionRate = 30f + this.timeAlive * this.timeAlive * 500f;
this.system.transform.localScale = Vector3.one * 0.9f * (num / 3.5f);
}
else
{ 
this.system.transform.localScale = Vector3.one * 0.9f * (this.magnitude / 3.5f);
this.system.startSpeed = 60f;
this.system.emissionRate = 150000f;
}
for (int i = 0; i < this.lights.Length; i++)
{ 
this.lights[i].range = 5f + num * 10f;
this.lights[i].intensity = 0.1f + this.timeAlive / 16f;
}
this.ray.direction = Vector3.down;
float num2 = num * num;
int j = 0;
IL_2EE:
while (j < 1 + (int)(this.timeAlive * this.timeAlive * 0.5f))
{ 
float num3 = UnityEngine.Random.Range(-num, num);
float num4 = Mathf.Sqrt(num2 - num3 * num3);
float z = UnityEngine.Random.Range(num4, -num4);
this.ray.origin = this.source + new Vector3(num3, 0f, z);
int num5 = 0;
float distance;
while (this.game.Map.IntersectRay(ref this.ray, 15f, out distance, out this.normal, false, true))
{ 
this.hit = this.ray.GetPoint(distance);
SphereShape shape = new SphereShape(this.hit, 0.1f);
this.game.Map.ConvertToParticles(this.game.Services.Get<TEParticleSystem>(), shape, 0, delegate(TEParticle particle)
{ 
particle.flags = (TEParticleFlag.Alive | TEParticleFlag.NoCollisions | TEParticleFlag.NoGravity);
particle.velocity = new Vector3(0f, UnityEngine.Random.Range(4f, 9f), 0f);
particle.drag = 0.15f;
particle.size *= 1.3f;
particle.lifetime = UnityEngine.Random.Range(0.5f, 1f);
}, -1);
this.game.Map.AddModification(shape);
if (this.hit.y <= 2f || num5++ >= 2)
{ 
IL_2E8:
j++;
goto IL_2EE;
}
}
goto IL_2E8;
}
TEParticleSystem tEParticleSystem = this.game.Services.Get<TEParticleSystem>();
IUnit[] unitsAt = this.game.Units.GetUnitsAt(this.bounds);
for (int k = 0; k < unitsAt.Length; k++)
{ 
IUnit unit = unitsAt[k];
unit.Disable();
Vector3 vector = unit.Model.transform.position;
Vector3 a = this.source - vector;
a.y *= 0.1f;
a.Normalize();
vector += a * Time.deltaTime * 4f;
unit.Model.transform.position = vector;
unit.Model.transform.Rotate(0f, Time.deltaTime * 360f * this.timeAlive, 0f);
int num6 = 5;
for (int l = 0; l < num6; l++)
{ 
TEParticle tEParticle = tEParticleSystem.Create();
tEParticle.flags = (TEParticleFlag.Alive | TEParticleFlag.Splatter);
tEParticle.color = ColorHelper.GetSplatterColor(unit.BloodColor);
tEParticle.position = vector;
tEParticle.lifetime = 2f;
tEParticle.size = 0.05f;
a = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
a.Normalize();
tEParticle.velocity = a * UnityEngine.Random.Range(10f, 15f);
tEParticle.drag = 0.15f;
tEParticle.splatterSize = UnityEngine.Random.Range(0.09f, 0.11f);
TEParticle expr_4AD_cp_0 = tEParticle;
expr_4AD_cp_0.position.x = expr_4AD_cp_0.position.x + UnityEngine.Random.Range(0f, 0.1f);
TEParticle expr_4CF_cp_0 = tEParticle;
expr_4CF_cp_0.position.y = expr_4CF_cp_0.position.y + UnityEngine.Random.Range(0f, 0.1f);
TEParticle expr_4F1_cp_0 = tEParticle;
expr_4F1_cp_0.position.z = expr_4F1_cp_0.position.z + UnityEngine.Random.Range(0f, 0.1f);
}
}
if (this.timeAlive > 4f)
{ 
this.game.Behaviour.StartCoroutine(this.DisableFlareDelayed());
this.Explode();
this.alive = false;
base.PrepareDestroy();
}
num
i
num2
j
num3
num4
z
num5
distance
shape
tEParticleSystem
unitsAt
k
unit
vector
a
num6
l
tEParticle
expr_4AD_cp_0
expr_4CF_cp_0
expr_4F1_cp_0
}

[DebuggerHidden]
private IEnumerator DisableFlareDelayed()
{ 
OrbitalStrike.<DisableFlareDelayed>c__IteratorC <DisableFlareDelayed>c__IteratorC = new OrbitalStrike.<DisableFlareDelayed>c__IteratorC();
<DisableFlareDelayed>c__IteratorC.<>f__this = this;
return <DisableFlareDelayed>c__IteratorC;
<DisableFlareDelayed>c__IteratorC
}

private void Explode()
{ 
SphereShape shape = new SphereShape(this.target, this.magnitude);
this.game.Services.Get<CameraEffects>().Shake(2f, 0.5f, 80f);
this.game.Map.ConvertToParticles(this.game.Services.Get<TEParticleSystem>(), shape, 2, delegate(TEParticle particle)
{ 
particle.flags = (TEParticleFlag.Alive | TEParticleFlag.NoCollisions | TEParticleFlag.NoGravity);
particle.velocity = new Vector3(0f, UnityEngine.Random.Range(5f, 9f), 0f);
particle.size *= 1.3f;
particle.lifetime = UnityEngine.Random.Range(0.2f, 0.5f);
}, -1);
this.game.Map.AddModification(shape);
shape
}
}
}
