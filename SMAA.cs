using System;
using UnityEngine;


public class SMAA : MonoBehaviour
{ 
public int Passes = 1;

private Texture2D black;

private Shader shader;

private Material mat;

private GameObject textureGenerator;

private AreaTexture areaTexture;

private SearchTexture searchTexture;

private void Start()
{ 
this.shader = Shader.Find("Custom/SMAAshader");
this.mat = new Material(this.shader);
this.black = new Texture2D(1, 1);
this.black.SetPixel(0, 0, new Color(0f, 0f, 0f, 0f));
this.black.Apply();
this.areaTexture = new AreaTexture();
this.searchTexture = new SearchTexture();
this.mat.SetTexture("areaTex", this.areaTexture.alphaTex);
this.mat.SetTexture("luminTex", this.areaTexture.luminTex);
this.mat.SetTexture("searchTex", this.searchTexture.alphaTex);
Vector4 vector = new Vector4(1f / (float)Screen.width, 1f / (float)Screen.height, (float)Screen.width, (float)Screen.height);
this.mat.SetVector("SMAA_RT_METRICS", vector);
vector
}

private void OnRenderImage(RenderTexture source, RenderTexture destination)
{ 
Graphics.Blit(this.black, destination);
this.mat.SetTexture("_SrcTex", source);
RenderTexture temporary = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
RenderTexture temporary2 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
RenderTexture temporary3 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
Graphics.Blit(source, temporary3);
for (int i = 0; i < this.Passes; i++)
{ 
Graphics.Blit(this.black, temporary);
Graphics.Blit(this.black, temporary2);
Graphics.Blit(temporary3, temporary, this.mat, 0);
Graphics.Blit(temporary, temporary2, this.mat, 1);
Graphics.Blit(temporary2, temporary3, this.mat, 2);
}
Graphics.Blit(temporary3, destination);
RenderTexture.ReleaseTemporary(temporary);
RenderTexture.ReleaseTemporary(temporary2);
RenderTexture.ReleaseTemporary(temporary3);
temporary
temporary2
temporary3
i
}
}
