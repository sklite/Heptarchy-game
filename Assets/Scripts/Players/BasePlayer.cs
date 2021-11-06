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
        Color _playerColor;

        ArmySpawnerSc _armySpawner;


        public bool IsGaia;

        public List<GameObject> ControlledCities;
        public List<GameObject> SelectedCities;

        public List<GameObject> armies;
        private Color _color;
        private MapSc _gameMap;

        // может ли вообще ходить
        public bool canGo = false;

        public BasePlayer(Color color)
        {
            ControlledCities = new List<GameObject>();
            SelectedCities = new List<GameObject>();
            armies = new List<GameObject>();
            _playerColor = color;
            _armySpawner = GameObject.Find("ArmySpawner").GetComponent<ArmySpawnerSc>();
        }

        /// <summary>
        /// Поздняя инициализация, вызывается когда уже карта заполнена замками
        /// </summary>
        public virtual void Init()
        {

        }

        public Color GetPlayerColor()
        {
            return _playerColor;
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
            foreach (var item in SelectedCities)
            {
                if (item == target)
                    continue;
                
                _armySpawner.CreateArmy(item, target);

            }
            SelectedCities.Clear();
        }
    }
}
