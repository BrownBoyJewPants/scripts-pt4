using System;
using System.Collections.Generic;
using UnityEngine;


public class GJSessionsMethods
{ 
public delegate void _OpenCallback(bool success);

public delegate void _PingCallback(bool success);

public delegate void _CloseCallback(bool success);

private const string SESSIONS_OPEN = "sessions/open/";

private const string SESSIONS_PING = "sessions/ping/";

private const string SESSIONS_CLOSE = "sessions/close/";

public GJSessionsMethods._OpenCallback OpenCallback;

public GJSessionsMethods._PingCallback PingCallback;

public GJSessionsMethods._CloseCallback CloseCallback;

~GJSessionsMethods()
{ 
this.OpenCallback = null;
this.PingCallback = null;
this.CloseCallback = null;
}

public void Open()
{ 
GJAPI.Instance.GJDebug("Openning Session.", LogType.Log);
GJAPI.Instance.Request("sessions/open/", null, true, new Action<string>(this.ReadOpenResponse));
}

private void ReadOpenResponse(string response)
{ 
bool flag = GJAPI.Instance.IsResponseSuccessful(response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not open the session.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.GJDebug("Session successfully opened.", LogType.Log);
}
if (this.OpenCallback != null)
{ 
this.OpenCallback(flag);
}
flag
}

public void Ping(bool active = true)
{ 
GJAPI.Instance.GJDebug("Pinging Session.", LogType.Log);
Dictionary<string, string> dictionary = new Dictionary<string, string>();
string value = (!active) ? "idle" : "active";
dictionary.Add("status", value);
GJAPI.Instance.Request("sessions/ping/", dictionary, true, new Action<string>(this.ReadPingResponse));
dictionary
value
}

private void ReadPingResponse(string response)
{ 
bool flag = GJAPI.Instance.IsResponseSuccessful(response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not ping the session.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.GJDebug("Session successfully pinged.", LogType.Log);
}
if (this.PingCallback != null)
{ 
this.PingCallback(flag);
}
flag
}

public void Close()
{ 
GJAPI.Instance.GJDebug("Closing Session.", LogType.Log);
GJAPI.Instance.Request("sessions/close/", null, true, new Action<string>(this.ReadCloseResponse));
}

private void ReadCloseResponse(string response)
{ 
bool flag = GJAPI.Instance.IsResponseSuccessful(response);
if (!flag)
{ 
GJAPI.Instance.GJDebug("Could not close the session.\n" + response, LogType.Error);
}
else
{ 
GJAPI.Instance.GJDebug("Session successfully closed.", LogType.Log);
}
if (this.CloseCallback != null)
{ 
this.CloseCallback(flag);
}
flag
}
}
