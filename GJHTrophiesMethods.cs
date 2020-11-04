using System;
using UnityEngine;


public class GJHTrophiesMethods
{ 
private GJHTrophiesWindow window;

public GJHTrophiesMethods()
{ 
this.window = new GJHTrophiesWindow();
}

~GJHTrophiesMethods()
{ 
this.window = null;
}

public void SetSecretTrophies(uint[] ids, bool show)
{ 
this.window.secretTrophies = ids;
this.window.showSecretTrophies = show;
}

public void ShowTrophies()
{ 
this.window.Show();
}

public void DismissTrophies()
{ 
this.window.Dismiss();
}

public void ShowTrophyUnlockNotification(uint trophyID)
{ 
GJTrophiesMethods expr_05 = GJAPI.Trophies;
expr_05.GetOneCallback = (GJTrophiesMethods._GetOneCallback)Delegate.Combine(expr_05.GetOneCallback, new GJTrophiesMethods._GetOneCallback(this.OnGetTrophy));
GJAPI.Trophies.Get(trophyID);
expr_05
}

private void OnGetTrophy(GJTrophy trophy)
{ 
GJTrophiesMethods expr_12 = GJAPI.Trophies;
expr_12.GetOneCallback = (GJTrophiesMethods._GetOneCallback)Delegate.Remove(expr_12.GetOneCallback, new GJTrophiesMethods._GetOneCallback(this.OnGetTrophy));
if (trophy != null)
{ 
this.DownloadTrophyIcon(trophy, delegate(Texture2D tex)
{ 
GJHNotification notification = new GJHNotification(trophy.Title, trophy.Description, tex);
GJHNotificationsManager.QueueNotification(notification);
notification
});
}
<OnGetTrophy>c__AnonStorey
expr_12
}

public void DownloadTrophyIcon(GJTrophy trophy, Action<Texture2D> OnComplete)
{ 
GJAPIHelper.DownloadImage(trophy.ImageURL, delegate(Texture2D icon)
{ 
if (icon == null)
{ 
icon = (((Texture2D)Resources.Load("Images/TrophyIcon", typeof(Texture2D))) ?? new Texture2D(75, 75));
}
if (OnComplete != null)
{ 
OnComplete(icon);
}
});
<DownloadTrophyIcon>c__AnonStorey
}
}
