using Assets.Scripts.Achievements;
using Game.Data;
using Game.GameStates;
using Game.Gui;
using Game.Services;
using System;
using TileEngine.Particles;
using TileEngine.TileMap;
using Zombiescape.States.GameStates;


namespace Game.Zombiescape
{ 
public class ZombiescapeInfo : IGameInfo
{ 
public Map map;

public Player player;

public ZombiescapeBehaviour behaviour;

public ZombieUnitManager units;

public AmmoManager ammo;

public GameJoltScoreboard gamejolt;

public readonly GameStateMachine State;

private TEParticleSystem particles;

IUnitManager IGameInfo.Units
{ 
get
{ 
return this.units;
}
}

Map IGameInfo.Map
{ 
get
{ 
return this.map;
}
}

IGameBehaviour IGameInfo.Behaviour
{ 
get
{ 
return this.behaviour;
}
}

GameStateMachine IGameInfo.State
{ 
get
{ 
return this.State;
}
}

public ServiceLocator Services
{ 
get;
private set;
}

public ZombiescapeInfo(ServiceLocator services)
{ 
this.State = new GameStateMachine(this);
this.Services = services;
}

public void Initialize()
{ 
this.Services.Register<AmmoManager>(this.ammo = new AmmoManager());
this.Services.Register<IUnitManager>(this.units = new ZombieUnitManager(this, this.player));
this.particles = this.Services.Get<TEParticleSystem>();
this.Services.Register<GuiBindings>(new GuiBindings());
this.Services.Register<IScoreboard>(new DummyScoreboard());
this.State.Push(new StartMenuState(this.behaviour.Menu.FindMenu("StartMenu")));
}

public void Update()
{ 
this.State.Update();
if (this.State.Current != null && !this.State.Current.ismodal)
{ 
this.particles.Update();
this.ammo.Update();
this.units.Update();
}
OnScreenDebug.Update(this);
}

public void FixedUpdate()
{ 
this.State.FixedUpdate();
if (this.State.Current != null && !this.State.Current.ismodal)
{ 
this.particles.FixedUpdate();
this.units.FixedUpdate();
}
}
}
}
