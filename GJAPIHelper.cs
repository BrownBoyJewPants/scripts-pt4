using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;


public class GJAPIHelper : MonoBehaviour
{ 
private static GJAPIHelper instance;

protected GUISkin skin;

private GJHUsersMethods users;

private GJHScoresMethods scores;

private GJHTrophiesMethods trophies;

public static GJAPIHelper Instance
{ 
get
{ 
if (GJAPIHelper.instance == null)
{ 
GJAPI gJAPI = (GJAPI)UnityEngine.Object.FindObjectOfType(typeof(GJAPI));
if (gJAPI == null)
{ 
UnityEngine.Debug.LogError("An instance of GJAPI is needed in the scene, but there is none. Can't initialise GJAPIHelper.");
}
else
{ 
GJAPIHelper.instance = gJAPI.gameObject.AddComponent<GJAPIHelper>();
if (GJAPIHelper.instance == null)
{ 
UnityEngine.Debug.Log("An error occured creating GJAPIHelper.");
}
}
}
return GJAPIHelper.instance;
gJAPI
}
}

public static GUISkin Skin
{ 
get
{ 
if (GJAPIHelper.Instance.skin == null)
{ 
GJAPIHelper.Instance.skin = (((GUISkin)Resources.Load("GJSkin", typeof(GUISkin))) ?? GUI.skin);
}
return GJAPIHelper.Instance.skin;
}
set
{ 
GJAPIHelper.Instance.skin = value;
}
}

public static GJHUsersMethods Users
{ 
get
{ 
if (GJAPIHelper.Instance.users == null)
{ 
GJAPIHelper.Instance.users = new GJHUsersMethods();
}
return GJAPIHelper.Instance.users;
}
}

public static GJHScoresMethods Scores
{ 
get
{ 
if (GJAPIHelper.Instance.scores == null)
{ 
GJAPIHelper.Instance.scores = new GJHScoresMethods();
}
return GJAPIHelper.Instance.scores;
}
}

public static GJHTrophiesMethods Trophies
{ 
get
{ 
if (GJAPIHelper.Instance.trophies == null)
{ 
GJAPIHelper.Instance.trophies = new GJHTrophiesMethods();
}
return GJAPIHelper.Instance.trophies;
}
}

private void OnDestroy()
{ 
base.StopAllCoroutines();
this.skin = null;
this.users = null;
this.scores = null;
this.trophies = null;
GJAPIHelper.instance = null;
}

public static void DownloadImage(string url, Action<Texture2D> OnComplete)
{ 
GJAPIHelper.Instance.StartCoroutine(GJAPIHelper.Instance.DownloadImageCoroutine(url, OnComplete));
}

[DebuggerHidden]
private IEnumerator DownloadImageCoroutine(string url, Action<Texture2D> OnComplete)
{ 
GJAPIHelper.<DownloadImageCoroutine>c__Iterator2 <DownloadImageCoroutine>c__Iterator = new GJAPIHelper.<DownloadImageCoroutine>c__Iterator2();
<DownloadImageCoroutine>c__Iterator.url = url;
<DownloadImageCoroutine>c__Iterator.OnComplete = OnComplete;
<DownloadImageCoroutine>c__Iterator.<$>url = url;
<DownloadImageCoroutine>c__Iterator.<$>OnComplete = OnComplete;
return <DownloadImageCoroutine>c__Iterator;
<DownloadImageCoroutine>c__Iterator
}

public void OnGetUserFromWeb(string response)
{ 
this.users.ReadGetFromWebResponse(response);
}
}
