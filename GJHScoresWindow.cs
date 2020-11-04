using System;
using UnityEngine;


public class GJHScoresWindow : GJHWindow
{ 
private enum ScoresWindowStates
{ 
ScoresList
}

private int tablesToolbarIndex;

private int lastTablesToolbarIndex;

private string[] tablesNames;

private GJTable[] tables;

private Vector2 scoresScrollViewPosition;

private GJScore[] scores;

private GUIStyle userScoreStyle;

public GJHScoresWindow()
{ 
this.Title = "Leaderboards";
float num = (float)Screen.width * 0.9f;
num = ((num <= 400f) ? num : 400f);
float num2 = (float)Screen.height * 0.9f;
this.Position = new Rect((float)(Screen.width / 2) - num / 2f, (float)(Screen.height / 2) - num2 / 2f, num, num2);
this.drawWindowDelegates.Add(GJHScoresWindow.ScoresWindowStates.ScoresList.ToString(), new GJHWindow.DrawWindowDelegate(this.DrawScoresList));
this.userScoreStyle = (GJAPIHelper.Skin.FindStyle("UserScore") ?? GJAPIHelper.Skin.label);
num
num2
}

~GJHScoresWindow()
{ 
this.tables = null;
this.scores = null;
this.userScoreStyle = null;
}

public override bool Show()
{ 
bool flag = base.Show();
if (flag)
{ 
if (this.tablesNames != null)
{ 
this.GetScores();
}
else
{ 
this.GetScoreTables();
}
}
return flag;
flag
}

public override bool Dismiss()
{ 
bool flag = base.Dismiss();
if (flag)
{ 
this.scores = null;
}
return flag;
flag
}

private void GetScoreTables()
{ 
base.SetWindowMessage("Loading score tables", string.Empty);
base.ChangeState(GJHWindow.BaseWindowStates.Process.ToString());
this.tables = null;
GJScoresMethods expr_2E = GJAPI.Scores;
expr_2E.GetTablesCallback = (GJScoresMethods._GetTablesCallback)Delegate.Combine(expr_2E.GetTablesCallback, new GJScoresMethods._GetTablesCallback(this.OnGetScoreTables));
GJAPI.Scores.GetTables();
expr_2E
}

private void OnGetScoreTables(GJTable[] t)
{ 
GJScoresMethods expr_05 = GJAPI.Scores;
expr_05.GetTablesCallback = (GJScoresMethods._GetTablesCallback)Delegate.Remove(expr_05.GetTablesCallback, new GJScoresMethods._GetTablesCallback(this.OnGetScoreTables));
if (t == null)
{ 
base.SetWindowMessage("Error loading score tables.", string.Empty);
base.ChangeState(GJHWindow.BaseWindowStates.Error.ToString());
return;
}
this.tables = t;
int num = t.Length;
this.tablesNames = new string[num];
for (int i = 0; i < num; i++)
{ 
this.tablesNames[i] = t[i].Name;
}
this.GetScores();
expr_05
num
i
}

private void GetScores()
{ 
base.SetWindowMessage("Loading Scores", string.Empty);
base.ChangeState(GJHWindow.BaseWindowStates.Process.ToString());
this.scores = null;
GJScoresMethods expr_2E = GJAPI.Scores;
expr_2E.GetMultipleCallback = (GJScoresMethods._GetMultipleCallback)Delegate.Combine(expr_2E.GetMultipleCallback, new GJScoresMethods._GetMultipleCallback(this.OnGetScores));
GJAPI.Scores.Get(false, this.tables[this.tablesToolbarIndex].Id, 100u);
expr_2E
}

private void OnGetScores(GJScore[] s)
{ 
GJScoresMethods expr_05 = GJAPI.Scores;
expr_05.GetMultipleCallback = (GJScoresMethods._GetMultipleCallback)Delegate.Remove(expr_05.GetMultipleCallback, new GJScoresMethods._GetMultipleCallback(this.OnGetScores));
if (s == null)
{ 
base.SetWindowMessage("Error loading scores.", string.Empty);
base.ChangeState(GJHWindow.BaseWindowStates.Error.ToString());
return;
}
this.scores = s;
base.ChangeState(GJHScoresWindow.ScoresWindowStates.ScoresList.ToString());
expr_05
}

private void DrawScoresList()
{ 
GUILayout.BeginHorizontal(new GUILayoutOption[0]);
GUILayout.FlexibleSpace();
this.tablesToolbarIndex = GUILayout.Toolbar(this.tablesToolbarIndex, this.tablesNames, new GUILayoutOption[0]);
if (this.tablesToolbarIndex != this.lastTablesToolbarIndex)
{ 
this.lastTablesToolbarIndex = this.tablesToolbarIndex;
this.GetScores();
return;
}
GUILayout.FlexibleSpace();
GUILayout.EndHorizontal();
GUILayout.Space(10f);
this.scoresScrollViewPosition = GUILayout.BeginScrollView(this.scoresScrollViewPosition, new GUILayoutOption[0]);
int num = this.scores.Length;
for (int i = 0; i < num; i++)
{ 
this.DrawScore(i);
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

private void DrawScore(int s)
{ 
if (GJAPI.User != null && (GJAPI.User.Name == this.scores[s].Name || (GJAPI.User.Type == GJUser.UserType.Developer && GJAPI.User.GetProperty("developer_name") == this.scores[s].Name)))
{ 
GUILayout.BeginHorizontal(this.userScoreStyle, new GUILayoutOption[0]);
}
else
{ 
GUILayout.BeginHorizontal(new GUILayoutOption[0]);
}
GUILayout.Label(this.scores[s].Name, new GUILayoutOption[0]);
GUILayout.FlexibleSpace();
GUILayout.Label(this.scores[s].Score, new GUILayoutOption[0]);
GUILayout.EndHorizontal();
}
}
