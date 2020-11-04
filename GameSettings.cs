using Game.Data;
using System;


namespace Assets.Scripts.Settings
{ 
public class GameSettings
{ 
public readonly AudioSettings audio;

public readonly VideoSettings video;

public GameSettings(IGameInfo game)
{ 
this.video = new VideoSettings(game.Services);
this.audio = new AudioSettings();
}
}
}
