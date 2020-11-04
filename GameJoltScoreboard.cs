using Assets.Scripts.Achievements;
using Game.Gui;
using System;


public class GameJoltScoreboard : IScoreboard
{ 
private bool isverified;

private bool verificationSuccess;

private string username;

private GuiBindings bindings;

public string Username
{ 
get
{ 
return this.username ?? "Guest";
}
}

public bool Connected
{ 
get
{ 
return this.isverified;
}
}

public GameJoltScoreboard(GuiBindings bindings, int gameId, string privateKey)
{ 
this.bindings = bindings;
GJAPI.Init(gameId, privateKey, true, 1);
GJAPIHelper.Users.GetFromWeb(new Action<string, string>(this.GotUserFromWeb));
GJUsersMethods expr_31 = GJAPI.Users;
expr_31.VerifyCallback = (GJUsersMethods._VerifyCallback)Delegate.Combine(expr_31.VerifyCallback, new GJUsersMethods._VerifyCallback(this.OnVerifyUser));
bindings.UpdateValue("username", this.Username);
bindings.UpdateValue("startmessage", this.Username + ", get ready to start...");
expr_31
}

public void Dispose()
{ 
GJUsersMethods expr_05 = GJAPI.Users;
expr_05.VerifyCallback = (GJUsersMethods._VerifyCallback)Delegate.Remove(expr_05.VerifyCallback, new GJUsersMethods._VerifyCallback(this.OnVerifyUser));
this.bindings = null;
expr_05
}

private void GotUserFromWeb(string name, string token)
{ 
this.username = name;
this.bindings.UpdateValue("username", this.username);
this.bindings.UpdateValue("startmessage", this.username + ", get ready to start...");
GJAPI.Users.Verify(name, token);
}

private void OnVerifyUser(bool success)
{ 
this.isverified = true;
this.verificationSuccess = success;
}

public void AddHighscore(uint score)
{ 
if (this.verificationSuccess)
{ 
GJAPI.Scores.Add(score + " kills", score, 0u, string.Empty);
}
}
}
