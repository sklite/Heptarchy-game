using Assets.Scripts.MapObjects.Castles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleSpawnerSc : MonoBehaviour
{
	public int castlesCount;
	public GameObject[] castlesPrefaps;
	public float SizeScale = 1.7f;
	GameObject _widthLabel;
	TheGameSc _gameScript;
	GameObject[] _castles;
    private MapSc _map;
	//	SettingsSc gameSettings;
	// Use this for initialization
	float _villageSize = 0.5f;
	float _barrackSize = 0.8f;
	float _castleSize = 1.2f;


	Dictionary<CastleSize, GameObject> _typesDict;

	void Start()
	{
		_typesDict = new Dictionary<CastleSize, GameObject>{
			{CastleSize.Village1, castlesPrefaps[3]},
			{CastleSize.Village2, castlesPrefaps[2]},
			{CastleSize.Barracks, castlesPrefaps[1]},
			{CastleSize.CastleSc, castlesPrefaps[0]}
		};

		_villageSize = 1.1f;
		_barrackSize = 1.3f;
		_castleSize = 1.4f;
        _map = GameObject.Find("Map").GetComponent<MapSc>();

		_castles = new GameObject[castlesCount];
		var theGame = GameObject.Find("TheGame");
		_gameScript = theGame.GetComponent<TheGameSc>();

		GenerateCitiesPositions();
		AssignCitiesToPlayers();

		InitPlayers();

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

			float population = 0.8f + Random.value * 0.7f;
			if (!GetFreePoint(population * SizeScale, out Vector3 pt))
				continue;

			var castleType = Random.value > 0.5 ? CastleSize.Village1 : CastleSize.Village2;
			if (population > _castleSize)
				castleType = CastleSize.CastleSc;
			else if (population > _barrackSize)
				castleType = CastleSize.Barracks;

			_castles[i] = Instantiate(_typesDict[castleType], pt, Quaternion.Euler(new Vector3(0, 0, 0)));

			_castles[i].transform.localScale = new Vector3(population * SizeScale, population * SizeScale, 0);

			var castleScript = _castles[i].GetComponent<CastleSc>();
			castleScript.CastleType = castleType;
			castleScript.SetBaseOwner(gaiaPlayer);
			castleScript.SetBasePopulation(population * 20);
			castleScript.UpdateTransformInfo(castleScript.transform);

			SettingsSc.Stored.CastleInfos.Add(castleScript.GetInfo());

		}
	}



	private void AssignCitiesToPlayers()
	{
		var nonGaiaPlayers = _gameScript.GetNonGaiaPlayers();
        if (_castles.Length < nonGaiaPlayers.Length)
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

	private bool GetFreePoint(float citySize, out Vector3 pt)
	{
		int maxAttemptCount = 100;
		pt = new Vector3(1, 1, 0);
		while (maxAttemptCount > 0)
		{
			maxAttemptCount--;

            var xCoord = Random.Range(MapSc.HabitableMapSize.xMin, MapSc.HabitableMapSize.xMax);
            var yCoord = Random.Range(MapSc.HabitableMapSize.yMin, MapSc.HabitableMapSize.yMax);

			pt = new Vector3(xCoord, yCoord, 0);
			if (CanPlaceHere(pt, citySize))
			{
              //  return new Vector3(25, 16, 0);
				return true;
			}
		}
		return false;
	}


	bool CanPlaceHere(Vector3 pt, float size)
	{

		for (int i = 0; i < castlesCount; i++)
		{
			var curCastle = _castles[i];
			if (curCastle == null)
				continue;

			var curCastleCollider = curCastle.GetComponent<BoxCollider2D>();
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

	//public void CleanMap(CastleStoredData castleStoredData)
	//{

	//	for (int i = 0; i < gameScript.GetNonGaiaPlayers().Length; i++)
	//	{
	//		var player = gameScript.GetNonGaiaPlayers()[i];
	//		player.ControlledCities.Clear();
	//		player.SelectedCities.Clear();
	//	}

	//	//Чистим замки
	//	for (int i = 0; i < castleStoredData.CastleInfos.Count; i++)
	//	{
	//		var curCastle = castles[i];
	//		if (curCastle == null)
	//			continue;
	//		var castleScript = curCastle.GetComponent<CastleSc>();

	//		var castleData = castleStoredData.CastleInfos[i];
	//		castleScript.SetOwner(castleData.Owner);
	//		castleScript.SetBasePopulation(castleData.BasePopulation);
	//	}

	//	var armySpawner = GameObject.Find("ArmySpawner").GetComponent<ArmySpawnerScript>();
	//	var armies = armySpawner.GetArmies();
	//	if (armies == null || armies.Length == 0)
	//		return;
	//	for (int i = 0; i < armies.Length; i++)
	//	{
	//		var armyObj = armies[i];
	//		Destroy(armyObj);
	//	}


	//	SettingsSc.RegenerateMap = true;
	//}
}
