using Game.Data;
using Game.Gui;
using System;
using System.Collections.Generic;
using UnityEngine;


public static class OnScreenDebug
{ 
private static List<string> lines;

private static int nr;

private static float updateInterval;

private static float accum;

private static int frames;

private static float timeleft;

private static float currentfps;

private static float memoryUsed;

public static GameObject DebugCube;

static OnScreenDebug()
{ 
OnScreenDebug.lines = new List<string>();
OnScreenDebug.nr = 0;
OnScreenDebug.updateInterval = 0.5f;
OnScreenDebug.accum = 0f;
OnScreenDebug.frames = 0;
OnScreenDebug.timeleft = OnScreenDebug.updateInterval;
}

public static void Update(IGameInfo game)
{ 
OnScreenDebug.timeleft -= Time.deltaTime;
OnScreenDebug.accum += Time.timeScale / Time.deltaTime;
OnScreenDebug.frames++;
if ((double)OnScreenDebug.timeleft <= 0.0)
{ 
OnScreenDebug.currentfps = OnScreenDebug.accum / (float)OnScreenDebug.frames;
OnScreenDebug.timeleft = OnScreenDebug.updateInterval;
OnScreenDebug.accum = 0f;
OnScreenDebug.frames = 0;
OnScreenDebug.memoryUsed = (float)GC.GetTotalMemory(false) / 1024f / 1024f;
GuiBindings.Instance.UpdateValue("debug.stats", string.Format("fps: {0}\r\nmono: {1:#0.0} MB\r\nstate: {2}", OnScreenDebug.currentfps.ToString("#"), OnScreenDebug.memoryUsed, game.State));
}
}

public static void Log(string message)
{ 
OnScreenDebug.lines.Add(string.Format("{0:00000} {1}", OnScreenDebug.nr++, message));
if (OnScreenDebug.lines.Count == 41)
{ 
OnScreenDebug.lines.RemoveAt(0);
}
}
}
