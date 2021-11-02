using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSc : MonoBehaviour
{

    public bool ScrollableMap = false;
    //public static Rect HabitableMapSize = new Rect(-20, -9.8f, 40, 19.6f);
    public static Rect HabitableMapSize = new Rect(-200, -98f, 400, 196f);

    //Visible parts of the map
    //public static readonly Vector3 Min = new Vector3(-246.6667f, -120);
    //public static readonly Vector3 Max = new Vector3(246.6667f, 120);
    public static readonly Vector3 Min = new Vector3(-208.7f, -120);
    public static readonly Vector3 Max = new Vector3(208.7f, 120);

    //counts from the center of coordinates system
    //public static readonly Vector2 MapSize = new Vector2(30, 20);
    public static readonly Vector2 ScrollableMapSize = new Vector2(28, 18); // NOT ACTUALLY USED

    // Use this for initialization
    void Start()
    {
        //if (ScrollableMap)
        //    HabitableMapSize = new Rect(-20, -9.8f, 40, 19.6f);
        //else
        //    HabitableMapSize = new Rect(-20, -9.8f, 40, 19.6f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public CastleSc[] GetAllCities()
    {
        var castlesObjects = GameObject.FindGameObjectsWithTag("Castles");

        var result = new List<CastleSc>();


        foreach (var castleObject in castlesObjects)
        {
            result.Add(castleObject.GetComponent<CastleSc>());
        }

        return result.ToArray();
    }
}
