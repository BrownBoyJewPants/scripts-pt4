using System;
using System.Collections.Generic;
using UnityEngine;


public class GJHWindowsManager : MonoBehaviour
{ 
private static GJHWindowsManager instance;

private List<GJHWindow> windows;

private int currentWindow = -1;

public static GJHWindowsManager Instance
{ 
get
{ 
if (GJHWindowsManager.instance == null)
{ 
GJAPIHelper gJAPIHelper = (GJAPIHelper)UnityEngine.Object.FindObjectOfType(typeof(GJAPIHelper));
if (gJAPIHelper == null)
{ 
Debug.LogError("An instance of GJAPIHelper is needed in the scene, but there is none. Can't initialise GJHWindowManager.");
}
else
{ 
GJHWindowsManager.instance = gJAPIHelper.gameObject.AddComponent<GJHWindowsManager>();
if (GJHWindowsManager.instance == null)
{ 
Debug.Log("An error occured creating GJHWindowManager.");
}
}
}
return GJHWindowsManager.instance;
gJAPIHelper
}
}

private void OnDestroy()
{ 
this.windows = null;
GJHWindowsManager.instance = null;
}

private void Awake()
{ 
this.windows = new List<GJHWindow>();
}

public static int RegisterWindow(GJHWindow window)
{ 
GJHWindowsManager.Instance.windows.Add(window);
return GJHWindowsManager.Instance.windows.Count - 1;
}

public static bool ShowWindow(int index)
{ 
if (index < 0)
{ 
Debug.Log("GJAPIHelper: The index of the window can't be negative. Can't show the window " + index);
return false;
}
if (index >= GJHWindowsManager.Instance.windows.Count)
{ 
Debug.Log("GJAPIHelper: The index of the window is out of range. Can't show the window " + index);
return false;
}
if (GJHWindowsManager.Instance.currentWindow == -1)
{ 
GJHWindowsManager.Instance.currentWindow = index;
return true;
}
if (GJHWindowsManager.Instance.currentWindow == index)
{ 
Debug.Log("GJAPIHelper: The window \"" + GJHWindowsManager.Instance.windows[index].Title + "\" is already showing.");
return false;
}
Debug.Log(string.Concat(new string[]
{ 
"GJAPIHelper: ",
GJHWindowsManager.Instance.windows[GJHWindowsManager.Instance.currentWindow].Title,
" window is already showing. Can't show \"",
GJHWindowsManager.Instance.windows[index].Title,
"\" window."
}));
return false;
}

public static bool DismissWindow(int index)
{ 
if (index < 0)
{ 
Debug.Log("GJAPIHelper: The index of the window can't be negative. Can't dismiss the window " + index);
return false;
}
if (index >= GJHWindowsManager.Instance.windows.Count)
{ 
Debug.Log("GJAPIHelper: The index of the window is out of range. Can't dismiss the window " + index);
return false;
}
if (GJHWindowsManager.Instance.currentWindow != index)
{ 
Debug.Log("GJAPIHelper: The window \"" + GJHWindowsManager.Instance.windows[index].Title + "\" isn't already showing. Can't dismiss it.");
return false;
}
GJHWindowsManager.Instance.currentWindow = -1;
return true;
}

public static bool IsWindowShowing(int index)
{ 
return GJHWindowsManager.Instance.currentWindow == index;
}

private void OnGUI()
{ 
if (this.currentWindow != -1)
{ 
this.windows[this.currentWindow].OnGUI();
}
}
}
