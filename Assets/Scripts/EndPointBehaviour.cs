using System.Collections;
using System.Collections.Generic;
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
        StartPoint startPoint = collision.transform.GetComponentInParent<StartPoint>();

        if (startPoint != null)
        {
            _sprite.color = _connectedColor;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Re-check connection after physics updates
        Invoke(nameof(CheckConnection), 0.02f);
    }

    private void CheckConnection()
    {
        StartPoint startPoint = GetComponentInParent<StartPoint>();

        if (startPoint == null)
        {
            _sprite.color = _disconnectedColor;
        }
    }
}
