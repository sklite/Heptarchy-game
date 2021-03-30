using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players.Ai
{
    public abstract class BaseBrain
    {
        protected Dictionary<CastleSc, float> citiesToAttack;

        protected MapSc map;
        protected TheGameSc game;

        protected CastleSc[] allCities;
        protected List<GameObject> playerCities;


        protected AiPlayer owner;

        public BaseBrain(TheGameSc game)
        {
            citiesToAttack = new Dictionary<CastleSc, float>();
            map = game.GetMap();
        }

        public void Init()
        {
            allCities = map.GetAllCities();
        }

        public void SetOwner(AiPlayer newOwner)
        {
            playerCities = newOwner.ControlledCities;
            owner = newOwner;
        }

        public abstract Dictionary<CastleSc, float> CalculateCitiesToAttack();
    }
}
