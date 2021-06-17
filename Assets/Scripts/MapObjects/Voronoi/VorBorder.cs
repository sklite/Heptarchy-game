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
        public int Castle1 { get; set; }
        public int Castle2 { get; set; }
        public (Vector3, Vector3) Line { get; set; }

        public VorBorder Next { get; set; }
        public VorBorder Previous { get; set; }
    }
}
