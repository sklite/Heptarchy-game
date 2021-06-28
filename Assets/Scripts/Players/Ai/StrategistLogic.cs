using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players.Ai
{
    class StrategistLogic : BaseBrain
    {
        public StrategistLogic(TheGameSc theGame)
            : base(theGame)
        {

        }

        // Расчитать вес для города (чем вес больше - тем выше шанс, что город будет атакован)
        private int CalculatePriorityToCity(CastleSc city)
        {

            // расстояние между городами обычно в пикселях
            Vector2 center = FindCenter();


            float koeff = 100 - MathCalculator.CalcDistance(center.x, city.transform.position.x,
                center.y, city.transform.position.y);
            //float armies = calcArmiesSumm();

            koeff -= city.CurrentPopulation * 0.2f;
            koeff += city.BasePopulation;

            //  koeff += city.GetOwner().IsGaia ? 0 : 0.22f;



            return (int)koeff;
        }

        private float CalcArmiesSumm()
        {

            float summ = 0;
            foreach (var city in playerCities)
            {
                summ += city.GetComponent<CastleSc>().CurrentPopulation;
            }

            return summ / 10;

        }

        private Vector2 FindCenter()
        {
            float centerX = 0;
            float centerY = 0;

            foreach (var city in playerCities)
            {
                centerX += city.transform.position.x;
                centerY += city.transform.position.y;
            }

            centerX /= playerCities.Count;
            centerY /= playerCities.Count;

            return new Vector2(centerX, centerY);
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
