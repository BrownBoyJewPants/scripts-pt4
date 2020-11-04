using System;
using UnityEngine;


public class GJHNotification
{ 
private GJHNotificationAnchor anchor = GJHNotificationAnchor.TopCenter;

private GJHNotificationType type;

private float displayTime = 5f;

private Rect position;

public string Title = string.Empty;

public string Description = string.Empty;

public Texture2D Icon;

private GUIStyle notificationBgStyle;

private GUIStyle notificationTitleStyle;

private GUIStyle notificationDescriptionStyle;

private GUIStyle smallNotificationTitleStyle;

public GJHNotificationAnchor Anchor
{ 
get
{ 
return this.anchor;
}
set
{ 
this.anchor = value;
this.SetPosition();
}
}

public GJHNotificationType Type
{ 
get
{ 
return this.type;
}
set
{ 
this.type = value;
this.SetPosition();
}
}

public float DisplayTime
{ 
get
{ 
return this.displayTime;
}
set
{ 
this.displayTime = ((value < 1f) ? 1f : value);
}
}

public GJHNotification(string title, string description = "", Texture2D icon = null)
{ 
this.Title = title;
if (!string.IsNullOrEmpty(description))
{ 
this.Description = description;
this.Icon = icon;
this.type = GJHNotificationType.WithIcon;
}
else
{ 
this.type = GJHNotificationType.Simple;
}
this.SetPosition();
this.notificationBgStyle = (GJAPIHelper.Skin.FindStyle("NotificationBg") ?? GJAPIHelper.Skin.label);
this.notificationTitleStyle = (GJAPIHelper.Skin.FindStyle("NotificationTitle") ?? GJAPIHelper.Skin.label);
this.notificationDescriptionStyle = (GJAPIHelper.Skin.FindStyle("NotificationDescription") ?? GJAPIHelper.Skin.label);
this.smallNotificationTitleStyle = (GJAPIHelper.Skin.FindStyle("SmallNotificationTitle") ?? GJAPIHelper.Skin.label);
}

~GJHNotification()
{ 
this.notificationBgStyle = null;
this.notificationTitleStyle = null;
this.notificationDescriptionStyle = null;
this.smallNotificationTitleStyle = null;
this.Icon = null;
}

public void OnGUI()
{ 
GJHNotificationType gJHNotificationType = this.type;
if (gJHNotificationType == GJHNotificationType.Simple || gJHNotificationType != GJHNotificationType.WithIcon)
{ 
this.DrawSmallNotification();
}
else
{ 
this.DrawMediumNotification();
}
gJHNotificationType
}

private void DrawSmallNotification()
{ 
GUI.BeginGroup(this.position, this.notificationBgStyle);
GUI.Label(new Rect(0f, 0f, this.position.width, this.position.height), this.Title, this.smallNotificationTitleStyle);
GUI.EndGroup();
}

private void DrawMediumNotification()
{ 
GUI.BeginGroup(this.position, this.notificationBgStyle);
GUI.DrawTexture(new Rect(10f, 10f, 75f, 75f), this.Icon);
GUI.Label(new Rect(100f, 10f, 290f, 20f), this.Title, this.notificationTitleStyle);
GUI.Label(new Rect(100f, 40f, 290f, 45f), this.Description, this.notificationDescriptionStyle);
GUI.EndGroup();
}

private void SetPosition()
{ 
GJHNotificationType gJHNotificationType = this.Type;
if (gJHNotificationType == GJHNotificationType.Simple || gJHNotificationType != GJHNotificationType.WithIcon)
{ 
this.position = new Rect(0f, 0f, 250f, 25f);
}
else
{ 
this.position = new Rect(0f, 0f, 400f, 95f);
}
switch (this.Anchor)
{ 
case GJHNotificationAnchor.TopLeft: 
IL_8B:
this.position.x = 10f;
this.position.y = 10f;
return;
case GJHNotificationAnchor.TopCenter: 
this.position.x = (float)(Screen.width / 2) - this.position.width / 2f;
this.position.y = 10f;
return;
case GJHNotificationAnchor.TopRight: 
this.position.x = (float)Screen.width - 10f - this.position.width;
this.position.y = 10f;
return;
case GJHNotificationAnchor.BottomLeft: 
this.position.x = 10f;
this.position.y = (float)Screen.height - 10f - this.position.height;
return;
case GJHNotificationAnchor.BottomCenter: 
this.position.x = (float)(Screen.width / 2) - this.position.width / 2f;
this.position.y = (float)Screen.height - 10f - this.position.height;
return;
case GJHNotificationAnchor.BottomRight: 
this.position.x = (float)Screen.width - 10f - this.position.width;
this.position.y = (float)Screen.height - 10f - this.position.height;
return;
 }
goto IL_8B;
gJHNotificationType
}
}
