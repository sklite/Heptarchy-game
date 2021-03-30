using Assets.Menu.Scripts;
using Assets.Scripts.Players;
using Assets.Scripts.Players.Ai;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TheGameSc : MonoBehaviour
{
    BasePlayer[] players;
    MapSc gameMap;


    int counter;
    // Use this for initialization
    void Start()
    {


        gameMap = GameObject.Find("GameMap").GetComponent<MapSc>();
        SettingsSc.SetPause(false);


        var playerInfos = GameInfo.Players;
        if (playerInfos != null)
        {
            var playerList = new List<BasePlayer>();
            playerList.Add(new HumanPlayer(Color.blue, gameMap));
            playerList.Add(new AiPlayer(Color.white, gameMap) { IsGaia = true });
            (playerList[1] as AiPlayer).SetBrains(new SwarmLogic(this), AiEnumeration.NoAi);

            foreach (var playerInfo in playerInfos)
            {
                var newAiPlayer = new AiPlayer(playerInfo.Color, gameMap);

                BaseBrain logic = null;

                if (playerInfo.AiSkill == AiEnumeration.Easy)
                    logic = new SwarmLogic(this);
                else if (playerInfo.AiSkill == AiEnumeration.Normal)
                    logic = new StrategistLogic(this);

                if (logic == null)
                    continue;
                newAiPlayer.SetBrains(logic, playerInfo.AiSkill);
                playerList.Add(newAiPlayer);
            }
            players = playerList.ToArray();
        }
        else
        {
            players = new BasePlayer[]
            {
                new HumanPlayer(Color.blue, gameMap),
                new AiPlayer(Color.white, gameMap)  {IsGaia = true},
                new AiPlayer(Color.red, gameMap),
                new AiPlayer(Color.green, gameMap),
                new AiPlayer(Color.yellow, gameMap)
            };

            (players[1] as AiPlayer).SetBrains(new SwarmLogic(this), AiEnumeration.NoAi);
            (players[2] as AiPlayer).SetBrains(new SwarmLogic(this), AiEnumeration.Easy);
            (players[3] as AiPlayer).SetBrains(new StrategistLogic(this), AiEnumeration.Normal);
            (players[4] as AiPlayer).SetBrains(new StrategistLogic(this), AiEnumeration.Normal);
        }

    }

    // Update is called once per frame
    void Update()
    {

        counter++;
        if (counter % 120 != 0)
        {
            if (counter > int.MaxValue - 10000)
                counter = 0;
            return;
        }

        foreach (var player in players)
        {
            if (player is HumanPlayer)
                continue;
            player.MakeMove();

            if (player.ControlledCities.Count == gameMap.GetAllCities().Length)
            {

            }

        }

    }



    public HumanPlayer GetHumanPlayer()
    {
        return players[0] as HumanPlayer;
    }

    public AiPlayer GetGaia()
    {
        return players[1] as AiPlayer;
    }

    public BasePlayer[] GetNonGaiaPlayers()
    {
        return players.Where(player => !player.IsGaia).ToArray();
    }

    public BasePlayer[] GetAllPlayers()
    {
        return players;
    }


    internal MapSc GetMap()
    {
        return gameMap;
    }
}
