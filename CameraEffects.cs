using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


namespace Game.Gui
{ 
public class CameraEffects
{ 
private MonoBehaviour gameBehaviour;

private ICameraShake shake;

public CameraEffects(MonoBehaviour gameBehaviour)
{ 
this.gameBehaviour = gameBehaviour;
this.shake = (ICameraShake)Camera.main.GetComponents(typeof(ICameraShake)).FirstOrDefault<Component>();
}

public void Shake(float magnitude, float duration, float speed)
{ 
if (this.shake == null)
{ 
return;
}
this.gameBehaviour.StartCoroutine(this.ShakePriv(magnitude, duration, speed));
}

[DebuggerHidden]
private IEnumerator ShakePriv(float magnitude, float duration, float speed)
{ 
CameraEffects.<ShakePriv>c__Iterator11 <ShakePriv>c__Iterator = new CameraEffects.<ShakePriv>c__Iterator11();
<ShakePriv>c__Iterator.duration = duration;
<ShakePriv>c__Iterator.speed = speed;
<ShakePriv>c__Iterator.magnitude = magnitude;
<ShakePriv>c__Iterator.<$>duration = duration;
<ShakePriv>c__Iterator.<$>speed = speed;
<ShakePriv>c__Iterator.<$>magnitude = magnitude;
<ShakePriv>c__Iterator.<>f__this = this;
return <ShakePriv>c__Iterator;
<ShakePriv>c__Iterator
}
}
}
