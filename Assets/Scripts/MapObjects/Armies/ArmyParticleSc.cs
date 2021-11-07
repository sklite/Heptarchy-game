using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Assets.Scripts.Helpers;
using Assets.Scripts.Players;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class ArmyParticleSc : MonoBehaviour
{
    public ArmyIs CurentState = ArmyIs.Moving;
    float _speed = 10;


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
        if (CurentState == ArmyIs.Fighting)
        {
            return;
        }

        if (CurentState == ArmyIs.GoingToFight)
        {
            int sf = 20;
        }

        var armySpeed2d = MathCalculator.Calc2DSpeed(transform.position, Destination.transform.position, _speed);
        _sprite.flipX = armySpeed2d.x < 0;
        _rigidBody.MovePosition(transform.position + armySpeed2d * Time.deltaTime);

        if (CurentState == ArmyIs.Moving)
        {
            CheckCastleCollisions();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (CurentState != ArmyIs.Moving)
            yield break;

        if (collision.gameObject.tag == "Armies")
        {
            var incomingArmySc = collision.gameObject.GetComponent<ArmyParticleSc>();
            if (incomingArmySc.GetOwner() == _owner)
                yield break;

            //Go to the enemy army for 0.3 sec
            CurentState = ArmyIs.GoingToFight;
            Destination = collision.gameObject;
            yield return new WaitForSeconds(0.3f);

            //And then attack
            _animator.Play("Attack", 0, 0.15f);
            CurentState = ArmyIs.Fighting;


            StartCoroutine(Die(5));
        }
    }


    void CheckCastleCollisions()
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
        _sprite.color = Color.Lerp(_sprite.color, Color.white, 0.85f);
        _owner = newOwner;
    }

    internal IEnumerator Die(float delay)
    {
        _collider.enabled = false;
        _rigidBody.bodyType = RigidbodyType2D.Static; 
        yield return new WaitForSeconds(delay);
        _animator.Play("Die", 0, 0.25f);
        yield return new WaitForSeconds(delay);
        Destroy();
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    internal void Destroy()
    {
        Destroy(gameObject);
    }
}

public enum ArmyIs
{
    Moving,
    GoingToFight,
    Fighting
}
