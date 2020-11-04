using Game.Data;
using System;
using System.Linq;
using TileEngine.TileMap;
using UnityEngine;


namespace Game
{ 
public class FogMap
{ 
public Texture2D texture;

public static bool needUpdate;

private IGameInfo game;

private Color32 dark = new Color32(255, 255, 0, 0);

private Color32 visited = new Color32(255, 255, 0, 0);

private Color32 light = new Color32(255, 255, 255, 255);

private Color32[] pixels;

private int dx;

private int dz;

public FogMap(IGameInfo game)
{ 
this.dx = game.Map.Dx;
this.dz = game.Map.Dz;
this.game = game;
this.texture = new Texture2D(this.dx, this.dz);
this.texture.filterMode = FilterMode.Bilinear;
this.pixels = new Color32[this.dx * this.dz];
for (int i = 0; i < this.pixels.Length; i++)
{ 
this.pixels[i].r = this.dark.r;
this.pixels[i].g = this.dark.g;
}
i
}

public void Update()
{ 
for (int i = 0; i < this.pixels.Length; i++)
{ 
if (this.pixels[i].r > this.visited.r)
{ 
this.pixels[i].r = this.visited.r;
this.pixels[i].g = this.visited.g;
}
}
foreach (IHasVisibilityInfo current in this.game.Units.Friendlies.OfType<IHasVisibilityInfo>())
{ 
current.Visibility.Update();
MapCell[] cells = current.Visibility.cells;
for (int j = 0; j < cells.Length; j++)
{ 
MapCell mapCell = cells[j];
int num = mapCell.mapposition.x + mapCell.mapposition.z * this.dx;
this.pixels[num].r = this.light.r;
this.pixels[num].g = this.light.g;
}
}
this.texture.SetPixels32(this.pixels);
this.texture.Apply();
FogMap.needUpdate = false;
i
enumerator
current
cells
j
mapCell
num
}
}
}
