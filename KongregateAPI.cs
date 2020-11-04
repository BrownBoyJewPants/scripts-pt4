using Assets.Scripts.Achievements;
using Game.Data;
using Game.Gui;
using System;
using UnityEngine;


public class KongregateAPI : MonoBehaviour, IScoreboard
{ 
private static KongregateAPI instance;

private static bool isKong;

private static bool isloaded;

private static string username;

private static IGameInfo game;

public bool Connected
{ 
get
{ 
return KongregateAPI.isloaded;
}
}

public string Username
{ 
get
{ 
return KongregateAPI.username;
}
}

private void Awake()
{ 
}

private void Start()
{ 
}

public void OnKongregateUserSignedIn(string userInfoString)
{ 
this.OnKongregateAPILoaded(userInfoString);
}

private void OnKongregateAPILoaded(string userInfoString)
{ 
KongregateAPI.instance = this;
KongregateAPI.isloaded = true;
string[] array = userInfoString.Split(new char[]
{ 
'|'
});
int num = int.Parse(array[0]);
KongregateAPI.username = array[1];
string text = array[2];
KongregateAPI.game.Services.Register<IScoreboard>(this);
KongregateAPI.game.Services.Get<GuiBindings>().UpdateValue("username", KongregateAPI.username);
KongregateAPI.game.Services.Get<GuiBindings>().UpdateValue("startmessage", KongregateAPI.username + ", get ready to start...");
array
num
text
}

public void AddHighscore(uint score)
{ 
Application.ExternalCall("kongregate.stats.submit", new object[]
{ 
"Kills",
score
});
}

public void Dispose()
{ 
KongregateAPI.game.Services.Register<IScoreboard>(new DummyScoreboard());
KongregateAPI.game = null;
}
}
