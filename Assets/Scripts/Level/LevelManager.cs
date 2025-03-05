using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<string> listPathLevelData;
    public int levelSelected;
    public int indexLevel;

    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
        LoadLevel();
    }

    void Start()
    {
        if (listPathLevelData.Count > 0)
        {
            indexLevel = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveLevel()
    {
        //LevelData data = new LevelData();
        //data.listPath = listPathLevelData;

        //string json = JsonUtility.ToJson(data);

        //File.WriteAllText(Application.persistentDataPath + "/level.json", json);
    }

    public void LoadLevel()
    {
        //string path = Application.persistentDataPath + "/level.json";

        //if (File.Exists(path))
        //{
        //    string json = File.ReadAllText(path);
        //    LevelData data = JsonUtility.FromJson<LevelData>(json);

        //    listPathLevelData = data.listPath;

        //    //foreach (string item in listPathLevelData)
        //    //{
        //    //    Debug.Log($"src: {item}");
        //    //}

        //    Debug.Log($"Load file by {path}");
        //} else
        //{
        //    Debug.Log($"Not found path {path}");
        //}

        PathLevelScriptableObject pathLevel = AssetDatabase.LoadAssetAtPath<PathLevelScriptableObject>("Assets/GameData/Level/PathLevel.asset");

        if (pathLevel == null)
        {
            Debug.Log("Khong tim thay file PathLevel");
            return;
        }

        Debug.Log("Da load file PathLevel");
        listPathLevelData = pathLevel.pathLevel;
    }

    public void SetLevel(int index)
    {
        if (index >= 0 && index < listPathLevelData.Count)
        {
            levelSelected = index;
        }
    }

    //[System.Serializable]
    //public class LevelData
    //{
    //    public List<string> listPath;
    //}
}
