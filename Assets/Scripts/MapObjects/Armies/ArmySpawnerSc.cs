using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmySpawnerSc : MonoBehaviour
{
    public float ArmySpeed;

    public float StartPositionOffset = 0.5f;
    public float MultipleArmiesSpawnDelaySeconds = 0.5f;
    public GameObject armyPrefab;
    public GameObject armyDotPrefab;

    public int BaseArmySize = 10;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public IEnumerator DoCreateArmy(GameObject sourceCastle, GameObject destination)
    {
        var sourceSc = sourceCastle.GetComponent<CastleSc>();
        while (sourceSc.CurrentPopulation > BaseArmySize)
        {
            var army = IssueTroops(sourceCastle);
            army.Destination = destination;
            yield return new WaitForSeconds(MultipleArmiesSpawnDelaySeconds);
        }
    }

    ArmyParticleSc IssueTroops(GameObject sourceCastle)
    {
        var castleScript = sourceCastle.GetComponent<CastleSc>();
        castleScript.CurrentPopulation -= BaseArmySize;

        var position = sourceCastle.transform.position;
        position.y -= StartPositionOffset;

        var newArmy = Instantiate(armyDotPrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)));

        var armyScript = newArmy.GetComponent<ArmyParticleSc>();
        armyScript.SetBaseSpeed(ArmySpeed);
        armyScript.Amount = BaseArmySize;
        armyScript.SetOwner(castleScript.GetOwner());

        return armyScript;
    }

    public void CreateArmy(GameObject sourceCastle, GameObject destination)
    {
        StartCoroutine(DoCreateArmy(sourceCastle, destination));
    }
}
