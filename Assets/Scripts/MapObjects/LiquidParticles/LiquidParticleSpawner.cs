using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.MapObjects.LiquidParticles
{
    public class LiquidParticleSpawner : MonoBehaviour
    {
        public GameObject LiquidParticlePrefab;
        public int FloodLength;

        private GameObject _particlesFolder;
        //private GameObject[] _castles;
        private List<LakeSc> _lakes;
        private Stopwatch sw = new Stopwatch();
        public bool IsFilling = true;
        

        
        // Start is called before the first frame update
        void Start()
        {
            sw.Start();
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
            {
                if (IsFilling)
                {
                    IsFilling = false;

                   // _lakes

                    sw.Stop();
                    print($"Elapsed seconds: {sw.Elapsed.Seconds}");



                }

                return;
            }

            _lakes.ForEach(lake => lake.UpdateExpanding());

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
