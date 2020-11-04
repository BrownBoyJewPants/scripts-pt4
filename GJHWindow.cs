using System;
using System.Collections.Generic;
using UnityEngine;


public class GJHWindow
{ 
protected enum BaseWindowStates
{ 
Empty,
Process,
Success,
Error
}

protected delegate void DrawWindowDelegate();

public string Title = string.Empty;

public Rect Position;

private int windowID;

private string previousWindowState = string.Empty;

private string currentWindowState = GJHWindow.BaseWindowStates.Empty.ToString();

protected Dictionary<string, GJHWindow.DrawWindowDelegate> drawWindowDelegates;

private string windowMsg = string.Empty;

private string windowReturnState = string.Empty;

private GUIStyle errorStyle;

private GUIStyle successStyle;

private GUIStyle ellipsisStyle;

public GJHWindow()
{ 
this.Title = "Base Window";
this.Position = new Rect((float)(Screen.width / 2 - 150), (float)(Screen.height / 2 - 50), 300f, 100f);
this.windowID = GJHWindowsManager.RegisterWindow(this);
this.drawWindowDelegates = new Dictionary<string, GJHWindow.DrawWindowDelegate>();
this.drawWindowDelegates.Add(GJHWindow.BaseWindowStates.Empty.ToString(), new GJHWindow.DrawWindowDelegate(this.DrawWindowEmpty));
this.drawWindowDelegates.Add(GJHWindow.BaseWindowStates.Process.ToString(), new GJHWindow.DrawWindowDelegate(this.DrawWindowProcessing));
this.drawWindowDelegates.Add(GJHWindow.BaseWindowStates.Success.ToString(), new GJHWindow.DrawWindowDelegate(this.DrawWindowSuccess));
this.drawWindowDelegates.Add(GJHWindow.BaseWindowStates.Error.ToString(), new GJHWindow.DrawWindowDelegate(this.DrawWindowError));
this.errorStyle = (GJAPIHelper.Skin.FindStyle("ErrorMsg") ?? GJAPIHelper.Skin.label);
this.successStyle = (GJAPIHelper.Skin.FindStyle("SuccessMsg") ?? GJAPIHelper.Skin.label);
this.ellipsisStyle = (GJAPIHelper.Skin.FindStyle("Ellipsis") ?? GJAPIHelper.Skin.label);
}

~GJHWindow()
{ 
this.drawWindowDelegates = null;
this.errorStyle = null;
this.successStyle = null;
this.ellipsisStyle = null;
}

public bool IsShowing()
{ 
return GJHWindowsManager.IsWindowShowing(this.windowID);
}

public virtual bool Show()
{ 
return GJHWindowsManager.ShowWindow(this.windowID);
}

public virtual bool Dismiss()
{ 
return GJHWindowsManager.DismissWindow(this.windowID);
}

public void OnGUI()
{ 
if (GJAPIHelper.Skin != null)
{ 
GUI.skin = GJAPIHelper.Skin;
}
GUI.ModalWindow(this.windowID, this.Position, new GUI.WindowFunction(this.DrawWindow), this.Title);
}

private void DrawWindow(int windowID)
{ 
if (this.drawWindowDelegates.ContainsKey(this.currentWindowState))
{ 
this.BeginWindow();
this.drawWindowDelegates[this.currentWindowState]();
this.EndWindow();
}
else
{ 
Debug.Log("Unknown window state. Can't draw the window.");
}
}

private void DrawWindowEmpty()
{ 
GUILayout.Label("I'm an empty window. Nobody likes me ;-(", new GUILayoutOption[0]);
GUILayout.FlexibleSpace();
if (GUILayout.Button("Close", new GUILayoutOption[0]))
{ 
this.Dismiss();
}
}

private void DrawWindowProcessing()
{ 
if (!string.IsNullOrEmpty(this.windowMsg))
{ 
GUILayout.Label(this.windowMsg, new GUILayoutOption[0]);
}
GUILayout.Label(this.AnimatedEllipsis(5, 1.5f), this.ellipsisStyle, new GUILayoutOption[0]);
GUILayout.FlexibleSpace();
}

private void DrawWindowSuccess()
{ 
if (!string.IsNullOrEmpty(this.windowMsg))
{ 
GUILayout.Label(this.windowMsg, this.successStyle, new GUILayoutOption[0]);
}
GUILayout.FlexibleSpace();
if (this.windowReturnState != string.Empty)
{ 
if (GUILayout.Button("Ok", new GUILayoutOption[0]))
{ 
this.ChangeState(this.windowReturnState);
}
}
else if (GUILayout.Button("Close", new GUILayoutOption[0]))
{ 
this.Dismiss();
}
}

private void DrawWindowError()
{ 
if (!string.IsNullOrEmpty(this.windowMsg))
{ 
GUILayout.Label(this.windowMsg, this.errorStyle, new GUILayoutOption[0]);
}
GUILayout.FlexibleSpace();
if (this.windowReturnState != string.Empty)
{ 
if (GUILayout.Button("Ok", new GUILayoutOption[0]))
{ 
this.ChangeState(this.windowReturnState);
}
}
else if (GUILayout.Button("Close", new GUILayoutOption[0]))
{ 
this.Dismiss();
}
}

protected bool ChangeState(string state)
{ 
if (!this.drawWindowDelegates.ContainsKey(state))
{ 
Debug.LogWarning("No such state exist. Can't change window state.");
return false;
}
this.previousWindowState = this.currentWindowState;
this.currentWindowState = state;
return true;
}

protected bool RevertToPreviousState()
{ 
if (string.IsNullOrEmpty(this.previousWindowState.Trim()))
{ 
Debug.LogWarning("No previous state found. Can't revert to previous window state.");
return false;
}
string text = this.currentWindowState;
this.currentWindowState = this.previousWindowState;
this.previousWindowState = text;
return true;
text
}

protected void SetWindowMessage(string msg, string returnState = "")
{ 
this.windowMsg = msg;
this.windowReturnState = returnState;
}

protected void BeginWindow()
{ 
GUILayout.Space(35f);
GUILayout.BeginHorizontal(new GUILayoutOption[0]);
GUILayout.FlexibleSpace();
GUILayout.BeginVertical(new GUILayoutOption[0]);
GUILayout.FlexibleSpace();
}

protected void EndWindow()
{ 
GUILayout.EndVertical();
GUILayout.FlexibleSpace();
GUILayout.EndHorizontal();
}

protected string AnimatedEllipsis(int amount = 3, float speed = 1f)
{ 
return new string('.', (int)(Time.time * speed) % (amount + 1));
}
}
