using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.U2D;

public class LakeSc : MonoBehaviour
{
    private PolygonCollider2D _collider;

    private Spline _spline;
    private SpriteShapeController _shapeController;
    private List<PolygonCollider2D> _otherLakes;
    private List<int> _activePoints;

    public float FloodSpeed;
    public int Alpha;


    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetColor(Color newColor)
    {
        _shapeController.spriteShapeRenderer.color = new Color(newColor.r, newColor.g, newColor.b, Alpha/ 255.0f);
    }

    bool PointInsideOtherLakes(Vector2 point, List<PolygonCollider2D> otherLakes)
    {
        foreach (var polygonCollider2D in otherLakes)
        {
            if (polygonCollider2D.OverlapPoint(point))
                return true;

        }

        return false;
    }

    public bool PointInsideCollider(Vector2 point)
    {
        var otherLakes = GameObject.FindGameObjectsWithTag(GameTags.Lakes)
            .Where(go => go != gameObject)
            .Select(go => go.GetComponent<PolygonCollider2D>())
            .ToList();

        return PointInsideOtherLakes(point, otherLakes);
    }

    public void InitExpanding()
    {
        _shapeController = GetComponent<SpriteShapeController>();
        _spline = _shapeController.spline;
        _activePoints = Enumerable.Range(0, _spline.GetPointCount()).ToList();

        _otherLakes = GameObject.FindGameObjectsWithTag(GameTags.Lakes)
            .Where(go => go != gameObject)
            .Select(go => go.GetComponent<PolygonCollider2D>())
            .ToList();

        var castle = GetComponentInParent<CastleSc>();
        var owner = castle.GetOwner();

        SetColor(owner.GetPlayerColor());
    }

    public void UpdateExpanding()
    {
        var pointsToRemove = new List<int>();
        foreach (var point in _activePoints)
        {
            var oldPosition = _spline.GetPosition(point);

            var pointInGlobalCoordinates = transform.TransformPoint(oldPosition);
            if (PointInsideOtherLakes(pointInGlobalCoordinates, _otherLakes))
            {
                pointsToRemove.Add(point);
                continue;
            }

            var change = oldPosition * FloodSpeed;
            _spline.SetPosition(point, oldPosition + change);

        }

        _activePoints.RemoveAll(pt => pointsToRemove.Contains(pt));

        //for (int i = 0; i < _spline.GetPointCount(); i++)
        //{
        //    var oldPosition = _spline.GetPosition(i);

        //    var pointInGlobalCoordinates = transform.TransformPoint(oldPosition);
        //    if (PointInsideOtherLakes(pointInGlobalCoordinates, _otherLakes))
        //    {
        //        _stoppedPoints.Add(i);
        //        continue;
        //    }

        //    var change = oldPosition * FloodSpeed;
        //    _spline.SetPosition(i, oldPosition + change);
        //}

        //_shapeController.BakeCollider();
        //_shapeController.RefreshSpriteShape();
    }
}
