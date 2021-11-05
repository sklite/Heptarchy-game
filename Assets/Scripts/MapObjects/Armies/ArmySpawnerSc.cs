using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmySpawnerSc : MonoBehaviour
{
    public float ArmySpeed = 0.025f;

    public float StartPositionOffset = 0.5f;
    public GameObject armyPrefab;
    public GameObject armyDotPrefab;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }


    public ArmyParticleSc CreateArmy(GameObject sourceCastle, GameObject destination)
    {
        var castleScript = sourceCastle.GetComponent<CastleSc>();

        var armySize = ((int)(castleScript.CurrentPopulation / SettingsSc.ConscriptKoeff));
        castleScript.CurrentPopulation -= armySize;
        //var newArmy = Instantiate(armyPrefab, sourceCastle.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));

        //var armyScript = newArmy.GetComponent<ArmySc>();
        //var speed = CalculateSpeed(sourceCastle.transform.position, destination.transform.position);

        //armyScript.SetOwner(castleScript.GetOwner());
        //armyScript.SetSpeed(speed);
        //armyScript.Amount = armySize;
        //armyScript.Destination = destination;
        var position = sourceCastle.transform.position;
        position.y -= StartPositionOffset;

        var newArmy = Instantiate(armyDotPrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)));

        var armyScript = newArmy.GetComponent<ArmyParticleSc>();
        var speed = CalculateSpeed(sourceCastle.transform.position, destination.transform.position);

        armyScript.SetOwner(castleScript.GetOwner());
        armyScript.SetSpeed(speed);
        armyScript.Amount = armySize;
        armyScript.Destination = destination;



        return armyScript;
    }

    public GameObject[] GetArmies()
    {
        return GameObject.FindGameObjectsWithTag("Armies");

    }

    Vector3 CalculateSpeed(Vector3 source, Vector3 destination)
    {
        float katetX = Mathf.Abs(source.x - destination.x);
        float katetY = Mathf.Abs(source.y - destination.y);
        float hypotenuza = (float)Mathf.Sqrt(katetX * katetX + katetY * katetY);

        float speed = ArmySpeed;
        var dx = speed * (katetX / hypotenuza);
        var dy = speed * (katetY / hypotenuza);

        if (source.y > destination.y)
            dy *= -1;

        if (source.x > destination.x)
            dx *= -1;

        return new Vector3(dx, dy);
    }
}
