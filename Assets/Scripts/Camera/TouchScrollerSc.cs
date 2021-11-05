using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TouchScrollerSc : MonoBehaviour
{
    public float ScrollMultiplier = 20;

    private CameraControllerSc _camera;
    private bool _isPressed;

    private Vector3 _previousMousePosition = Vector3.zero;

    public GameObject _armyPointPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<CameraControllerSc>();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            _previousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _isPressed = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _previousMousePosition = Vector3.zero;
            _isPressed = false;

        }

        if (_isPressed)
        {
            var mouseWorldCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var translationDistance = _previousMousePosition - mouseWorldCoord;
            _camera.Move(translationDistance * ScrollMultiplier);
            _previousMousePosition = mouseWorldCoord;
        }

    }
}
