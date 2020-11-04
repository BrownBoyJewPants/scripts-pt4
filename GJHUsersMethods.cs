using System;
using UnityEngine;


public class GJHUsersMethods
{ 
private Action<string, string> getFromWebCallback;

private GJHLoginWindow window;

public GJHUsersMethods()
{ 
this.window = new GJHLoginWindow();
}

~GJHUsersMethods()
{ 
this.getFromWebCallback = null;
this.window = null;
}

public void GetFromWeb(Action<string, string> onComplete)
{ 
this.getFromWebCallback = null;
if (onComplete != null)
{ 
onComplete(string.Empty, string.Empty);
}
Debug.Log("GJAPIHelper: The method \"GetFromWeb\" can only be called from WebPlayer builds.");
}

public void ReadGetFromWebResponse(string response)
{ 
if (this.getFromWebCallback == null)
{ 
return;
}
string arg = string.Empty;
string arg2 = string.Empty;
if (response.ToLower() == "false" || response == string.Empty || response.ToLower() == "Guest")
{ 
arg = "Guest";
arg2 = string.Empty;
}
else
{ 
string[] array = response.Split(new char[]
{ 
':'
});
arg = array[0];
arg2 = array[1];
}
this.getFromWebCallback(arg, arg2);
arg
arg2
array
}

public void ShowGreetingNotification()
{ 
if (GJAPI.User == null)
{ 
Debug.LogWarning("GJAPIHelper: There is no verified user to show greetings to ;-(");
return;
}
GJHNotification notification = new GJHNotification(string.Format("Welcome back {0}!", GJAPI.User.Name), string.Empty, null);
GJHNotificationsManager.QueueNotification(notification);
notification
}

public void ShowLogin()
{ 
this.window.Show();
}

public void DismissLogin()
{ 
this.window.Dismiss();
}

public void DownloadUserAvatar(GJUser user, Action<Texture2D> OnComplete)
{ 
GJAPIHelper.DownloadImage(user.AvatarURL, delegate(Texture2D avatar)
{ 
if (avatar == null)
{ 
avatar = (((Texture2D)Resources.Load("Images/UserAvatar", typeof(Texture2D))) ?? new Texture2D(60, 60));
}
if (OnComplete != null)
{ 
OnComplete(avatar);
}
});
<DownloadUserAvatar>c__AnonStorey
}
}
