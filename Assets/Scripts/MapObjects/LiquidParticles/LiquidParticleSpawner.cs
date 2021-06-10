using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.MapObjects.LiquidParticles
{
    public class LiquidParticleSpawner : MonoBehaviour
    {
        public GameObject LiquidParticlePrefab;

        private GameObject _particlesFolder;
        //private GameObject[] _castles;
        private List<LakeSc> _lakes;

        public int FloodLength;

        // Start is called before the first frame update
        void Start()
        {
            _particlesFolder = GameObject.Find("LiquidParticles");
            _lakes = GameObject.FindGameObjectsWithTag(GameTags.Lakes)
                .Select(go => go.GetComponent<LakeSc>())
                .ToList();
            //FillCastlesWithFluids();
            FillCastlesLakes();
        }

        // Update is called once per frame
        void Update()
        {
            if (FloodLength <= 0)
                return;

            foreach (var lake in _lakes)
            {
                if (lake != null)
                {
                    lake.UpdateExpanding();
                }
            }

            FloodLength--;
        }

        void FillCastlesLakes()
        {
            foreach (var lake in _lakes)
            {
                if (lake != null)
                {
                    lake.InitExpanding();
                }
            }

            for (int i = 0; i < 100; i++)
            {
                foreach (var lake in _lakes)
                {
                    lake.UpdateExpanding();
                }
            }
        }

        //void FillCastlesWithFluids()
        //{
        //    for (int i = 0; i < 50; i++)
        //    {
        //        foreach (var castle in _castles)
        //        {
        //            var castleSc = castle.GetComponent<CastleSc>();

        //            var x = castle.transform.position.x + Random.Range(0, 0.1f);
        //            var y = castle.transform.position.y + Random.Range(0, 0.1f);
        //            CreateLiquidParticle(new Vector3(x, y, 0), castleSc.CastleNumber);
        //        }
        //    }
        //}

        public void CheckPoint(Vector2 point)
        {

            //foreach (var lake in lakes)
            //{
            //    if (lake.PointInsideCollider(point))
            //        return;
            //}

            for (int i = 0; i < 10; i++)
            {
                foreach (var lake in _lakes)
                {
                    lake.UpdateExpanding();
                }
            }
        }

        public void CreateLiquidParticle(Vector3 pos, int castleNum)
        {
            var particle = Instantiate(LiquidParticlePrefab, new Vector3(pos.x, pos.y, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            particle.GetComponent<LiquidSc>().SourceCastle = castleNum;
            particle.transform.SetParent(_particlesFolder.transform);
            //_castles[i] = Instantiate(_typesDict[castleType], pt, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
}
