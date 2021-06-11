using System;
using Assets.Scripts.MapObjects.Castles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.MapObjects.LiquidParticles;
using Assets.Scripts.Memory;
using UnityEngine;
using Random = UnityEngine.Random;

public class CastleSpawnerSc : MonoBehaviour
{
	public int castlesCount;

	public GameObject[] castlesPrefaps;
	public float[] castlesFrequences;
	public float[] castlesSizes;
    public float OverallSizeScale;


	GameObject _widthLabel;
	TheGameSc _gameScript;
    List<GameObject> _castles;
 


    private Dictionary<CastleType, GameObject> _typesDict = new Dictionary<CastleType, GameObject>();
    private Dictionary<CastleType, float> _typePopulationDict = new Dictionary<CastleType, float>();

	void Start()
    {
        if (castlesPrefaps.Length != castlesFrequences.Length)
            throw new Exception("You should provide all castles and their frequencies");

        if (castlesPrefaps.Length != castlesSizes.Length)
            throw new Exception("You should provide all castles and their sizes!");

        

        for (var i = 0; i < castlesPrefaps.Length; i++)
        {
            var castleType = castlesPrefaps[i].GetComponent<CastleSc>().CastleType;

            _typesDict[castleType] = castlesPrefaps[i];
            _typePopulationDict[castleType] = castlesSizes[i];
        }

        //_map = GameObject.Find("Map").GetComponent<MapSc>();

        _castles = new List<GameObject>();
		var theGame = GameObject.Find("TheGame");
		_gameScript = theGame.GetComponent<TheGameSc>();

		GenerateCitiesPositions();
		AssignCitiesToPlayers();
        InitPlayers();
        //TurnCastlesColliders(false);
        
	}

	private void InitPlayers()
	{
		foreach (var player in _gameScript.GetAllPlayers())
		{
			player.Init();
		}
	}


	private void GenerateCitiesPositions()
	{
		SettingsSc.Stored.CastleInfos.Clear();

		var gaiaPlayer = _gameScript.GetGaia();
		for (int i = 0; i < castlesCount; i++)
        {
            var castleType = GetNewCastleType();
            var populationFactor = _typePopulationDict[castleType];

			float population = populationFactor + Random.value * 0.7f;

            if (!GetFreePoint(population * OverallSizeScale * 1.5f, out Vector3 pt))
				continue;

            var newCastle = Instantiate(_typesDict[castleType], pt, Quaternion.Euler(new Vector3(0, 0, 0)));
            newCastle.transform.localScale = new Vector3(population * OverallSizeScale, population * OverallSizeScale, 0);

			var castleScript = newCastle.GetComponent<CastleSc>();
			castleScript.CastleType = castleType;
			castleScript.SetBaseOwner(gaiaPlayer);
			castleScript.SetBasePopulation(population * 20);
			castleScript.UpdateTransformInfo(castleScript.transform);
            castleScript.CastleNumber = i;

			SettingsSc.Stored.CastleInfos.Add(castleScript.GetInfo());

            _castles.Add(newCastle);
		}
	}


    CastleType GetNewCastleType()
    {
        var chancesSum = castlesFrequences.Sum();

        var randomNum = Random.Range(0, chancesSum);
        var summ = 0.0f;

        for (int i = 0; i < _typesDict.Count; i++)
        {
            summ += castlesFrequences[i];
            if (randomNum <= summ)
                return _typesDict.ElementAt(i).Key;
        }


        return CastleType.Village1;
    }

	void AssignCitiesToPlayers()
	{
		var nonGaiaPlayers = _gameScript.GetNonGaiaPlayers();
        if (_castles.Count < nonGaiaPlayers.Length)
            return;
        for (int i = 0; i < nonGaiaPlayers.Length; i++)
        {
            var castleScript = _castles[i].GetComponent<CastleSc>();
            castleScript.SetBaseOwner(nonGaiaPlayers[i]);

        }

        //for (int i = 0; i < _castles.Length; i++)
        //{
        //    var castleScript = _castles[i].GetComponent<CastleSc>();
        //    castleScript.SetBaseOwner(nonGaiaPlayers[0]);
        //}
	}

    void TurnCastlesColliders(bool state)
    {
        foreach (var castle in _castles)
        {
            castle.GetComponent<BoxCollider2D>().enabled = state;
        }
    }

	bool GetFreePoint(float citySize, out Vector3 resultPt)
	{
		int maxAttemptCount = 100;
		resultPt = new Vector3(1, 1, 0);
		while (maxAttemptCount > 0)
		{
			maxAttemptCount--;

            var xCoord = Random.Range(MapSc.HabitableMapSize.xMin, MapSc.HabitableMapSize.xMax);
            var yCoord = Random.Range(MapSc.HabitableMapSize.yMin, MapSc.HabitableMapSize.yMax);

			resultPt = new Vector3(xCoord, yCoord, 0);
			if (CanPlaceHere(resultPt, citySize))
			{
              //  return new Vector3(25, 16, 0);
				return true;
			}
		}
		return false;
	}
    
	bool CanPlaceHere(Vector3 pt, float size)
	{
        foreach (var castle in _castles)
        {
            if (castle == null)
                continue;

            var curCastleCollider = castle.GetComponent<BoxCollider2D>();
            Bounds newBounds = new Bounds(pt, new Vector3(size, size, 0));
            if (curCastleCollider.bounds.Intersects(newBounds))
            {
                return false;
            }
        }

		return true;
    }

	// Update is called once per frame
	void Update()
	{


	}

    public void CleanMap(CastleStoredData castleStoredData)
    {

        for (int i = 0; i < _gameScript.GetNonGaiaPlayers().Length; i++)
        {
            var player = _gameScript.GetNonGaiaPlayers()[i];
            player.ControlledCities.Clear();
            player.SelectedCities.Clear();
        }

        //Чистим замки
        for (int i = 0; i < castleStoredData.CastleInfos.Count; i++)
        {
            var curCastle = _castles[i];
            if (curCastle == null)
                continue;
            var castleScript = curCastle.GetComponent<CastleSc>();

            var castleData = castleStoredData.CastleInfos[i];
            castleScript.SetOwner(castleData.Owner);
            castleScript.SetBasePopulation(castleData.BasePopulation);
        }

        var armySpawner = GameObject.Find("ArmySpawner").GetComponent<ArmySpawnerSc>();
        var armies = armySpawner.GetArmies();
        if (armies == null || armies.Length == 0)
            return;
        for (int i = 0; i < armies.Length; i++)
        {
            var armyObj = armies[i];
            Destroy(armyObj);
        }


        SettingsSc.RegenerateMap = true;
    }
}
