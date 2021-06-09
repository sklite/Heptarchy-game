using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Players;
using UnityEngine;

public class LiquidSc : MonoBehaviour
{
    public int SourceCastle;


    // Start is called before the first frame update
    void Start()
    {
       // GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    collision.gameObject.SendMessage("ApplyDamage", 10);
        //}
        

        //var otherLiquid = collision.gameObject.GetComponent<LiquidSc>();
        //if (otherLiquid == null)
        //    return;

        //if (otherLiquid.SourceCastle == SourceCastle)
        //    return;
        
        
        //var rigidBody = GetComponent<Rigidbody2D>();

        //if (rigidBody.bodyType == RigidbodyType2D.Kinematic)
        //    return;
        
        //rigidBody.bodyType = RigidbodyType2D.Kinematic;
        //collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        //GetComponent<SpriteRenderer>().color = Color.black;

    }

    //void OnParticleCollision(GameObject other)
    //{
    //    var otherLiquid = other.GetComponent<LiquidSc>();
    //    if (otherLiquid == null)
    //        return;

    //    if (otherLiquid.SourceCastle == SourceCastle)
    //        return;

    //    if (otherLiquid.SourceCastle != SourceCastle)
    //    {
    //        GetComponent<SpriteRenderer>().color = Color.black;
    //    }


    //    //Rigidbody rb = other.GetComponent<Rigidbody>();
    //    //int i = 0;

    //    //while (i < numCollisionEvents)
    //    //{
    //    //    if (rb)
    //    //    {
    //    //        Vector3 pos = collisionEvents[i].intersection;
    //    //        Vector3 force = collisionEvents[i].velocity * 10;
    //    //        rb.AddForce(force);
    //    //    }
    //    //    i++;
    //    //}
    //}

    public void SetNewOwner(BasePlayer newOwner)
    {
        GetComponent<SpriteRenderer>().color = newOwner.GetPlayerColor();
    }

}
