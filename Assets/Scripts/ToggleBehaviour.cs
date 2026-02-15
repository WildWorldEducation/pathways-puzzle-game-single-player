using UnityEngine;

public class ToggleBehaviour : MonoBehaviour
{
    private SpriteRenderer _sprite;

    private Color _connectedColor = new Color(1, 1, 0, 1);
    private Color _disconnectedColor = new Color(1, 1, 1, 1);

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Detach children
            while (transform.childCount > 0)
            {
                transform.GetChild(0).SetParent(null);
            }

            // Detach self
            transform.SetParent(null);

            // Rotate
            transform.Rotate(0, 0, -90);

            // Temporary colour reset
            _sprite.color = _disconnectedColor;

            ConnectionManager.Instance.MarkDirty();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Direct StartPoint
        if (collision.gameObject.CompareTag("StartPoint"))
        {
            transform.SetParent(collision.transform);
            ConnectionManager.Instance.MarkDirty();
            return;
        }

        // Anything already connected to StartPoint
        StartPoint startPoint =
            collision.transform.GetComponentInParent<StartPoint>();

        if (startPoint != null)
        {
            transform.SetParent(collision.transform);
            ConnectionManager.Instance.MarkDirty();
        }
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
        {
            _sprite.color = _connectedColor;
        }
        else
        {
            _sprite.color = _disconnectedColor;
            transform.SetParent(null);
        }
    }

    private void RefreshAll()
    {
        ConnectionManager.Instance.RefreshConnections();
    }
}
