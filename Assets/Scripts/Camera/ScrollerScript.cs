using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerScript : MonoBehaviour
{
    public float Speed;
    public ScrollerDirections Direction;

    bool _isPressed;
    Vector3 _direction;
    CameraController _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        SetDirection(Direction);
        //ui = GameObject.Find("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var boxCollider = GetComponent<BoxCollider2D>();
            var mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionInWorld.z = 0;
            if (boxCollider.bounds.Contains(mousePositionInWorld))
            {
                _isPressed = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
            _isPressed = false;

        UpdatePressing();
    }

    void UpdatePressing()
    {
        if (_isPressed)
        {
            Press();
        }
    }

    public void SetDirection(ScrollerDirections dir)
    {
        switch (dir)
        {
            case ScrollerDirections.Up:
                _direction = new Vector3(0, Speed, 0);
                break;
            case ScrollerDirections.Down:
                _direction = new Vector3(0, -Speed, 0);
                break;
            case ScrollerDirections.Left:
                _direction = new Vector3(-Speed, 0, 0);
                break;
            case ScrollerDirections.Right:
                _direction = new Vector3(Speed, 0, 0);
                break;
            default:
                break;
        }
    }

    void Press()
    {
        _camera.Move(_direction);
    }

    
}

public enum ScrollerDirections
{
    Up,
    Down,
    Left,
    Right
}
