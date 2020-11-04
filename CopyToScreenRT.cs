using System;
using UnityEngine;


[ExecuteInEditMode]
public class CopyToScreenRT : MonoBehaviour
{ 
private RenderTexture activeRT;

private void OnPreRender()
{ 
if (base.camera.actualRenderingPath == RenderingPath.DeferredLighting)
{ 
this.activeRT = RenderTexture.active;
}
else
{ 
this.activeRT = null;
}
}

private void OnRenderImage(RenderTexture src, RenderTexture dest)
{ 
if (base.camera.actualRenderingPath == RenderingPath.DeferredLighting && this.activeRT)
{ 
if (src.format == this.activeRT.format)
{ 
Graphics.Blit(src, this.activeRT);
}
else
{ 
Debug.LogWarning("Cant resolve texture, because of different formats!");
}
}
Graphics.Blit(src, dest);
}
}
