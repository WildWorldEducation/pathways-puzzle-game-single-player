using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class MapLoadManager : MonoBehaviour
{
    //  public Tilemap tilemap;
    //  public List<CustomTile> tiles = new List<CustomTile>();
    private string _mapName;
    // public TileBase HorizontalToggleTile;
    //  public TileBase VerticalToggleTile;

    [SerializeField]
    private GameObject _toggle;
    [SerializeField]
    private GameObject _startPoint;
    [SerializeField]
    private GameObject _endPoint;
    [SerializeField]
    private GameObject _blocker;
    [SerializeField]
    private Transform _mapRoot;

    public void Start()
    {
        AssignMapName("test2");
        // Do not auto-load an empty map JSON on start.
    }
    public void AssignMapName(string mapName)
    {
        _mapName = mapName;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadMap();
    }
    public void LoadMap()
    {
        // Reserved: runtime or scene-load triggered map loading can be implemented here.
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
            LoadMapFromJson(json);
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

    // Parse JSON and instantiate map objects.
    public void LoadMapFromJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogWarning("Empty JSON provided to LoadMapFromJson.");
            return;
        }

        MapData data = JsonUtility.FromJson<MapData>(json);
        if (data == null || data.tiles == null || data.positions == null)
        {
            Debug.LogError("Invalid map JSON.");
            return;
        }

        ClearMap();

        for (int i = 0; i < data.tiles.Count; i++)
        {
            Vector3 positionAdjustedForGrid = new Vector3(data.positions[i].x + 0.5f, data.positions[i].y + 0.5f, 0);


            if (data.tiles[i] == "0")
            {
                if (_toggle != null)
                    Instantiate(_toggle, positionAdjustedForGrid, Quaternion.identity, _mapRoot);
            }
            else if (data.tiles[i] == "1")
            {
                if (_toggle != null)
                    Instantiate(_toggle, positionAdjustedForGrid, transform.rotation * Quaternion.Euler(0f, 0f, 90f), _mapRoot);
            }
            else if (data.tiles[i] == "2")
            {
                if (_startPoint != null)
                    Instantiate(_startPoint, positionAdjustedForGrid, Quaternion.identity, _mapRoot);
            }
            else if (data.tiles[i] == "3")
            {
                if (_endPoint != null)
                    Instantiate(_endPoint, positionAdjustedForGrid, Quaternion.identity, _mapRoot);
            }
            else if (data.tiles[i] == "4")
            {
                if (_blocker != null)
                    Instantiate(_blocker, positionAdjustedForGrid, Quaternion.identity, _mapRoot);
            }
        }
    }

    private void EnsureMapRoot()
    {
        if (_mapRoot != null) return;

        var found = GameObject.Find("MapRoot");
        if (found != null)
        {
            _mapRoot = found.transform;
            return;
        }

        var go = new GameObject("MapRoot");
        go.transform.SetParent(this.transform, false);
        _mapRoot = go.transform;
    }

    private void ClearMap()
    {
        EnsureMapRoot();
        for (int i = _mapRoot.childCount - 1; i >= 0; i--)
        {
            var child = _mapRoot.GetChild(i).gameObject;
            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }
    }
}

[System.Serializable]
public class MapData
{
    public string name;
    public List<string> tiles = new List<string>();
    public List<Vector3Int> positions = new List<Vector3Int>();
}
