using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Players;
using UnityEngine;

public class ArmyParticleSc : MonoBehaviour
{
    Vector3 _speed;
    // Use this for collisions check
    GameObject[] _castles;

    BasePlayer _owner;
    CircleCollider2D _collider;

    // Color color;
    SpriteRenderer _sprite;

    GameObject _mapArmies;
    Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _castles = GameObject.FindGameObjectsWithTag("Castles");

        _collider = GetComponent<CircleCollider2D>();

        transform.SetParent(GameObject.Find("Armies").transform);
    }

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        var newPosition = new Vector2(transform.position.x + _speed.x, transform.position.y + _speed.y);
        _rigidBody.MovePosition(newPosition);

        CheckCollisions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckCollisions()
    {
        foreach (var castle in _castles)
        {
            var castleCollider = castle.GetComponent<CircleCollider2D>();
            if (castleCollider.bounds.Intersects(_collider.bounds))
            {
                castle.GetComponent<CastleSc>().Visit(this);
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
