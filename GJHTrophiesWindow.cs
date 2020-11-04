using System;
using System.Collections.Generic;
using UnityEngine;


public class GJHTrophiesWindow : GJHWindow
{ 
private enum TrophiesWindowStates
{ 
TrophiesList
}

private Vector2 trophiesScrollViewPosition;

private GJTrophy[] trophies;

private Texture2D[] trophiesIcons;

private GUIStyle trophyTitleStyle;

private GUIStyle trophyDescriptionStyle;

public uint[] secretTrophies;

public bool showSecretTrophies = true;

public GJHTrophiesWindow()
{ 
this.Title = "Trophies";
float num = (float)Screen.width * 0.9f;
num = ((num <= 500f) ? num : 500f);
float num2 = (float)Screen.height * 0.9f;
this.Position = new Rect((float)(Screen.width / 2) - num / 2f, (float)(Screen.height / 2) - num2 / 2f, num, num2);
this.drawWindowDelegates.Add(GJHTrophiesWindow.TrophiesWindowStates.TrophiesList.ToString(), new GJHWindow.DrawWindowDelegate(this.DrawTrophiesList));
this.trophyTitleStyle = (GJAPIHelper.Skin.FindStyle("TrophyTitle") ?? GJAPIHelper.Skin.label);
this.trophyDescriptionStyle = (GJAPIHelper.Skin.FindStyle("TrophyDescription") ?? GJAPIHelper.Skin.label);
num
num2
}

~GJHTrophiesWindow()
{ 
this.trophies = null;
this.trophiesIcons = null;
this.trophyTitleStyle = null;
this.trophyDescriptionStyle = null;
this.secretTrophies = null;
}

public override bool Show()
{ 
bool flag = base.Show();
if (flag)
{ 
this.GetTrophies();
}
return flag;
flag
}

public override bool Dismiss()
{ 
return base.Dismiss();
}

private void GetTrophies()
{ 
base.SetWindowMessage("Loading trophies", string.Empty);
base.ChangeState(GJHWindow.BaseWindowStates.Process.ToString());
GJTrophiesMethods expr_27 = GJAPI.Trophies;
expr_27.GetAllCallback = (GJTrophiesMethods._GetAllCallback)Delegate.Combine(expr_27.GetAllCallback, new GJTrophiesMethods._GetAllCallback(this.OnGetTrophies));
GJAPI.Trophies.GetAll();
expr_27
}

private void OnGetTrophies(GJTrophy[] t)
{ 
GJTrophiesMethods expr_05 = GJAPI.Trophies;
expr_05.GetAllCallback = (GJTrophiesMethods._GetAllCallback)Delegate.Remove(expr_05.GetAllCallback, new GJTrophiesMethods._GetAllCallback(this.OnGetTrophies));
if (t == null)
{ 
base.SetWindowMessage("Error loading trophies.", string.Empty);
base.ChangeState(GJHWindow.BaseWindowStates.Error.ToString());
return;
}
this.trophies = t;
int num = this.trophies.Length;
this.trophiesIcons = new Texture2D[num];
for (int i = 0; i < num; i++)
{ 
this.trophiesIcons[i] = (((Texture2D)Resources.Load("Images/TrophyIcon", typeof(Texture2D))) ?? new Texture2D(75, 75));
int index = i;
GJAPIHelper.Trophies.DownloadTrophyIcon(this.trophies[i], delegate(Texture2D icon)
{ 
this.trophiesIcons[index] = icon;
});
}
base.ChangeState(GJHTrophiesWindow.TrophiesWindowStates.TrophiesList.ToString());
expr_05
num
i
<OnGetTrophies>c__AnonStorey
}

private void DrawTrophiesList()
{ 
this.trophiesScrollViewPosition = GUILayout.BeginScrollView(this.trophiesScrollViewPosition, new GUILayoutOption[0]);
int num = this.trophies.Length;
int i = 0;
while (i < num)
{ 
if (this.secretTrophies == null || this.secretTrophies.Length <= 0 || !((ICollection<uint>)this.secretTrophies).Contains(this.trophies[i].Id) || this.trophies[i].Achieved)
{ 
this.DrawTrophy(i, true);
goto IL_94;
}
if (this.showSecretTrophies)
{ 
this.DrawTrophy(i, false);
goto IL_94;
}
IL_A7:
i++;
continue;
IL_94:
if (i != num - 1)
{ 
GUILayout.Space(10f);
goto IL_A7;
}
goto IL_A7;
}
GUILayout.EndScrollView();
GUILayout.Space(10f);
GUILayout.BeginHorizontal(new GUILayoutOption[0]);
GUILayout.FlexibleSpace();
if (GUILayout.Button("Close", new GUILayoutOption[0]))
{ 
this.Dismiss();
}
GUILayout.EndHorizontal();
num
i
}

private void DrawTrophy(int t, bool show)
{ 
GUILayout.BeginHorizontal(new GUILayoutOption[0]);
GUI.enabled = this.trophies[t].Achieved;
GUILayout.Label(this.trophiesIcons[t], new GUILayoutOption[0]);
GUI.enabled = true;
GUILayout.Space(10f);
GUILayout.BeginVertical("box", new GUILayoutOption[]
{ 
GUILayout.Height(75f)
});
GUILayout.FlexibleSpace();
GUILayout.Label((!show) ? "???" : this.trophies[t].Title, this.trophyTitleStyle, new GUILayoutOption[0]);
GUILayout.Space(5f);
GUILayout.Label((!show) ? "???" : this.trophies[t].Description, this.trophyDescriptionStyle, new GUILayoutOption[0]);
GUILayout.FlexibleSpace();
GUILayout.EndVertical();
GUILayout.FlexibleSpace();
GUILayout.EndHorizontal();
}
}
