using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraControllerSc : MonoBehaviour
{
    Vector2 _botLeft, _topRight;

    float _borderEpsilon = 0.2f;

  //  GameObject ui;
    List<Func<Vector3, bool>> _bordersCheck;



    public CameraControllerSc()
    {
        _bordersCheck = new List<Func<Vector3, bool>>
        {
            direction => direction.x > 0 && _topRight.x > MapSc.ScrollableMapSize.x - _borderEpsilon,
            direction => direction.x < 0 && _botLeft.x < -MapSc.ScrollableMapSize.x + _borderEpsilon,
            direction => direction.y > 0 && _topRight.y > MapSc.ScrollableMapSize.y - _borderEpsilon,
            direction => direction.y < 0 && _botLeft.y < -MapSc.ScrollableMapSize.y + _borderEpsilon
        };

        //_bordersCheck = new List<Func<Vector3, bool>>
        //{
        //    direction => direction.x > 0 && _topRight.x > MapSc.MapSize.x - _borderEpsilon,
        //    direction => direction.x < 0 && _botLeft.x < -MapSc.MapSize.x + _borderEpsilon,
        //    direction => direction.y > 0 && _topRight.y > MapSc.MapSize.y - _borderEpsilon,
        //    direction => direction.y < 0 && _botLeft.y < -MapSc.MapSize.y + _borderEpsilon
        //};

    }
    // Use this for initialization
    void Start()
    {
        //   ui = GameObject.Find("Controls");
        Application.targetFrameRate = 60;
        Camera.main.aspect = 16f / 9f;
        QualitySettings.vSyncCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //for scrolling
        //_botLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        //_topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
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

    public void Move(Vector2 direction)
    {
        transform.Translate(direction, Space.Self);
    }
}
