using Assets.Menu.Scripts;
using Assets.Scripts.Players;
using Assets.Scripts.Players.Ai;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TheGameSc : MonoBehaviour
{
    BasePlayer[] _players;
    MapSc _gameMap;


    int _counter;
    // Use this for initialization
    void Start()
    {
        _gameMap = GameObject.Find("Map").GetComponent<MapSc>();
        SettingsSc.SetPause(false);


        var playerInfos = GameInfo.Players;
        if (playerInfos != null)
        {
            var playerList = new List<BasePlayer>();
            playerList.Add(new HumanPlayer(Color.blue));
            playerList.Add(new AiPlayer(Color.white) { IsGaia = true });
            (playerList[1] as AiPlayer).SetBrains(new SwarmLogic(this), AiEnumeration.NoAi);

            foreach (var playerInfo in playerInfos)
            {
                var newAiPlayer = new AiPlayer(playerInfo.Color);

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
            _players = playerList.ToArray();
        }
        else
        {
            _players = new BasePlayer[]
            {
                new HumanPlayer(Color.blue),
                new AiPlayer(Color.white)  {IsGaia = true},
                new AiPlayer(Color.red),
                new AiPlayer(Color.green),
                new AiPlayer(Color.yellow)
            };

            (_players[1] as AiPlayer).SetBrains(new SwarmLogic(this), AiEnumeration.NoAi);
            (_players[2] as AiPlayer).SetBrains(new StrategistLogic(this), AiEnumeration.Normal);
            (_players[3] as AiPlayer).SetBrains(new StrategistLogic(this), AiEnumeration.Normal);
            (_players[4] as AiPlayer).SetBrains(new StrategistLogic(this), AiEnumeration.Normal);
        }

    }

    // Update is called once per frame
    void Update()
    {

        _counter++;
        if (_counter % 180 != 0)
        {

            if (_counter == int.MaxValue)
                _counter = 0;
            return;
        }

        foreach (var player in _players)
        {
            if (player is HumanPlayer)
                continue;
            player.MakeMove();

            if (player.ControlledCities.Count == _gameMap.GetAllCities().Length)
            {

            }

        }

    }



    public HumanPlayer GetHumanPlayer()
    {
        return _players[0] as HumanPlayer;
    }

    public AiPlayer GetGaia()
    {
        return _players[1] as AiPlayer;
    }

    public BasePlayer[] GetNonGaiaPlayers()
    {
        return _players.Where(player => !player.IsGaia).ToArray();
    }

    public BasePlayer[] GetAllPlayers()
    {
        return _players;
    }


    internal MapSc GetMap()
    {
        return _gameMap;
    }
}
