using Assets.Scripts.MapObjects.Castles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleSpawnerSc : MonoBehaviour
{
	public int castlesCount;
	public GameObject[] castlesPrefaps;
	GameObject widthLabel;
	TheGameSc gameScript;
	GameObject[] castles;
	//	SettingsSc gameSettings;
	// Use this for initialization
	float villageSize = 0.5f;
	float barrackSize = 0.8f;
	float castleSize = 1.2f;

	Dictionary<CastleSize, GameObject> typesDict;

	void Start()
	{
		typesDict = new Dictionary<CastleSize, GameObject>{
			{CastleSize.Village1, castlesPrefaps[3]},
			{CastleSize.Village2, castlesPrefaps[2]},
			{CastleSize.Barracks, castlesPrefaps[1]},
			{CastleSize.CastleSc, castlesPrefaps[0]}
		};

		villageSize = 1.1f;
		barrackSize = 1.3f;
		castleSize = 1.4f;
		var gameSettings = SettingsSc.FindMe();
		castles = new GameObject[castlesCount];
		var theGame = GameObject.Find("TheGame");
		gameScript = theGame.GetComponent<TheGameSc>();

		GenerateCitiesPositions();
		AssignCitiesToPlayers();

		InitPlayers();

	}

	private void InitPlayers()
	{
		foreach (var player in gameScript.GetAllPlayers())
		{
			player.Init();
		}
	}


	private void GenerateCitiesPositions()
	{

		//if (SettingsSc.Stored != null && SettingsSc.Stored.CastleInfos != null)
		SettingsSc.Stored.CastleInfos.Clear();
		var gaiaPlayer = gameScript.GetGaia();
		Vector3 pt = new Vector3();
		for (int i = 0; i < castlesCount; i++)
		{

			float population = 0.8f + UnityEngine.Random.value * 0.7f;
			pt = getFreePoint(population);

			if (pt.y == -777)
				continue;

			var castleType = UnityEngine.Random.value > 0.5 ? CastleSize.Village1 : CastleSize.Village2;
			if (population > castleSize)
				castleType = CastleSize.CastleSc;
			else if (population > barrackSize)
				castleType = CastleSize.Barracks;

			castles[i] = Instantiate(typesDict[castleType], pt, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;

			castles[i].transform.localScale = new Vector3(population, population, 0);

			var castleScript = castles[i].GetComponent<CastleSc>();
			castleScript.CastleType = castleType;
			castleScript.SetBaseOwner(gaiaPlayer);
			castleScript.SetBasePopulation(population * 20);
			castleScript.UpdateTransformInfo(castleScript.transform);

			SettingsSc.Stored.CastleInfos.Add(castleScript.GetInfo());

		}
	}



	private void AssignCitiesToPlayers()
	{
		var nonGaiaPlayers = gameScript.GetNonGaiaPlayers();
		if (castles.Length < nonGaiaPlayers.Length)
			return;
		for (int i = 0; i < nonGaiaPlayers.Length; i++)
		{
			var castleScript = castles[i].GetComponent<CastleSc>();
			castleScript.SetBaseOwner(nonGaiaPlayers[i]);

		}
	}

	private Vector3 getFreePoint(float citySize)
	{

		int maxAttemptCount = 100;
		Vector3 pt = new Vector3(1, 1, 0);
		while (maxAttemptCount > 0)
		{
			maxAttemptCount--;
			float xCoordinate = UnityEngine.Random.value * SettingsSc.ScreenWidth / 2.0f - citySize / 2;
			float yCoordinate = UnityEngine.Random.value * SettingsSc.ScreenHeight / 2.0f - citySize / 2;
			if (UnityEngine.Random.value < 0.5)
				xCoordinate *= -1;
			if (UnityEngine.Random.value < 0.5)
				yCoordinate *= -1;
			pt = new Vector3(xCoordinate, yCoordinate, 0);
			if (CanPlaceHere(pt, citySize))
			{
				return pt;
			}
		}
		pt.y = -777; ;
		return pt;

	}


	bool CanPlaceHere(Vector3 pt, float size)
	{

		for (int i = 0; i < castlesCount; i++)
		{
			var curCastle = castles[i];
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
