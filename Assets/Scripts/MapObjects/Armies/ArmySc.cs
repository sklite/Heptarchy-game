using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmySc : MonoBehaviour
{
    Vector3 _speed;
    // Use this for initialization
    GameObject[] _castles;
    GameObject _destination;

    BasePlayer _owner;
    BoxCollider2D _collider;

    // Color color;
    SpriteRenderer _sprite;

    GameObject _mapArmies;

    void Awake()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _sprite = spriteRenderers.First(render => render.name == "ColorSprite");

        //foreach (var item in spriteRenderers)
        //{
        //    if (item.name == "ColorSprite")
        //    {
        //        sprite = item;
        //    }
        //}
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

        //transform.Translate(Vector3.up * Time.deltaTime, Space.World);

        CheckCollisions();
    }


    void CheckCollisions()
    {
        foreach (var castle in _castles)
        {

            var castleCollider = castle.GetComponent<BoxCollider2D>();
            if (castleCollider.bounds.Intersects(_collider.bounds))
            {
                castle.GetComponent<CastleSc>().Visit(this);
                return;
            }
        }
    }


    //bool VectorEqual(Vector3 left, Vector3 right, int digit)
    //{
    //    return new Vector3()
    //}


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
