using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance;

    private bool _needsRefresh = false;

    private void Awake()
    {
        Instance = this;
    }

    public void MarkDirty()
    {
        _needsRefresh = true;
    }

    private void FixedUpdate()
    {
        if (!_needsRefresh) return;

        _needsRefresh = false;
        RefreshConnections();
    }

    public void RefreshConnections()
    {
        ToggleBehaviour[] toggles = FindObjectsOfType<ToggleBehaviour>();
        EndPointBehaviour[] endpoints = FindObjectsOfType<EndPointBehaviour>();

        foreach (var t in toggles)
            t.CheckConnection();

        foreach (var e in endpoints)
            e.CheckConnection();
    }
}
