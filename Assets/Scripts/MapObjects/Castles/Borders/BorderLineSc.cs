using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Events;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Players;
using UnityEngine;
using UnityEngine.U2D;

public class BorderLineSc : MonoBehaviour, IHaveOwner
{
    private SpriteShapeRenderer _shapeRenderer;
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
