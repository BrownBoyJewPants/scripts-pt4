using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class BetterList<T>
{ 
public T[] buffer;

public int size;

public T this[int i]
{ 
get
{ 
return this.buffer[i];
}
set
{ 
this.buffer[i] = value;
}
}

[DebuggerHidden]
public IEnumerator<T> GetEnumerator()
{ 
BetterList<T>.<GetEnumerator>c__Iterator5 <GetEnumerator>c__Iterator = new BetterList<T>.<GetEnumerator>c__Iterator5();
<GetEnumerator>c__Iterator.<>f__this = this;
return <GetEnumerator>c__Iterator;
<GetEnumerator>c__Iterator
}

private void AllocateMore()
{ 
T[] array = (this.buffer == null) ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)];
if (this.buffer != null && this.size > 0)
{ 
this.buffer.CopyTo(array, 0);
}
this.buffer = array;
array
}

private void Trim()
{ 
if (this.size > 0)
{ 
if (this.size < this.buffer.Length)
{ 
T[] array = new T[this.size];
for (int i = 0; i < this.size; i++)
{ 
array[i] = this.buffer[i];
}
this.buffer = array;
}
}
else
{ 
this.buffer = null;
}
array
i
}

public void Clear()
{ 
this.size = 0;
}

public void Release()
{ 
this.size = 0;
this.buffer = null;
}

public void Add(T item)
{ 
if (this.buffer == null || this.size == this.buffer.Length)
{ 
this.AllocateMore();
}
this.buffer[this.size++] = item;
}

public void Insert(int index, T item)
{ 
if (this.buffer == null || this.size == this.buffer.Length)
{ 
this.AllocateMore();
}
if (index < this.size)
{ 
for (int i = this.size; i > index; i--)
{ 
this.buffer[i] = this.buffer[i - 1];
}
this.buffer[index] = item;
this.size++;
}
else
{ 
this.Add(item);
}
i
}

public bool Contains(T item)
{ 
if (this.buffer == null)
{ 
return false;
}
for (int i = 0; i < this.size; i++)
{ 
if (this.buffer[i].Equals(item))
{ 
return true;
}
}
return false;
i
}

public bool Remove(T item)
{ 
if (this.buffer != null)
{ 
EqualityComparer<T> @default = EqualityComparer<T>.Default;
for (int i = 0; i < this.size; i++)
{ 
if (@default.Equals(this.buffer[i], item))
{ 
this.size--;
this.buffer[i] = default(T);
for (int j = i; j < this.size; j++)
{ 
this.buffer[j] = this.buffer[j + 1];
}
return true;
}
}
}
return false;
default
i
j
}

public void RemoveAt(int index)
{ 
if (this.buffer != null && index < this.size)
{ 
this.size--;
this.buffer[index] = default(T);
for (int i = index; i < this.size; i++)
{ 
this.buffer[i] = this.buffer[i + 1];
}
}
i
}

public T Pop()
{ 
if (this.buffer != null && this.size != 0)
{ 
T result = this.buffer[--this.size];
this.buffer[this.size] = default(T);
return result;
}
return default(T);
result
}

public T[] ToArray()
{ 
this.Trim();
return this.buffer;
}

public void Sort(Comparison<T> comparer)
{ 
bool flag = true;
while (flag)
{ 
flag = false;
for (int i = 1; i < this.size; i++)
{ 
if (comparer(this.buffer[i - 1], this.buffer[i]) > 0)
{ 
T t = this.buffer[i];
this.buffer[i] = this.buffer[i - 1];
this.buffer[i - 1] = t;
flag = true;
}
}
}
flag
i
t
}
}
