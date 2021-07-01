using System.Collections.Generic;

namespace Assets.Scripts.Helpers.VoronoiGraph
{
    class VorRegion
    {
        public int CastleOwner { get; set; }
        public List<VorBorder> Borders { get; set; }
    }
}
