using Assets.Scripts.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MapObjects.Castles
{
    public class CastleInfo
    {
        public float growthRate;

        public bool selected;

        public float CurrentPopulation;

        public float BasePopulation;

        public CastleSize Type;

        /// <summary>
        /// Первоначальный владелец замка
        /// </summary>
        public BasePlayer Owner;

        public Color CastleColor;

        public Color CurrentColor;

        public Transform Position;
    }
}
