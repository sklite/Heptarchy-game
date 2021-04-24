using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 _botLeft, _topRight;

    float _borderEpsilon = 0.2f;

  //  GameObject ui;
    List<Func<Vector3, bool>> _bordersCheck;



    public CameraController()
    {
        _bordersCheck = new List<Func<Vector3, bool>>
        {
            direction => direction.x > 0 && _topRight.x > MapSc.MapSize.x - _borderEpsilon,
            direction => direction.x < 0 && _botLeft.x < -MapSc.MapSize.x + _borderEpsilon,
            direction => direction.y > 0 && _topRight.y > MapSc.MapSize.y - _borderEpsilon,
            direction => direction.y < 0 && _botLeft.y < -MapSc.MapSize.y + _borderEpsilon
        };

    }
    // Use this for initialization
    void Start()
    {
     //   ui = GameObject.Find("Controls");


    }

    // Update is called once per frame
    void Update()
    {
        _botLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        _topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        //print position of mouse
        var mousex = (Input.mousePosition.x);
        var mousey = (Input.mousePosition.y);
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousex, mousey, 0));
        if (Input.GetKeyDown(KeyCode.RightAlt))
            print(mousePosition);
    }


    public void Move(Vector3 direction)
    {
        if (_bordersCheck.Any(func => func(direction)))
            return;

        transform.Translate(direction * Time.deltaTime * 2);
    }


    public float Right
    {
        get { return transform.position.x + Camera.main.orthographicSize; }
    }
}
