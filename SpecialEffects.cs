using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;


namespace Game.Global
{ 
public class SpecialEffects
{ 
private class ParticleSystemEffect
{ 
private PooledPrefab prefab;

private ParticleSystem system;

private SpecialEffects effects;

public ParticleSystemEffect(SpecialEffects effects, GameObject prefab)
{ 
this.effects = effects;
this.prefab = new PooledPrefab(prefab);
this.system = prefab.GetComponent<ParticleSystem>();
}

public void Draw(Vector3 position, Vector3 orientation, float size, int particles = 500)
{ 
GameObject gameObject = this.prefab.Instantiate();
gameObject.transform.position = position;
gameObject.transform.localRotation = Quaternion.LookRotation(orientation);
ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
component.startSize = this.system.startSize * size;
component.startSpeed = this.system.startSpeed * size;
component.emissionRate = 0f;
component.Emit(particles);
this.effects.behaviour.StartCoroutine(this.effects.DestroyObject(this.prefab, gameObject, this.system.duration));
gameObject
component
}
}

private Color32 ambient;

private GameObject floatingTextHolder;

private PooledPrefab floatingTextPool;

private PooledPrefab muzzleFlashPool;

private SpecialEffects.ParticleSystemEffect smokeRing;

private SpecialEffects.ParticleSystemEffect smokePuff;

private IGameBehaviour behaviour;

public SpecialEffects(IGameBehaviour behaviour)
{ 
this.behaviour = behaviour;
this.ambient = RenderSettings.ambientLight;
this.smokeRing = new SpecialEffects.ParticleSystemEffect(this, Resources.Load<GameObject>("prefabs/smokering"));
this.smokePuff = new SpecialEffects.ParticleSystemEffect(this, Resources.Load<GameObject>("prefabs/smokepuff"));
this.floatingTextPool = new PooledPrefab(Resources.Load<GameObject>("prefabs/floatingtext"));
this.muzzleFlashPool = new PooledPrefab(Resources.Load<GameObject>("prefabs/muzzleflash"));
this.floatingTextHolder = GameObject.Find("FloatingTextHolder");
}

public void DrawSmokeRing(Vector3 position, Vector3 orientation, float size, int particles = 500)
{ 
this.smokeRing.Draw(position, orientation, size / 3f, particles);
}

public void DrawSmokePuff(Vector3 position, Vector3 orientation, float size)
{ 
this.smokePuff.Draw(position, orientation, size, 500);
}

public void DrawFloatingText(Vector3 position, Color32 color, float duration, string text, float scrollSpeed = 100f)
{ 
GameObject gameObject = this.floatingTextPool.Instantiate();
FloatingText component = gameObject.GetComponent<FloatingText>();
gameObject.transform.parent = this.floatingTextHolder.transform;
gameObject.transform.localScale = new Vector3(16f, 16f, 0f);
component.scrollspeed = scrollSpeed;
component.position = position;
component.pool = this.floatingTextPool;
component.text = text;
component.color = color;
component.duration = duration;
component.fadestart = Mathf.Max(0f, duration - 0.5f);
component.enabled = true;
gameObject
component
}

[DebuggerHidden]
private IEnumerator DestroyObject(PooledPrefab pool, GameObject obj, float time)
{ 
SpecialEffects.<DestroyObject>c__Iterator12 <DestroyObject>c__Iterator = new SpecialEffects.<DestroyObject>c__Iterator12();
<DestroyObject>c__Iterator.time = time;
<DestroyObject>c__Iterator.pool = pool;
<DestroyObject>c__Iterator.obj = obj;
<DestroyObject>c__Iterator.<$>time = time;
<DestroyObject>c__Iterator.<$>pool = pool;
<DestroyObject>c__Iterator.<$>obj = obj;
return <DestroyObject>c__Iterator;
<DestroyObject>c__Iterator
}

public void BlinkScreen(Color32 color)
{ 
RenderSettings.ambientLight = color;
this.behaviour.StartCoroutine(this.ResetAmbient());
}

[DebuggerHidden]
private IEnumerator ResetAmbient()
{ 
SpecialEffects.<ResetAmbient>c__Iterator13 <ResetAmbient>c__Iterator = new SpecialEffects.<ResetAmbient>c__Iterator13();
<ResetAmbient>c__Iterator.<>f__this = this;
return <ResetAmbient>c__Iterator;
<ResetAmbient>c__Iterator
}

public void DrawMuzzleFlash(Vector3 position)
{ 
GameObject gameObject = this.muzzleFlashPool.Instantiate();
gameObject.transform.position = position;
this.behaviour.StartCoroutine(this.RemoveFlash(gameObject));
gameObject
}

public void AddMuzzleFlash(Transform transform, Vector3 offset)
{ 
GameObject gameObject = this.muzzleFlashPool.Instantiate();
gameObject.transform.parent = transform;
gameObject.transform.localPosition = offset;
this.behaviour.StartCoroutine(this.RemoveFlash(gameObject));
gameObject
}

[DebuggerHidden]
private IEnumerator RemoveFlash(GameObject flash)
{ 
SpecialEffects.<RemoveFlash>c__Iterator14 <RemoveFlash>c__Iterator = new SpecialEffects.<RemoveFlash>c__Iterator14();
<RemoveFlash>c__Iterator.flash = flash;
<RemoveFlash>c__Iterator.<$>flash = flash;
<RemoveFlash>c__Iterator.<>f__this = this;
return <RemoveFlash>c__Iterator;
<RemoveFlash>c__Iterator
}
}
}
