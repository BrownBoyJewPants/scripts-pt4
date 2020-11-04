using System;
using UnityEngine;


public class SearchTexture
{ 
public Texture2D alphaTex;

public static byte[] searchTexBytes;

public SearchTexture()
{ 
this.alphaTex = new Texture2D(64, 16, TextureFormat.Alpha8, false);
this.alphaTex.wrapMode = TextureWrapMode.Repeat;
this.alphaTex.anisoLevel = 0;
this.alphaTex.filterMode = FilterMode.Point;
for (int i = 0; i < 1024; i++)
{ 
int x = i % 64;
int y = i / 64;
float a = (float)SearchTexture.searchTexBytes[i] / 255f;
Color color = new Color(0f, 0f, 0f, a);
this.alphaTex.SetPixel(x, y, new Color(0f, 0f, 0f, a));
}
this.alphaTex.Apply();
i
x
y
a
color
}

static SearchTexture()
{ 
// Note: this type is marked as 'beforefieldinit'.
byte[] expr_0A = new byte[1024];
expr_0A[0] = 254;
expr_0A[1] = 254;
expr_0A[3] = 127;
expr_0A[4] = 127;
expr_0A[7] = 254;
expr_0A[8] = 254;
expr_0A[10] = 127;
expr_0A[11] = 127;
expr_0A[21] = 127;
expr_0A[22] = 127;
expr_0A[24] = 127;
expr_0A[25] = 127;
expr_0A[28] = 127;
expr_0A[29] = 127;
expr_0A[31] = 127;
expr_0A[32] = 127;
expr_0A[33] = 254;
expr_0A[34] = 127;
expr_0A[40] = 127;
expr_0A[41] = 127;
expr_0A[64] = 254;
expr_0A[65] = 254;
expr_0A[67] = 127;
expr_0A[68] = 127;
expr_0A[71] = 254;
expr_0A[72] = 254;
expr_0A[74] = 127;
expr_0A[75] = 127;
expr_0A[85] = 127;
expr_0A[86] = 127;
expr_0A[88] = 127;
expr_0A[89] = 127;
expr_0A[92] = 127;
expr_0A[93] = 127;
expr_0A[95] = 127;
expr_0A[96] = 127;
expr_0A[97] = 254;
expr_0A[98] = 127;
expr_0A[104] = 127;
expr_0A[105] = 127;
expr_0A[192] = 254;
expr_0A[193] = 254;
expr_0A[195] = 127;
expr_0A[196] = 127;
expr_0A[199] = 254;
expr_0A[200] = 254;
expr_0A[202] = 127;
expr_0A[203] = 127;
expr_0A[213] = 127;
expr_0A[214] = 127;
expr_0A[216] = 127;
expr_0A[217] = 127;
expr_0A[220] = 127;
expr_0A[221] = 127;
expr_0A[223] = 127;
expr_0A[224] = 127;
expr_0A[225] = 254;
expr_0A[226] = 127;
expr_0A[232] = 127;
expr_0A[233] = 127;
expr_0A[256] = 254;
expr_0A[257] = 254;
expr_0A[259] = 127;
expr_0A[260] = 127;
expr_0A[263] = 254;
expr_0A[264] = 254;
expr_0A[266] = 127;
expr_0A[267] = 127;
expr_0A[277] = 127;
expr_0A[278] = 127;
expr_0A[280] = 127;
expr_0A[281] = 127;
expr_0A[284] = 127;
expr_0A[285] = 127;
expr_0A[287] = 127;
expr_0A[288] = 127;
expr_0A[289] = 254;
expr_0A[290] = 127;
expr_0A[296] = 127;
expr_0A[297] = 127;
expr_0A[448] = 127;
expr_0A[449] = 127;
expr_0A[451] = 127;
expr_0A[452] = 127;
expr_0A[455] = 127;
expr_0A[456] = 127;
expr_0A[458] = 127;
expr_0A[459] = 127;
expr_0A[469] = 127;
expr_0A[470] = 127;
expr_0A[472] = 127;
expr_0A[473] = 127;
expr_0A[476] = 127;
expr_0A[477] = 127;
expr_0A[479] = 127;
expr_0A[480] = 127;
expr_0A[481] = 127;
expr_0A[482] = 127;
expr_0A[488] = 127;
expr_0A[489] = 127;
expr_0A[512] = 127;
expr_0A[513] = 127;
expr_0A[515] = 127;
expr_0A[516] = 127;
expr_0A[519] = 127;
expr_0A[520] = 127;
expr_0A[522] = 127;
expr_0A[523] = 127;
expr_0A[533] = 127;
expr_0A[534] = 127;
expr_0A[536] = 127;
expr_0A[537] = 127;
expr_0A[540] = 127;
expr_0A[541] = 127;
expr_0A[543] = 127;
expr_0A[544] = 127;
expr_0A[545] = 127;
expr_0A[546] = 127;
expr_0A[552] = 127;
expr_0A[553] = 127;
expr_0A[640] = 127;
expr_0A[641] = 127;
expr_0A[643] = 127;
expr_0A[644] = 127;
expr_0A[647] = 127;
expr_0A[648] = 127;
expr_0A[650] = 127;
expr_0A[651] = 127;
expr_0A[661] = 127;
expr_0A[662] = 127;
expr_0A[664] = 127;
expr_0A[665] = 127;
expr_0A[668] = 127;
expr_0A[669] = 127;
expr_0A[671] = 127;
expr_0A[672] = 127;
expr_0A[673] = 127;
expr_0A[674] = 127;
expr_0A[680] = 127;
expr_0A[681] = 127;
expr_0A[704] = 127;
expr_0A[705] = 127;
expr_0A[707] = 127;
expr_0A[708] = 127;
expr_0A[711] = 127;
expr_0A[712] = 127;
expr_0A[714] = 127;
expr_0A[715] = 127;
expr_0A[725] = 127;
expr_0A[726] = 127;
expr_0A[728] = 127;
expr_0A[729] = 127;
expr_0A[732] = 127;
expr_0A[733] = 127;
expr_0A[735] = 127;
expr_0A[736] = 127;
expr_0A[737] = 127;
expr_0A[738] = 127;
expr_0A[744] = 127;
expr_0A[745] = 127;
SearchTexture.searchTexBytes = expr_0A;
expr_0A
}
}
