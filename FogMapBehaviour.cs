using Game.Data;
using System;
using UnityEngine;


namespace Game.MonoBehaviours
{ 
public class FogMapBehaviour : MonoBehaviour
{ 
public MonoBehaviour game;

public IGameBehaviour gameBehaviour;

private FogMap fogmap;

private void Start()
{ 
this.gameBehaviour = (this.game as IGameBehaviour);
if (this.gameBehaviour == null)
{ 
Debug.LogError("No game connected to fogmap");
return;
}
ITileMapBehaviour tileMap = this.gameBehaviour.TileMap;
IGameInfo gameInfo = this.gameBehaviour.Game;
this.fogmap = new FogMap(this.gameBehaviour.Game);
this.fogmap.Update();
tileMap.Material.mainTexture = this.fogmap.texture;
tileMap.Material.SetVector("_WorldSize", new Vector4((float)gameInfo.Map.Dx, (float)gameInfo.Map.Dy, (float)gameInfo.Map.Dz, 0f));
tileMap
gameInfo
}

private void Update()
{ 
if (FogMap.needUpdate)
{ 
this.fogmap.Update();
}
}
}
}
