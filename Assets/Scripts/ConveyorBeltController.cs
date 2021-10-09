using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltController : MonoBehaviour
{
    public float force = 1000f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Colisão!");
        Rigidbody2D objBody = other.gameObject.GetComponent<Rigidbody2D>();
        if (objBody != null)
        {
            Vector2 direction = Vector2.right;
            objBody.AddForce(direction * force);
        }
    }
}
