using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 botLeft, topRight;

    float borderEpsilon = 0.2f;

  //  GameObject ui;
    List<Func<Vector3, bool>> bordersCheck;



    public CameraController()
    {
        bordersCheck = new List<Func<Vector3, bool>>
        {
            direction => direction.x > 0 && topRight.x > MapSettings.MapSize.x - borderEpsilon,
            direction => direction.x < 0 && botLeft.x < -MapSettings.MapSize.x + borderEpsilon,
            direction => direction.y > 0 && topRight.y > MapSettings.MapSize.y - borderEpsilon,
            direction => direction.y < 0 && botLeft.y < -MapSettings.MapSize.y + borderEpsilon
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

        botLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        //print position of mouse
        var mousex = (Input.mousePosition.x);
        var mousey = (Input.mousePosition.y);
        var mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mousex, mousey, 0));
        //print(mouseposition);

    }


    public void Move(Vector3 direction)
    {
        if (bordersCheck.Any(func => func(direction)))
            return;

        transform.Translate(direction * Time.deltaTime * 2);
    }


    public float Right
    {
        get { return transform.position.x + Camera.main.orthographicSize; }
    }
}
