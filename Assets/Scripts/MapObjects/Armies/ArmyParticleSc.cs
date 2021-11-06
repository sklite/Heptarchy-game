using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Players;
using UnityEngine;

public class ArmyParticleSc : MonoBehaviour
{
    public bool IsFighting = false;
    float _speed;


    // Use this for collisions check
    GameObject[] _castles;
    BasePlayer _owner;
    CircleCollider2D _collider;

    SpriteRenderer _sprite;
    Rigidbody2D _rigidBody;
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _castles = GameObject.FindGameObjectsWithTag("Castles");

        _collider = GetComponent<CircleCollider2D>();
        transform.SetParent(GameObject.Find("Armies").transform);

        _animator = GetComponentInChildren<Animator>();
    }

    void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (IsFighting)
        {
            return;
        }
        
        var armySpeed2d = MathCalculator.Calc2DSpeed(transform.position, Destination.transform.position, _speed);

        _sprite.flipX = armySpeed2d.x < 0;

        _rigidBody.MovePosition(transform.position + armySpeed2d * Time.deltaTime);
        
        CheckCollisions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsFighting)
            return;

        if (collision.gameObject.tag == "Armies")
        {
            var incomingArmySc = collision.gameObject.GetComponent<ArmyParticleSc>();
            if (incomingArmySc.GetOwner() == _owner)
                return;

            _animator.Play("Attack", 0, 0.15f);
            IsFighting = true;
            StartCoroutine(Die(5));
        }
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

    public void SetBaseSpeed(float newSpeed)
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
        _sprite.color = Color.Lerp(_sprite.color, Color.white, 0.9f);
        _owner = newOwner;
    }

    internal IEnumerator Die(float delay)
    {
        _collider.enabled = false;
        _rigidBody.bodyType = RigidbodyType2D.Static; 
        yield return new WaitForSeconds(delay);
        _animator.Play("Die", 0, 0.25f);
        yield return new WaitForSeconds(delay);
        //if (delay > 0)

        Destroy();
    }

    internal void Destroy()
    {
        Destroy(gameObject);
    }
}
