using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    private void Start()
    {
    }

    public void LoadLevelData(LevelNames level)
    {
        string path = Path.Combine(Application.persistentDataPath, $"/saves/{level}.json");
        if (File.Exists(path))
        {
            LevelData levelData = JsonUtility.FromJson<LevelData>(path);
            SaveDataObject[] objects = FindObjectsByType<SaveDataObject>();
            foreach (ObjectData data in levelData.objects)
            {
                SaveDataObject obj = objects.FirstOrDefault(o => o.DataId == data.objectId);
                switch (data.objectType)
                {
                    case "Lift":
                        LiftObjectData liftData = JsonUtility.FromJson<LiftObjectData>(data.jsonData);
                        obj.transform.SetLocalPositionAndRotation(liftData.position.ToVector(), liftData.rotation.ToQuaternion());
                        if (string.IsNullOrEmpty(liftData.platformName))
                        {
                            PlatformObject platform = GameObject.Find(liftData.platformName).GetComponent<PlatformObject>();
                            platform.PlaceObject(obj.GetComponent<LiftObject>());
                        }
                        break;
                    case "Place":
                        PlaceData place = JsonUtility.FromJson<PlaceData>(data.jsonData);
                        obj.transform.SetLocalPositionAndRotation(place.position.ToVector(), place.rotation.ToQuaternion());
                        break;
                    case "SoundLevers":
                        SoundLightConsoleData leversConsoleData = JsonUtility.FromJson<SoundLightConsoleData>(data.jsonData);
                        obj.GetComponent<SoundLeversScreen>().SetLevers(leversConsoleData.leverTones);
                        break;
                }
            }
        }
    }

    public void SaveData(LevelNames level)
    {
       string path = Path.Combine(Application.persistentDataPath,$"/saves/{level}.json");

        LevelData levelData = new LevelData();
        levelData.levelName = level.ToString();
        SaveDataObject[] objects = FindObjectsByType<SaveDataObject>();
        foreach (SaveDataObject obj in objects)
        {
            ObjectData data = null;
            switch (obj.DataType)
            {
                case "Lift":
                    LiftObject liftObj = obj.GetComponent<LiftObject>();
                    data = new LiftObjectData()
                    {
                        position = new VectorSerializable(obj.transform.position),
                        rotation = new QuaternionSerializable(obj.transform.rotation),
                        platformName = liftObj.Picked ? liftObj.PlatformName : ""
                    };
                    break;
                case "Place":
                    data = new PlaceData()
                    {
                        position = new VectorSerializable(obj.transform.position),
                        rotation = new QuaternionSerializable(obj.transform.rotation)
                    };
                    break;
                case "SoundLevers":
                    data = new SoundLightConsoleData()
                    {
                        leverTones = obj.GetComponent<SoundLeversScreen>().LeversTones
                    };
                    break;
            }
            levelData.objects.Add(new ObjectData()
            {
                objectId = obj.DataId,
                objectType = obj.DataType,
                jsonData = JsonUtility.ToJson(data)
            });
        }
    }
}
