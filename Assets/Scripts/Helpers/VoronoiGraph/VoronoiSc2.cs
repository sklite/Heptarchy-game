using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Helpers.VoronoiGraph
{
    public class VoronoiSc2 : MonoBehaviour
    {
        Vector2Int _imageDim = new Vector2Int(Screen.width, Screen.height);

        private int _regionAmount;
        private List<(Vector3, Vector3)> lines = new List<(Vector3, Vector3)>();

        private Voronoi _voroObject = new Voronoi(0.1f);
        private void Start()
        {
            SpreadPoints();
        }

        void SpreadPoints()
        {
            var castles = GameObject.FindGameObjectsWithTag(GameTags.Castles);
            _regionAmount = castles.Length;

            var sites = castles.Select(cas => Camera.main.WorldToScreenPoint(cas.transform.position)).ToList();

            List<GraphEdge> ge = MakeVoronoiGraph(sites, _imageDim.x, _imageDim.y);
            var castlesSc = castles.Select(cas => cas.GetComponent<CastleSc>()).ToList();

            for (var i = 0; i < ge.Count; i++)
            {
                try
                {
                    var p1 = new Vector3(ge[i].x1, ge[i].y1);
                    var p2 = new Vector3(ge[i].x2, ge[i].y2); 
                    var p1World = Camera.main.ScreenToWorldPoint(p1);
                    var p2World = Camera.main.ScreenToWorldPoint(p2);

                    castlesSc[ge[i].site1].AddBorderLine((p1World, p2World), castlesSc[ge[i].site2]);
                    castlesSc[ge[i].site2].AddBorderLine((p1World, p2World), castlesSc[ge[i].site1]);

                    //print($"Castle {castlesSc[ge[i].site1].CastleNumber} has point {p1World} to {p2World}");
                    //print($"Castle {castlesSc[ge[i].site2].CastleNumber} has point {p1World} to {p2World}");

                    lines.Add((p1World, p2World));
                }
                catch
                {
                    string s = "\nP " + i + ": " + ge[i].x1 + ", " + ge[i].y1 + " || " + ge[i].x2 + ", " + ge[i].y2;
                }
            }

            foreach (var castle in castlesSc)
            {
                castle.BuildBorders();
            }
        }

        List<GraphEdge> MakeVoronoiGraph(List<Vector3> sites, int width, int height)
        {
            float[] xVal = new float[sites.Count];
            float[] yVal = new float[sites.Count];
            for (int i = 0; i < sites.Count; i++)
            {
                xVal[i] = sites[i].x;
                yVal[i] = sites[i].y;
            }
            return _voroObject.generateVoronoi(xVal, yVal, 0, width, 0, height);

        }
    }
}
