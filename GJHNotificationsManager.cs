using System;
using System.Collections.Generic;
using UnityEngine;


public class GJHNotificationsManager : MonoBehaviour
{ 
private static GJHNotificationsManager instance;

private Queue<GJHNotification> queue = new Queue<GJHNotification>();

private GJHNotification currentNotification;

private float currentNotificationAppearTime;

public static GJHNotificationsManager Instance
{ 
get
{ 
if (GJHNotificationsManager.instance == null)
{ 
GJAPIHelper gJAPIHelper = (GJAPIHelper)UnityEngine.Object.FindObjectOfType(typeof(GJAPIHelper));
if (gJAPIHelper == null)
{ 
Debug.LogError("An instance of GJAPIHelper is needed in the scene, but there is none. Can't initialise GJHNotificationsManager.");
}
else
{ 
GJHNotificationsManager.instance = gJAPIHelper.gameObject.AddComponent<GJHNotificationsManager>();
if (GJHNotificationsManager.instance == null)
{ 
Debug.Log("An error occured creating GJHNotificationsManager.");
}
}
}
return GJHNotificationsManager.instance;
gJAPIHelper
}
}

private void OnDestroy()
{ 
this.queue = null;
this.currentNotification = null;
GJHNotificationsManager.instance = null;
}

public static void QueueNotification(GJHNotification notification)
{ 
GJHNotificationsManager.Instance.queue.Enqueue(notification);
}

private void OnGUI()
{ 
if (this.currentNotification != null)
{ 
if (Time.time > this.currentNotificationAppearTime + this.currentNotification.DisplayTime)
{ 
this.currentNotification = null;
}
else
{ 
if (GJAPIHelper.Skin != null)
{ 
GUI.skin = GJAPIHelper.Skin;
}
this.currentNotification.OnGUI();
}
}
else if (this.queue.Count > 0)
{ 
this.currentNotification = this.queue.Dequeue();
this.currentNotificationAppearTime = Time.time;
}
}
}
