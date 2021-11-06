using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TouchHandlerSc : MonoBehaviour
{
    public GameObject _armyPointPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonUp(0)) 
            if (false)
        {
            var mousex = (Input.mousePosition.x);
            var mousey = (Input.mousePosition.y);
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousex, mousey, 0));

            var newArmy = Instantiate(_armyPointPrefab, new Vector3(mousePosition.x, mousePosition.y, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            if (Input.GetKeyDown(KeyCode.RightAlt))
                print(mousePosition);

            var armiesFolder = GameObject.Find("ArmyPoints");
            newArmy.transform.SetParent(armiesFolder.transform);
        }
    }
}
