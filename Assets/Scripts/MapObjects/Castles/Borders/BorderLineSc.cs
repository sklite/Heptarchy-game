using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Events;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Players;
using UnityEngine;
using UnityEngine.U2D;

public class BorderLineSc : MonoBehaviour//, IHaveOwner
{
    private SpriteShapeRenderer _shapeRenderer;
    private CastleSc _ownerCastle;
    protected CastleSc _oppositeCastle;

    SpriteShapeRenderer ShapeRenderer
    {
        get { _shapeRenderer ??= gameObject.GetComponent<SpriteShapeRenderer>(); return _shapeRenderer; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _shapeRenderer = GetComponent<SpriteShapeRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetOwnerCastle(CastleSc ownerCastle, CastleSc oppositeCastle)
    {
        _ownerCastle = ownerCastle;
        _oppositeCastle = oppositeCastle;

        ownerCastle.OwnerChanged += OwnerCastle_OwnerChanged;
        oppositeCastle.OwnerChanged += OppositeCastle_OwnerChanged;

        var newOwnerColor = _ownerCastle.GetOwner().GetPlayerColor();
        SetColor(newOwnerColor);
    }

    private void OppositeCastle_OwnerChanged(object sender, OwnerChangedEventArgs e)
    {
        if (_oppositeCastle.GetOwner() == _ownerCastle.GetOwner())
        {
            SetColor(new Color(0, 0, 0, 0));
            
        }
        else
        {
            var myOwnerColor = _ownerCastle.GetOwner().GetPlayerColor();
            SetColor(myOwnerColor);

        }
    }

    private void OwnerCastle_OwnerChanged(object sender, OwnerChangedEventArgs e)
    {
        if (_oppositeCastle.GetOwner() == _ownerCastle.GetOwner())
        {

            SetColor(new Color(0, 0, 0, 0));

        }
        else
        {
            var newOwnerColor = e.NewOwner.GetPlayerColor();
            SetColor(newOwnerColor);

        }





    }

    public void SetColor(Color col)
    { 
        ShapeRenderer.color = col;
    }

    public void SetOwner(BasePlayer newOwner)
    {
        //throw new System.NotImplementedException();
    }

    public BasePlayer GetOwner()
    {
        throw new System.NotImplementedException();
    }

    public event EventHandler<OwnerChangedEventArgs> OwnerChanged;
}
