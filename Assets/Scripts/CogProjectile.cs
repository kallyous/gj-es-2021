using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogProjectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2D.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Cog collided with " + other.gameObject.name);
        
        ClockworkEnemyController e = other.collider.GetComponent<ClockworkEnemyController>();
        if (e != null) e.Fix();

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }
}
