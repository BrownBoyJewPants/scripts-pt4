using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VoxelEngine;
using VoxelEngine.VoxelDataStructures;


namespace Assets.Scripts
{ 
public static class KvxAdapter
{ 
public static VoxelData loadkvx(string filename)
{ 
BinaryReader binaryReader = new BinaryReader(File.OpenRead(filename));
int num = binaryReader.ReadInt32();
int num2 = binaryReader.ReadInt32();
int num3 = binaryReader.ReadInt32();
int num4 = binaryReader.ReadInt32();
int num5 = binaryReader.ReadInt32();
int num6 = binaryReader.ReadInt32();
int num7 = binaryReader.ReadInt32();
ArrayVoxelData arrayVoxelData = new ArrayVoxelData(num2, num4, num3);
int[] array = KvxAdapter.LoadPalette(filename);
ushort[][] array2 = new ushort[num2][];
byte[] array3 = binaryReader.ReadBytes(num2 + 1 << 2);
int[] dst = new int[array3.Length >> 2];
Buffer.BlockCopy(array3, 0, dst, 0, array3.Length);
for (int i = 0; i < num2; i++)
{ 
array3 = binaryReader.ReadBytes(num3 + 1 << 1);
array2[i] = new ushort[array3.Length >> 1];
Buffer.BlockCopy(array3, 0, array2[i], 0, array3.Length);
}
for (int j = 0; j < num2; j++)
{ 
for (int k = 0; k < num3; k++)
{ 
int i = (int)(array2[j][k + 1] - array2[j][k]);
if (i != 0)
{ 
do
{ 
byte[] array4 = binaryReader.ReadBytes(3);
int num8 = (int)array4[0];
int num9 = (int)array4[1];
i -= num9 + 3;
int num10 = num8 + num9;
for (int l = num8; l < num10; l++)
{ 
int num11 = (int)binaryReader.ReadByte();
arrayVoxelData.SetData(j, num4 - l - 1, k, array[num11]);
}
}
while (i != 0);
}
}
}
binaryReader.Close();
KvxAdapter.FloodFill(arrayVoxelData);
return arrayVoxelData;
binaryReader
num
num2
num3
num4
num5
num6
num7
arrayVoxelData
array
array2
array3
dst
i
j
k
array4
num8
num9
num10
l
num11
}

private static void FloodFill(ArrayVoxelData voxeldata)
{ 
bool done = false;
for (int i = 0; i < voxeldata.Dz; i++)
{ 
for (int j = 0; j < voxeldata.Dx; j++)
{ 
if (voxeldata.GetData(j, 0, i) == 0)
{ 
voxeldata.SetData(j, 0, i, -1);
}
}
}
int current = -1;
while (!done)
{ 
done = true;
voxeldata.bounds.ForEach(delegate(int x, int y, int z)
{ 
if (voxeldata.GetData(x, y, z) == current)
{ 
if (voxeldata.GetData(x - 1, y, z) == 0)
{ 
voxeldata.SetData(x - 1, y, z, current - 1);
done = false;
}
if (voxeldata.GetData(x + 1, y, z) == 0)
{ 
voxeldata.SetData(x + 1, y, z, current - 1);
done = false;
}
if (voxeldata.GetData(x, y - 1, z) == 0)
{ 
done = false;
voxeldata.SetData(x, y - 1, z, current - 1);
}
if (voxeldata.GetData(x, y + 1, z) == 0)
{ 
done = false;
voxeldata.SetData(x, y + 1, z, current - 1);
}
if (voxeldata.GetData(x, y, z - 1) == 0)
{ 
done = false;
voxeldata.SetData(x, y, z - 1, current - 1);
}
if (voxeldata.GetData(x, y, z + 1) == 0)
{ 
done = false;
voxeldata.SetData(x, y, z + 1, current - 1);
}
}
});
current--;
}
for (int k = -1; k <= voxeldata.Dx; k++)
{ 
for (int l = -1; l <= voxeldata.Dy; l++)
{ 
for (int m = -1; m <= voxeldata.Dz; m++)
{ 
int data = voxeldata.GetData(k, l, m);
if (data == 0)
{ 
voxeldata.SetData(k, l, m, 1);
}
else if (data < 0)
{ 
voxeldata.SetData(k, l, m, 0);
}
}
}
}
<FloodFill>c__AnonStorey2C
i
j
k
l
m
data
}

private static int[] LoadPalette(string filename)
{ 
List<int> list = new List<int>();
BinaryReader binaryReader = new BinaryReader(File.OpenRead(filename));
binaryReader.BaseStream.Seek(-768L, SeekOrigin.End);
byte[] array = binaryReader.ReadBytes(768);
int num = 0;
for (int i = 0; i < 256; i++)
{ 
Color32 color = new Color((float)array[num++] / 64f, (float)array[num++] / 64f, (float)array[num++] / 64f, 0f);
list.Add(HelperFunctions.ColorToInt(ref color));
}
binaryReader.Close();
return list.ToArray();
list
binaryReader
array
num
i
color
}
}
}
