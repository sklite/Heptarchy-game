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
        private GameObject[] _castles;

        // Start is called before the first frame update
        void Start()
        {
            _particlesFolder = GameObject.Find("LiquidParticles");

            _castles = GameObject.FindGameObjectsWithTag(GameTags.Castles);
            FillCastlesWithFluids();
        }

        // Update is called once per frame
        void Update()
        {

        }


        void FillCastlesWithFluids()
        {
            for (int i = 0; i < 0; i++)
            {
                foreach (var castle in _castles)
                {
                    var castleSc = castle.GetComponent<CastleSc>();

                    var x = castle.transform.position.x + Random.Range(0, 0.1f);
                    var y = castle.transform.position.y + Random.Range(0, 0.1f);
                    CreateLiquidParticle(new Vector3(x, y, 0), castleSc.CastleNumber);
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
