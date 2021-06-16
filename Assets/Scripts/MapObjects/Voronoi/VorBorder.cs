using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MapObjects.Voronoi
{
    class VorBorder
    {
        public int CastleNum { get; set; }
        public List<(Vector3, Vector3)> Borders { get; set; }
    }
}
