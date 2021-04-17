using Assets.Scripts.MapObjects.Castles;
using System.Collections.Generic;

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
