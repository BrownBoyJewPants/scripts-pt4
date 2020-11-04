using System;


public class GJHScoresMethods
{ 
private GJHScoresWindow window;

public GJHScoresMethods()
{ 
this.window = new GJHScoresWindow();
}

~GJHScoresMethods()
{ 
this.window = null;
}

public void ShowLeaderboards()
{ 
this.window.Show();
}

public void DismissLeaderboards()
{ 
this.window.Dismiss();
}
}
