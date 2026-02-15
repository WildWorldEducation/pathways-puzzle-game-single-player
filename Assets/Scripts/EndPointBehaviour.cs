using UnityEngine;

public class EndPointBehaviour : MonoBehaviour
{
    private SpriteRenderer _sprite;

    private Color _connectedColor = new Color(1, 1, 0, 1);
    private Color _disconnectedColor = new Color(1, 1, 1, 1);

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Allow endpoint to join an existing chain
        StartPoint startPoint =
            collision.transform.GetComponentInParent<StartPoint>();

        if (startPoint != null)
        {
            transform.SetParent(collision.transform);
        }

        ConnectionManager.Instance.MarkDirty();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ConnectionManager.Instance.MarkDirty();
    }

    // Called by ConnectionManager
    public void CheckConnection()
    {
        StartPoint startPoint = GetComponentInParent<StartPoint>();

        if (startPoint != null)
            _sprite.color = _connectedColor;
        else
            _sprite.color = _disconnectedColor;
    }

    private void RefreshAll()
    {
        ConnectionManager.Instance.RefreshConnections();
    }
}
