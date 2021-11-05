using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmySc : MonoBehaviour
{
    Vector3 _speed;
    // Use this for collisions check
    GameObject[] _castles;

    BasePlayer _owner;
    BoxCollider2D _collider;

    // Color color;
    SpriteRenderer _sprite;

    GameObject _mapArmies;

    void Awake()
    {
        _sprite = GetComponentsInChildren<SpriteRenderer>().First(render => render.name == "ColorSprite");
    }

    void Start()
    {
        //		var camera = Camera.main;
        _castles = GameObject.FindGameObjectsWithTag("Castles");

        _collider = GetComponent<BoxCollider2D>();

        _mapArmies = GameObject.Find("Armies");

        transform.SetParent(_mapArmies.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsSc.IsPaused)
            return;

        transform.Translate(_speed, Space.World);
    }

    void FixedUpdate()
    {
        if (SettingsSc.IsPaused)
            return;
        
        CheckCollisions();
    }


    void CheckCollisions()
    {
        foreach (var castle in _castles)
        {
            var castleCollider = castle.GetComponent<BoxCollider2D>();
            if (castleCollider.bounds.Intersects(_collider.bounds))
            {
                //TODO: 
               // castle.GetComponent<CastleSc>().Visit(this);
                return;
            }
        }
    }

    public GameObject Destination
    {
        get;
        set;
    }

    public float Amount
    {
        get;
        set;
    }

    public void SetSpeed(Vector3 newSpeed)
    {
        _speed = newSpeed;
    }

    public BasePlayer GetOwner()
    {
        return _owner;
    }

    public void SetOwner(BasePlayer newOwner)
    {
        _sprite.color = newOwner.GetPlayerColor();
        _owner = newOwner;
    }

    internal void Destroy()
    {
        Destroy(gameObject);
    }
}
