using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSc : MonoBehaviour
{
    public static readonly Vector2 MapSize = new Vector2(30, 20);
    public static readonly Rect HabitableMapSize = new Rect(-25, -17, 50, 34);

    // Use this for initialization
    void Start()
    {

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
