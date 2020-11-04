using System;
using System.Linq;
using UnityEngine;


namespace Game
{ 
public static class RandomNames
{ 
private static string[] malenames;

private static string[] femalenames;

private static string[] surnames;

private static void InitializeNames()
{ 
if (RandomNames.malenames != null)
{ 
return;
}
RandomNames.surnames = RandomNames.LoadData("data/surnames");
RandomNames.malenames = RandomNames.LoadData("data/malenames");
RandomNames.femalenames = RandomNames.LoadData("data/femalenames");
}

private static string[] LoadData(string resource)
{ 
return (from l in Resources.Load<TextAsset>(resource).text.Split(new string[]
{ 
"\r\n"
}, StringSplitOptions.RemoveEmptyEntries)
select RandomNames.ProperCase(l)).ToArray<string>();
}

private static string ProperCase(string value)
{ 
if (string.IsNullOrEmpty(value))
{ 
return value;
}
return value[0].ToString().ToUpperInvariant() + value.Substring(1).ToLowerInvariant();
}

public static string GetFemaleName()
{ 
RandomNames.InitializeNames();
return string.Format("{0} {1}", RandomNames.RandomValue(RandomNames.femalenames), RandomNames.RandomValue(RandomNames.surnames));
}

public static string GetMaleName()
{ 
RandomNames.InitializeNames();
return string.Format("{0} {1}", RandomNames.RandomValue(RandomNames.malenames), RandomNames.RandomValue(RandomNames.surnames));
}

private static string RandomValue(string[] array)
{ 
return array[UnityEngine.Random.Range(0, array.Length)];
}
}
}
