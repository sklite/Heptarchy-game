using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmySc : MonoBehaviour
{
    Vector3 speed;
    // Use this for initialization
    GameObject[] castles;
    GameObject destination;

    BasePlayer owner;
    new BoxCollider2D collider;

    // Color color;
    SpriteRenderer sprite;

    GameObject mapArmies;

    void Awake()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        sprite = spriteRenderers.First(render => render.name == "ColorSprite");

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
        castles = GameObject.FindGameObjectsWithTag("Castles");

        collider = GetComponent<BoxCollider2D>();

        mapArmies = GameObject.Find("Armies");

        transform.SetParent(mapArmies.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsSc.IsPaused)
            return;
        transform.Translate(speed, Space.World);



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
        foreach (var castle in castles)
        {

            var castleCollider = castle.GetComponent<BoxCollider2D>();
            if (castleCollider.bounds.Intersects(collider.bounds))
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
        this.speed = newSpeed;
    }

    public BasePlayer GetOwner()
    {
        return owner;
    }

    public void SetOwner(BasePlayer newOwner)
    {
        sprite.color = newOwner.GetPlayerColor();
        this.owner = newOwner;
    }

    internal void Destroy()
    {
        Destroy(gameObject);
    }
}
