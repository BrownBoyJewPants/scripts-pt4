using System;
using System.IO;
using UnityEngine;


public class ScreenshotBehaviour : MonoBehaviour
{ 
private int screenshotNr = 1;

private void Update()
{ 
if (Input.GetKeyDown(KeyCode.F12))
{ 
string text;
do
{ 
text = Path.Combine(Application.persistentDataPath, "Screenshot-" + this.screenshotNr++ + ".png");
}
while (File.Exists(text));
Debug.Log("Screenshot saved: " + text);
Application.CaptureScreenshot(text);
}
text
}
}
