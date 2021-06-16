using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.MapObjects.Voronoi;
using UnityEngine;
using Random = UnityEngine.Random;

public class VoronoiSc2 : MonoBehaviour
{
    //private Vector2Int _imageDim = new Vector2Int(Screen.width, Screen.height);
    Vector2Int _imageDim = new Vector2Int(Screen.width, Screen.height);

    private int _regionAmount;
    private List<(Vector3, Vector3)> lines = new List<(Vector3, Vector3)>();
    private Dictionary<int, VorBorder> _borders = new Dictionary<int, VorBorder>();

    private Voronoi _voroObject = new Voronoi(0.1f);
    private void Start()
    {
       SpreadPoints();
    }

	// Update is called once per frame
	void Update()
    {
        
    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //  //  Gizmos.DrawLine(new Vector3(1, 1), new Vector3(2, 2));

    //    foreach (var line in lines)
    //    {
    //        Gizmos.DrawLine(line.Item1, line.Item2);
    //    }
    //}

    

    void SpreadPoints()
    {
        var castles = GameObject.FindGameObjectsWithTag(GameTags.Castles);
        _regionAmount = castles.Length;

        var sites = castles.Select(cas => Camera.main.WorldToScreenPoint(cas.transform.position)).ToList();

        //List<Vector3> sites = points;// new List<Vector2>();
      //  int seed = seeder.Next();

        //for (int i = 0; i < _regionAmount; i++)
        //{
        //    sites.Add(new Vector2(Random.Range(0, _imageDim.x - 1), Random.Range(0, _imageDim.y - 1)));
        //}
        

        //for (int i = 0; i < sites.Count; i++)
        //{
        //    g.FillEllipse(Brushes.Blue, sites[i].X - 1.5f, sites[i].Y - 1.5f, 3, 3);
        //}

        List<GraphEdge> ge;
        ge = MakeVoronoiGraph(sites, _imageDim.x, _imageDim.y);
        
        var castlesSc = castles.Select(cas => cas.GetComponent<CastleSc>()).ToList();

      //  _borders = castleNumbers.ToDictionary(k => k, i => new VorBorder());

        for (var i = 0; i < ge.Count; i++)
        {
            try
            {
                var p1 = new Vector3(ge[i].x1, ge[i].y1);
                var p2 = new Vector3(ge[i].x2, ge[i].y2); 
                //print($"New line from: {p1} to {p2}");
                var p1World = Camera.main.ScreenToWorldPoint(p1);
                var p2World = Camera.main.ScreenToWorldPoint(p2);
                print($"Or like from: {p1World} to {p2World}");

                castlesSc[ge[i].site1].Border.Add((p1World, p2World));
                castlesSc[ge[i].site2].Border.Add((p1World, p2World));
                

                //foreach (var castle in castles)
                //{

                //    if (ge[i].site2)
                //}

                lines.Add((p1World, p2World));
               // g.DrawLine(Pens.Black, p1.X, p1.Y, p2.X, p2.Y);
            }
            catch
            {
                string s = "\nP " + i + ": " + ge[i].x1 + ", " + ge[i].y1 + " || " + ge[i].x2 + ", " + ge[i].y2;
            }
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
