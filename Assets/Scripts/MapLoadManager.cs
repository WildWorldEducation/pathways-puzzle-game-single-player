using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.ComponentModel;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapLoadManager : MonoBehaviour
{
    private string _mapName;

    [SerializeField]
    private GameObject _toggle;
    [SerializeField]
    private GameObject _startPoint;
    [SerializeField]
    private GameObject _endPoint;
    [SerializeField]
    private GameObject _blocker;

    public void Start()
    {
        var startMap = @"{
    ""name"": ""map"",
    ""tiles"": [
        ""2"",
        ""1"",
        ""1"",
        ""1"",
        ""1"",
        ""1"",
        ""1"",
        ""3""
    ],
    ""positions"": [
        {
            ""x"": -7,
            ""y"": 1,
            ""z"": 0
        },
        {
            ""x"": -6,
            ""y"": 1,
            ""z"": 0
        },
        {
            ""x"": -4,
            ""y"": 1,
            ""z"": 0
        },
        {
            ""x"": -2,
            ""y"": 1,
            ""z"": 0
        },
        {
            ""x"": 0,
            ""y"": 1,
            ""z"": 0
        },
        {
            ""x"": 2,
            ""y"": 1,
            ""z"": 0
        },
        {
            ""x"": 4,
            ""y"": 1,
            ""z"": 0
        },
        {
            ""x"": 5,
            ""y"": 1,
            ""z"": 0
        }
    ]
}";

        LoadMap(startMap);
    }
    public void AssignMapName(string mapName)
    {
        _mapName = mapName;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    // Called by a UI Button. Opens a file picker in the Editor; in builds shows an error.
    public void SelectMapFile()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select Map JSON", "", "json");
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("Map selection cancelled.");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            Debug.Log("Map JSON content: " + json);
            LoadMap(json);
            Debug.Log("Loaded map from: " + path);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to read map file: " + ex.Message);
        }
#else
        Debug.LogError("File picker is only available in the Unity Editor. Add a runtime file browser plugin for builds.");
#endif
    }

    public void LoadMap(string json)
    {
        ClearMap();

        MapData data = JsonUtility.FromJson<MapData>(json);

        for (int i = 0; i < data.tiles.Count; i++)
        {
            Vector3 positionAdjustedForGrid = new Vector3(data.positions[i].x + 0.5f, data.positions[i].y + 0.5f, 0);

            Debug.Log($"Instantiating {data.tiles[i]} at {positionAdjustedForGrid}");

            if (data.tiles[i] == "0")
            {
                Instantiate(_toggle, positionAdjustedForGrid, Quaternion.identity);
            }
            else if (data.tiles[i] == "1")
            {
                Instantiate(_toggle, positionAdjustedForGrid, transform.rotation * Quaternion.Euler(0f, 0f, 90f));
            }
            else if (data.tiles[i] == "2")
            {
                Instantiate(_startPoint, positionAdjustedForGrid, Quaternion.identity);
            }
            else if (data.tiles[i] == "3")
            {
                Instantiate(_endPoint, positionAdjustedForGrid, Quaternion.identity);
            }
            else if (data.tiles[i] == "4")
            {
                Instantiate(_blocker, positionAdjustedForGrid, Quaternion.identity);
            }
        }
    }

    public void ClearMap()
    {
        string[] tagsToClear = { "Toggle", "EndPoint", "StartPoint", "Blocker" };

        foreach (string tag in tagsToClear)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objects)
            {
                Destroy(obj);
            }
        }
    }

}

public class MapData
{
    public string name;
    public List<string> tiles = new List<string>();
    public List<Vector3Int> positions = new List<Vector3Int>();
}
