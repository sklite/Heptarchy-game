using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Events;
using Assets.Scripts.Helpers;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Players;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;

public class CastleBorderBuilderSc : MonoBehaviour, IHaveOwner
{
    public GameObject BorderPrefab;

    private List<BorderLineSc> _borderLines = new List<BorderLineSc>();

    private GameObject _castle;

    private List<(Vector3, Vector3)> _vectors = new List<(Vector3, Vector3)>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rebuild()
    {
        foreach (var vector in _vectors)
        {
            AddBorderLine(vector, _castle, false);
        }
    }

    public void AddBorderLine((Vector3, Vector3) border, GameObject castle, bool pop = true)
    {
        _castle = castle;
        if (pop)
            _vectors.Add(border);

        if (MathCalculator.EqualVectors(border.Item1, border.Item2, MathCalculator.MinNodeDistance))
            return;

        var newBorder = Instantiate(BorderPrefab, castle.transform, true);// Quaternion.Euler(new Vector3(0, 0, 0)));
        newBorder.transform.SetParent(gameObject.transform);
        newBorder.transform.localPosition = Vector3.zero;
        newBorder.transform.localScale = Vector3.one;

        var spline = newBorder.GetComponent<SpriteShapeController>().spline;
        spline.Clear();

        var localA = transform.InverseTransformPoint(border.Item1);
        var localB = transform.InverseTransformPoint(border.Item2);

        var angleA = Math.Atan2(localA.y, localA.x);
        var angleB = Math.Atan2(localB.y, localB.x);

        angleA = MathCalculator.ToTrigonoimetricAngleDeg(angleA);
        angleB = MathCalculator.ToTrigonoimetricAngleDeg(angleB);

        //Make side to the center
        if (angleB > angleA && Math.Abs(angleB - angleA) < 180)
        {
            spline.InsertPointAt(0, localA);
            spline.InsertPointAt(1, localB);
        }
        else if (angleA > angleB && Math.Abs(angleA - angleB) > 180)
        {
            spline.InsertPointAt(0, localA);
            spline.InsertPointAt(1, localB);
        }
        else
        {
            spline.InsertPointAt(0, localB);
            spline.InsertPointAt(1, localA);
        }

        var borderLineSc = newBorder.GetComponent<BorderLineSc>();
        borderLineSc.SetColor(castle.GetComponent<CastleSc>().GetOwner().GetPlayerColor());

        _borderLines.Add(newBorder.GetComponent<BorderLineSc>());
    }

    public void SetColor(Color col)
    {
        foreach (var borderLine in _borderLines)
        {
            borderLine.SetColor(col);
        }
    }

    public void SetOwner(BasePlayer newOwner)
    {
        throw new NotImplementedException();
    }

    public BasePlayer GetOwner()
    {
        throw new NotImplementedException();
    }

    public event EventHandler<OwnerChangedEventArgs> OwnerChanged;
}
