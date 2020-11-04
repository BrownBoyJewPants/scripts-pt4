using System;
using UnityEngine;


public class CasterCollider
{ 
public enum CasterType
{ 
BoxCollider2d,
CircleCollider2d,
PolygonCollider2d,
EdgeCollider2d
}

public Collider2D collider;

public Vector2[] points;

public Transform transform;

public int TotalPointsCount;

public CasterCollider.CasterType type;

internal float lastRadius;

internal float lastZRot;

internal CasterCollider(PolygonCollider2D coll)
{ 
this.collider = coll;
this.transform = coll.transform;
this.TotalPointsCount = coll.GetTotalPointCount();
this.points = new Vector2[this.TotalPointsCount];
this.points = coll.points;
this.type = CasterCollider.CasterType.PolygonCollider2d;
}

internal CasterCollider(BoxCollider2D coll)
{ 
this.collider = coll;
this.transform = coll.transform;
this.TotalPointsCount = 4;
this.points = new Vector2[this.TotalPointsCount];
this.points = this.getSquarePoints();
this.type = CasterCollider.CasterType.BoxCollider2d;
}

internal CasterCollider(CircleCollider2D coll, Vector2 lightSource)
{ 
this.collider = coll;
this.transform = coll.transform;
this.TotalPointsCount = 2;
this.points = new Vector2[this.TotalPointsCount];
this.points = this.getCirclePoints(lightSource);
this.type = CasterCollider.CasterType.CircleCollider2d;
}

internal CasterCollider(EdgeCollider2D coll)
{ 
this.collider = coll;
this.transform = coll.transform;
this.TotalPointsCount = coll.pointCount;
this.points = new Vector2[this.TotalPointsCount];
this.points = coll.points;
this.type = CasterCollider.CasterType.EdgeCollider2d;
}

internal Vector2[] getSquarePoints()
{ 
Collider2D collider2D = this.collider;
this.lastZRot = this.transform.localEulerAngles.z;
if (this.lastZRot != 0f)
{ 
this.transform.localEulerAngles = Vector3.zero;
}
Rect rect = default(Rect);
rect.x = collider2D.bounds.min.x;
rect.y = collider2D.bounds.min.y;
rect.width = collider2D.bounds.max.x;
rect.height = collider2D.bounds.max.y;
Vector2[] array = new Vector2[4];
array[0].x = rect.x;
array[0].y = rect.y;
array[1].x = rect.width;
array[1].y = rect.y;
array[2].x = rect.width;
array[2].y = rect.height;
array[3].x = rect.x;
array[3].y = rect.height;
for (int i = 0; i < this.TotalPointsCount; i++)
{ 
array[i] = this.transform.InverseTransformPoint(array[i]);
}
this.transform.localEulerAngles = new Vector3(0f, 0f, this.lastZRot);
return array;
collider2D
rect
array
i
}

internal Vector2[] getCirclePoints(Vector2 lightSource)
{ 
CircleCollider2D circleCollider2D = (CircleCollider2D)this.collider;
float num = circleCollider2D.radius * this.transform.localScale.x * 1.0001f;
Vector2 a = this.transform.position;
Vector2 vector = a - lightSource;
float num2 = Mathf.Atan2(vector.y, vector.x);
float magnitude = vector.magnitude;
float num3 = Mathf.Asin(num / magnitude);
float f = num2 - num3;
float f2 = num2 + num3;
Vector2 vector2 = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
vector2.Normalize();
vector2 *= magnitude;
vector2 += lightSource;
num3 = num2 + num3;
Vector2 vector3 = new Vector2(Mathf.Cos(f2), Mathf.Sin(f2));
vector3.Normalize();
vector3 *= magnitude;
vector3 += lightSource;
return new Vector2[]
{ 
this.transform.InverseTransformPoint(vector2),
this.transform.InverseTransformPoint(vector3)
};
circleCollider2D
num
a
vector
num2
magnitude
num3
f
f2
vector2
vector3
}

public void recalcTan(Vector2 source)
{ 
this.points = this.getCirclePoints(source);
}

public void recalcBox()
{ 
this.points = this.getSquarePoints();
}

public int getTotalPointsCount()
{ 
return this.TotalPointsCount;
}
}
