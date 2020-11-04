using System;
using UnityEngine;


public struct Box
{ 
public Vector3 Min;

public Vector3 Max;

public Bounds GetBounds(float dx, float dz)
{ 
Vector3 vector = (this.Min + this.Max) * 0.5f;
vector += new Vector3(-dx * 0.5f + 0.5f, 0.5f, -dz * 0.5f + 0.5f);
Vector3 vector2 = new Vector3(Mathf.Abs(this.Min.x - this.Max.x) + 1f, Mathf.Abs(this.Min.y - this.Max.y) + 1f, Mathf.Abs(this.Min.z - this.Max.z) + 1f);
vector2 += Vector3.one * 0.1f;
return new Bounds(vector, vector2);
vector
vector2
}

public Box Fix()
{ 
return new Box
{ 
Min = new Vector3
{ 
x = Mathf.Min(this.Min.x, this.Max.x),
y = Mathf.Min(this.Min.y, this.Max.y),
z = Mathf.Min(this.Min.z, this.Max.z)
},
Max = new Vector3
{ 
x = Mathf.Max(this.Min.x, this.Max.x),
y = Mathf.Max(this.Min.y, this.Max.y),
z = Mathf.Max(this.Min.z, this.Max.z)
}
};
}
}
