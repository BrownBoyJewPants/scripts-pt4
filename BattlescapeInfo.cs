using Assets.Scripts.Settings;
using Game.Data;
using Game.GameStates;
using Game.Global;
using Game.Gui;
using Game.Services;
using System;
using System.Collections;
using TileEngine.Particles;
using TileEngine.TileMap;


namespace Game
{ 
public class BattlescapeInfo : IGameInfo
{ 
public GameBehaviour behaviour;

public DateTime roundStartTime;

public Map map;

public readonly VoxelResourceManager resourceManager;

public readonly GameGui gui;

public readonly AudioGui audio;

public readonly GameStateMachine state;

public readonly BattlescapeUnitManager units;

public GameSettings settings;

public TEParticleSystem particles;

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
return this.state;
}
}

public ServiceLocator Services
{ 
get;
private set;
}

public BattlescapeInfo(GameBehaviour behaviour)
{ 
this.behaviour = behaviour;
this.resourceManager = behaviour.resourceManager;
this.units = new BattlescapeUnitManager(this);
this.state = new GameStateMachine(this);
this.gui = new GameGui(this);
this.audio = new AudioGui();
this.Services = new ServiceLocator();
this.Services.Register<BattlescapeUnitManager>(this.units);
this.Services.Register<IUnitManager>(this.units);
this.Services.Register<GameStateMachine>(this.state);
this.Services.Register<GameGui>(this.gui);
this.Services.Register<AudioGui>(this.audio);
this.Services.Register<CameraEffects>(this.gui.cameraEffects);
this.Services.Register<GuiBindings>(this.gui.bindings);
this.Services.Register<SpecialEffects>(new SpecialEffects(behaviour));
}

public void StartCoroutine(IEnumerator routine)
{ 
this.behaviour.StartCoroutine(routine);
}

public void Start()
{ 
this.particles = new TEParticleSystem(TEParticleSystemRenderMethod.Dx11, this.behaviour.maxParticles);
this.Services.Register<TEParticleSystem>(this.particles);
this.settings = new GameSettings(this);
this.Services.Register<VideoSettings>(this.settings.video);
this.gui.bindings.SetVisible("game.enemyturn", false);
for (int i = this.units.Friendlies.Count; i <= 7; i++)
{ 
this.gui.bindings.SetVisible("unit." + i, false);
}
this.state.Push(new RootGameState());
i
}

public void Update()
{ 
this.state.Update();
this.units.Update();
this.particles.Update();
if (this.map.isdirty || this.units.isDirty)
{ 
foreach (Unit current in this.units.Friendlies)
{ 
current.pathField = null;
current.Visibility.isdirty = true;
}
FogMap.needUpdate = true;
this.map.isdirty = false;
this.units.isDirty = false;
}
enumerator
current
}

public void FixedUpdate()
{ 
this.state.FixedUpdate();
this.units.FixedUpdate();
this.particles.FixedUpdate();
}
}
}
