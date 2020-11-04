using System;


namespace Assets.Scripts.Achievements
{ 
public class DummyScoreboard : IScoreboard
{ 
public bool Connected
{ 
get
{ 
return false;
}
}

public string Username
{ 
get
{ 
return "Offline";
}
}

public void AddHighscore(uint score)
{ 
}

public void Dispose()
{ 
}
}
}
