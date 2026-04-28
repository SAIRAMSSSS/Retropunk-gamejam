using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string levelName;
    public List<ObjectData> objects = new List<ObjectData>();
}

[Serializable]
public class ObjectData
{
    public string objectId;
    public string objectType;
    public string jsonData;
}

[Serializable]
public class LiftObjectData : ObjectData
{
    public VectorSerializable position;
    public QuaternionSerializable rotation;
    public string platformName;
}

[Serializable]
public class PlaceData : ObjectData
{
    public VectorSerializable position;
    public QuaternionSerializable rotation;
}

[Serializable]
public class SoundLightConsoleData : ObjectData
{
    public int[] leverTones;
}

[Serializable]
public class VectorSerializable
{
    public float x;
    public float y;
    public float z;

    public VectorSerializable(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector() => new Vector3(x, y, z);
}

[Serializable]
public class QuaternionSerializable
{
    public float x;
    public float y;
    public float z;
    public float w;

    public QuaternionSerializable(Quaternion q)
    {
        x = q.x;
        y = q.y;
        z = q.z;
    }

    public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
}
