using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interfaces;
using Assets.Scripts.MapObjects.Castles.Borders;
using Assets.Scripts.MapObjects.Voronoi;
using Assets.Scripts.Players;
using UnityEngine;
using UnityEngine.U2D;

public class VoronoiLakeSc : MonoBehaviour, IHaveOwner
{
    public List<(Vector3, Vector3)> Border = new List<(Vector3, Vector3)>();
    public int Alpha = 80;
    
    private List<VorBorder> _vorShape;
    private BasePlayer _currentOwner;

    SpriteShapeController _shapeController;
    SpriteShapeController ShapeController
    {
        get { _shapeController ??= gameObject.GetComponent<SpriteShapeController>(); return _shapeController; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = GetOwner().GetPlayerColor();
        //  Gizmos.DrawLine(new Vector3(1, 1), new Vector3(2, 2));

        foreach (var line in Border)
        {
            Gizmos.DrawLine(line.Item1, line.Item2);
        }
    }

    public void SetOwner(BasePlayer newOwner)
    {
        _currentOwner = newOwner;
        SetColor(newOwner.GetPlayerColor());
    }

    public void SetColor(Color newColor)
    {
        ShapeController.spriteShapeRenderer.color = new Color(newColor.r, newColor.g, newColor.b, Alpha / 255.0f);
    }

    public BasePlayer GetOwner()
    {
        return _currentOwner;
    }

    public void BuildBorders()
    {
        if (Border?.Any() != true)
            return;

        var points = Border.Select(line => line.Item1).ToList();
        points.AddRange(Border.Select(line => line.Item2));

        var builder = new VoronoiBorderBuilder();
        _vorShape = builder.BuildClosedShape(Border);
        
        var spline = ShapeController.spline;
        spline.Clear();


        for (int i = 0; i < _vorShape.Count; i++)
        {
            var currPoint = _vorShape[i].Line.Item1;
            var localCoord = transform.InverseTransformPoint(currPoint);

            spline.InsertPointAt(i, localCoord);
            // spline.SetHeight(i, 0.2f);
        }

        try
        {
            var lastCoord = transform.InverseTransformPoint(_vorShape.Last().Line.Item2);
            spline.InsertPointAt(_vorShape.Count, lastCoord);
            spline.SetHeight(_vorShape.Count, 0.2f);
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception at {e}");
        }
    }
}
