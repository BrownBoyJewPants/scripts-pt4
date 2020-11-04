using System;
using UnityEngine;


public static class ColorHelper
{ 
public static Color32 GetSplatterColor(Color32 bloodColor)
{ 
return Color32.Lerp(bloodColor, Color.black, (float)UnityEngine.Random.Range(0, 2) * 0.2f);
}

public static Color32 GetGroundColor(Color32 color)
{ 
return Color32.Lerp(color, Color.black, UnityEngine.Random.Range(0.05f, 0.3f));
}
}
