using Game.Global;
using Game.Gui;
using Game.Zombiescape.States;
using System;
using System.Collections;
using System.Diagnostics;
using TileEngine.Particles;
using TileEngine.TileMap;
using UnityEngine;
using VoxelEngine.TileEngine.Generators;


namespace Game.Data.Items
{ 
public class RifleAmmo : Ammo
{ 
private PooledPrefab shellPool;

public RifleAmmo(IGameInfo game, GameObject model, PooledPrefab shell) : base(game, model)
{ 
this.speed = 100f;
this.maxDistance = 20f;
this.maxTimeAlive = 1f;
this.damage = 35f;
this.shellPool = shell;
}

public override void Start()
{ 
this.game.Services.Get<AudioGui>().PlaySound("m4a1shot", this.model.transform.position, 1f);
if (this.weapon.FirePoint != null)
{ 
this.game.Services.Get<SpecialEffects>().AddMuzzleFlash(this.weapon.FirePoint, Vector3.zero);
Vector3 vector = this.model.transform.right + this.model.transform.up * 0.3f;
vector = HelperFunctions.CreateRandomDirection(vector, 0f, 15f);
if (GenerationSettings.shellCasings && this.weapon.EjectionPoint != null)
{ 
GameObject gameObject = this.shellPool.Instantiate();
gameObject.transform.position = this.weapon.EjectionPoint.position + vector * 0.1f;
gameObject.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
Rigidbody componentInChildren = gameObject.transform.GetComponentInChildren<Rigidbody>();
componentInChildren.position = gameObject.transform.position;
componentInChildren.rotation = gameObject.transform.rotation;
componentInChildren.velocity = this.unit.Velocity;
componentInChildren.AddForce(vector * 120f, ForceMode.Acceleration);
this.game.Behaviour.StartCoroutine(this.DestroyShell(gameObject));
}
}
base.Start();
vector
gameObject
componentInChildren
}

[DebuggerHidden]
private IEnumerator DestroyShell(GameObject shell)
{ 
RifleAmmo.<DestroyShell>c__IteratorD <DestroyShell>c__IteratorD = new RifleAmmo.<DestroyShell>c__IteratorD();
<DestroyShell>c__IteratorD.shell = shell;
<DestroyShell>c__IteratorD.<$>shell = shell;
<DestroyShell>c__IteratorD.<>f__this = this;
return <DestroyShell>c__IteratorD;
<DestroyShell>c__IteratorD
}

public override void HitWorld(ref Vector3 normal)
{ 
this.game.Services.Get<AudioGui>().PlaySound("bullethitwall", this.model.transform.position, 1f);
this.game.Services.Get<SpecialEffects>().DrawSmokePuff(this.model.transform.position, normal, 0.5f);
SphereShape shape = new SphereShape(this.model.transform.position, 0.15f);
Vector3 vector = normal;
Color32 color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
this.game.Map.ConvertToParticles(this.game.Services.Get<TEParticleSystem>(), shape, 0, delegate(TEParticle p)
{ 
p.velocity = HelperFunctions.CreateRandomDirection(this.ray.direction, 0f, 30f) * this.speed * UnityEngine.Random.Range(0.05f, 0.25f);
if (p.position.y <= 1.5f)
{ 
p.velocity.y = p.velocity.y + UnityEngine.Random.Range(1f, 2f);
}
if (PlayingState.painterEdition)
{ 
p.flags |= TEParticleFlag.Splatter;
p.color = ColorHelper.GetSplatterColor(color);
p.splatterSize = UnityEngine.Random.Range(0.1f, 0.15f);
}
else
{ 
p.color = ColorHelper.GetGroundColor(p.color);
}
p.size *= 1.2f;
}, -1);
this.game.Map.AddModification(shape);
base.HitWorld(ref normal);
<HitWorld>c__AnonStorey
shape
vector
}
}
}
