using Assets.Scripts.Players.Ai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class AiPlayer : BasePlayer
    {
        Dictionary<CastleSc, float> citiesToAttack;
        BaseBrain brains;

        public AiPlayer(Color color, MapSc gameMap)
            : base(color, gameMap)
        {
            citiesToAttack = new Dictionary<CastleSc, float>();
        }

        public override void MakeMove()
        {

            if (!canGo)
                return;

            if (!ControlledCities.Any())
                return;

            SelectedCities.AddRange(ControlledCities);

            CalculateAttackPriority();

            AttackIfRequired();
        }


        public void SetBrains(BaseBrain newBrains, AiEnumeration aiEnumeration)
        {
            this.brains = newBrains;
            brains.SetOwner(this);
            if (aiEnumeration != AiEnumeration.NoAi)
                canGo = true;

        }

        public override void Init()
        {
            brains.Init();
        }

        private void AttackIfRequired()
        {
            CastleSc cityToAttack = null;
            float biggestAttackValue = 0;

            foreach (var curCity in citiesToAttack)
            {
                if (curCity.Value > biggestAttackValue)
                {
                    biggestAttackValue = curCity.Value;
                    cityToAttack = curCity.Key;
                }
            }



            if (cityToAttack != null)
            {
                int countArmy = -5;

                for (int i = 0; i < SelectedCities.Count; i++)
                {
                    countArmy += (int)(SelectedCities[i].GetComponent<CastleSc>().CurrentPopulation / SettingsSc.ConscriptKoeff);
                    if (countArmy > cityToAttack.CurrentPopulation)
                    {
                        SendTroops(cityToAttack.gameObject);
                        break;
                    }
                }


            }
        }

        private void CalculateAttackPriority()
        {
            citiesToAttack = brains.CalculateCitiesToAttack();
        }


    }
}
