using UnityEngine;

namespace Assets.Scripts.Helpers.VoronoiGraph
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
