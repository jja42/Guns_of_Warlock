using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Data_Manager : MonoBehaviour
{
    public static Data_Manager instance;
    public Data data;
    readonly string datafile = "save.sav";
    // Update is called once per frame
    private void Start()
    {
    }
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void Save()
    {
        string filePath = Application.persistentDataPath + "/" + datafile;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);
        bf.Serialize(file, data);
        file.Close();
    }
    public void Load()
    {
        string filePath = Application.persistentDataPath + "/" + datafile;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(filePath))
        {
            FileStream file = File.Open(filePath, FileMode.Open);
            Data loaded = (Data)bf.Deserialize(file);
            data = loaded;
            file.Close();
        }
    }
}
[System.Serializable]
public class Data
{
    public Position player = null;
    public Position[] entities = null;
    public int health = 5;
    public int saved = 0;
    public int[] flags = new int[13];
    public int[,] items = new int[8,1];
}
[System.Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;

    public Position(Vector3 p)
    {
        x = p.x;
        y = p.y;
        z = p.z;
    }
    public Vector3 toVector3()
    {
        return new Vector3(x, y, z);
    }
}