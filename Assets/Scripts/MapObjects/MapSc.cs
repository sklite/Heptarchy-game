using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSc : MonoBehaviour
{
    List<CastleSc> mapCastles;


    public static readonly Vector2 MapSize = new Vector2(21, 14);
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
