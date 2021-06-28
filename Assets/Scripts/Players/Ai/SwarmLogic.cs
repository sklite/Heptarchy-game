using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Players.Ai
{
    class SwarmLogic : BaseBrain
    {

        public SwarmLogic(TheGameSc theGame)
            : base(theGame)
        {

        }

        // Расчитать вес для города (чем вес больше - тем выше шанс, что город будет атакован)
        private float CalculatePriorityToCity(CastleSc city)
        {

            float distance = 500 - MathCalculator.CalcDistance(playerCities[0].transform.position.x, city.transform.position.x,
                playerCities[0].transform.position.y, city.transform.position.y);



            return (distance);
        }

        public override Dictionary<CastleSc, float> CalculateCitiesToAttack()
        {

            citiesToAttack.Clear();
            foreach (var curCity in allCities)
            {
                if (curCity.GetComponent<CastleSc>().GetOwner() == owner)
                    continue;

                var priority = CalculatePriorityToCity(curCity);

                citiesToAttack.Add(curCity.GetComponent<CastleSc>(), priority);
            }

            return citiesToAttack;  //To change body of implemented methods use File | SettingsSc | File Templates.

        }


    }
}
