using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public abstract class BasePlayer
    {
        Color PlayerColor;

        ArmySpawnerSc armySpawner;


        public bool IsGaia;

        public List<GameObject> ControlledCities;
        public List<GameObject> SelectedCities;

        public List<GameObject> armies;
        private Color color;
        private MapSc gameMap;

        // может ли вообще ходить
        public bool canGo = false;

        public BasePlayer(Color color, MapSc gameMap)
        {
            ControlledCities = new List<GameObject>();
            SelectedCities = new List<GameObject>();
            armies = new List<GameObject>();
            this.PlayerColor = color;
            armySpawner = GameObject.Find("ArmySpawner").GetComponent<ArmySpawnerSc>();
        }

        /// <summary>
        /// Поздняя инициализация, вызывается когда уже карта заполнена замками
        /// </summary>
        public virtual void Init()
        {

        }

        public Color GetPlayerColor()
        {
            return PlayerColor;
        }

        public virtual void MakeMove()
        {

        }

        public void DeselectCity(GameObject city)
        {
            if (SelectedCities.Contains(city))
                SelectedCities.Remove(city);
        }

        public void LostCity(GameObject city)
        {
            DeselectCity(city);
            if (ControlledCities.Contains(city))
                ControlledCities.Remove(city);
        }

        internal void SendTroops(GameObject target)
        {
            //var script = target.GetComponent<CastleSc>();

            foreach (var item in SelectedCities)
            {
                if (target == item)
                    continue;
                var castleScript = item.GetComponent<CastleSc>();
                if (castleScript.CurrentPopulation < 2)
                    continue;
                var newArmyObject = armySpawner.CreateArmy(item, target);
                newArmyObject.GetComponent<ArmySc>().SetOwner(this);

            }
            SelectedCities.Clear();
        }
    }
}
