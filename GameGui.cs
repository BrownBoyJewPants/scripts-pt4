using System;
using System.Linq;
using UnityEngine;


namespace Game.Gui
{ 
public class GameGui
{ 
private static bool initialized;

private static Camera nguiCam;

public readonly PathFieldGui pathfield;

public readonly GameEffects effects;

public readonly SelectedFriendlyGui friendlyGui;

public readonly SelectedEnemyGui enemyGui;

public readonly VisibleUnitGui unitGui;

public readonly GuiBindings bindings;

public readonly CameraEffects cameraEffects;

private BattlescapeInfo game;

public GameGui(BattlescapeInfo game)
{ 
this.game = game;
Material selectionBoxMaterial = Resources.Load<Material>("materials/selectionboxmaterial");
this.pathfield = new PathFieldGui(Resources.Load<Material>("materials/pathfieldgridmaterial"));
this.effects = new GameEffects(game.behaviour, game.resourceManager, selectionBoxMaterial);
this.bindings = new GuiBindings();
this.friendlyGui = new SelectedFriendlyGui(this);
this.enemyGui = new SelectedEnemyGui(this);
this.cameraEffects = new CameraEffects(game.behaviour);
this.unitGui = new VisibleUnitGui(game);
selectionBoxMaterial
}

public static void Dispose()
{ 
GameGui.initialized = false;
GameGui.nguiCam = null;
}

public static void Initialize()
{ 
GameGui.nguiCam = Camera.allCameras.FirstOrDefault((Camera c) => c.GetComponent<UICamera>() != null);
GameGui.initialized = true;
}

public static bool HitGUI(Vector3 pos)
{ 
if (!GameGui.initialized)
{ 
GameGui.Initialize();
}
if (GameGui.nguiCam == null)
{ 
return false;
}
if (GameGui.nguiCam != null)
{ 
Ray ray = GameGui.nguiCam.ScreenPointToRay(pos);
RaycastHit raycastHit;
if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("GUI")))
{ 
return true;
}
}
return false;
ray
raycastHit
}
}
}
