using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBehaviour : MonoBehaviour
{
    private Collider2D _startPoint;
    private SpriteRenderer _sprite;
    private Color _connectedColor = new Color(1, 1, 0, 1);
    private Color _disconnectedColor = new Color(1, 1, 1, 1);
    private Transform _originalParent;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _startPoint = GameObject.FindWithTag("StartPoint").GetComponent<CircleCollider2D>();
        _originalParent = transform.parent;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Detach all children
            while (transform.childCount > 0)
            {
                transform.GetChild(0).SetParent(null);
            }

            // Detach self
            transform.SetParent(null);

            // Rotate
            transform.Rotate(0, 0, -90);

            // Reset colour temporarily
            _sprite.color = _disconnectedColor;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If we hit a StartPoint directly
        if (collision.gameObject.CompareTag("StartPoint"))
        {
            _sprite.color = _connectedColor;
            transform.SetParent(collision.transform);
        }
        // If we hit something that already has a StartPoint in its parent chain
        else
        {
            StartPoint startPoint = collision.transform.GetComponentInParent<StartPoint>();

            if (startPoint != null)
            {
                _sprite.color = _connectedColor;
                transform.SetParent(collision.transform);
            }
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
            transform.SetParent(null); // detach from chain
        }
    }
}
