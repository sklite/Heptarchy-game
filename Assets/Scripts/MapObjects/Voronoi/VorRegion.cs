using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.MapObjects.Voronoi
{
    class VorRegion
    {
        public int CastleOwner { get; set; }
        public List<VorBorder> Borders { get; set; }
    }
}
