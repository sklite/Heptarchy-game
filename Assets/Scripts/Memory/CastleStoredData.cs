using Assets.Scripts.MapObjects.Castles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Memory
{
    public class CastleStoredData
    {
        public List<CastleInfo> CastleInfos
        {
            get;
            set;
        }

        public CastleStoredData()
        {
            CastleInfos = new List<CastleInfo>();
        }

    }
}
