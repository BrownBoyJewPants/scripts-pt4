using Game.BaseScape;
using Game.Data;
using System;
using TileEngine.MonoBehaviours;
using TileEngine.TileMap;
using UnityEngine;


namespace Assets.Scripts.Game.Basescape.Monobehaviours
{ 
public class BasescapeBehaviour : MonoBehaviour
{ 
public GraphicsTileMapBehaviour tilemap;

public VoxelResourceManager voxelresourcemanager;

public TemplateResourceManager templates;

private BasescapeInfo basescape;

private void Start()
{ 
IntVector2 intVector = new IntVector2(15, 15);
IntVector2 size = intVector * 8;
ChunkManager.chunkSize = intVector.x;
this.tilemap.transform.position = new Vector3((float)(-(float)size.x) * 0.5f, 0f, (float)(-(float)size.y) * 0.5f);
this.basescape = new BasescapeInfo();
this.basescape.basemap = new BaseMap(this.voxelresourcemanager, size, intVector);
this.tilemap.SetMap(this.basescape.basemap.TileMap);
BaseModule baseModule = new BaseModule
{ 
name = "Barracks",
description = "Soldiers live here",
template = this.templates.GetResource("Barracks")
};
BaseModule baseModule2 = new BaseModule
{ 
description = "Nothing here",
template = this.templates.GetResource("EmptyRoom")
};
for (int i = 0; i < size.x; i += 15)
{ 
for (int j = 0; j < size.y; j += 15)
{ 
this.basescape.basemap.PlaceModule((i != 45 || (j != 45 && j != 60)) ? baseModule2 : baseModule, new IntVector2(i, j));
}
}
intVector
size
baseModule
baseModule2
i
j
}
}
}
