using System;
using UnityEngine;


public class GJHLoginWindow : GJHWindow
{ 
private enum LoginWindowStates
{ 
LoginForm
}

private string uName = string.Empty;

private string uToken = string.Empty;

public GJHLoginWindow()
{ 
this.Title = "Login to Game Jolt";
this.Position = new Rect((float)(Screen.width / 2 - 150), (float)(Screen.height / 2 - 100), 300f, 200f);
this.drawWindowDelegates.Add(GJHLoginWindow.LoginWindowStates.LoginForm.ToString(), new GJHWindow.DrawWindowDelegate(this.DrawForm));
}

public override bool Show()
{ 
bool flag = base.Show();
if (flag)
{ 
base.ChangeState(GJHLoginWindow.LoginWindowStates.LoginForm.ToString());
}
return flag;
flag
}

public override bool Dismiss()
{ 
return base.Dismiss();
}

private void DrawForm()
{ 
GUILayout.BeginHorizontal(new GUILayoutOption[0]);
GUILayout.Label("Username", new GUILayoutOption[]
{ 
GUILayout.Width(100f)
});
this.uName = GUILayout.TextField(this.uName, new GUILayoutOption[]
{ 
GUILayout.Width(150f)
});
GUILayout.EndHorizontal();
GUILayout.BeginHorizontal(new GUILayoutOption[0]);
GUILayout.Label("Token", new GUILayoutOption[]
{ 
GUILayout.Width(100f)
});
this.uToken = GUILayout.PasswordField(this.uToken, '*', new GUILayoutOption[]
{ 
GUILayout.Width(150f)
});
GUILayout.EndHorizontal();
GUILayout.FlexibleSpace();
if (GUILayout.Button("Login", new GUILayoutOption[0]))
{ 
if (this.uName.Trim() == string.Empty || this.uToken.Trim() == string.Empty)
{ 
base.SetWindowMessage("Either Username or Token is empty.", GJHLoginWindow.LoginWindowStates.LoginForm.ToString());
base.ChangeState(GJHWindow.BaseWindowStates.Error.ToString());
}
else
{ 
GJUsersMethods expr_124 = GJAPI.Users;
expr_124.VerifyCallback = (GJUsersMethods._VerifyCallback)Delegate.Combine(expr_124.VerifyCallback, new GJUsersMethods._VerifyCallback(this.OnVerifyUser));
GJAPI.Users.Verify(this.uName, this.uToken);
base.SetWindowMessage("Connecting", string.Empty);
base.ChangeState(GJHWindow.BaseWindowStates.Process.ToString());
}
}
if (GUILayout.Button("Cancel", new GUILayoutOption[0]))
{ 
this.Dismiss();
}
expr_124
}

private void OnVerifyUser(bool success)
{ 
GJUsersMethods expr_05 = GJAPI.Users;
expr_05.VerifyCallback = (GJUsersMethods._VerifyCallback)Delegate.Remove(expr_05.VerifyCallback, new GJUsersMethods._VerifyCallback(this.OnVerifyUser));
if (!success)
{ 
base.SetWindowMessage("Error logging in.\nPlease check your username and token.", GJHLoginWindow.LoginWindowStates.LoginForm.ToString());
base.ChangeState(GJHWindow.BaseWindowStates.Error.ToString());
}
else
{ 
base.SetWindowMessage(string.Format("Hello {0}!", this.uName), string.Empty);
base.ChangeState(GJHWindow.BaseWindowStates.Success.ToString());
}
expr_05
}
}
