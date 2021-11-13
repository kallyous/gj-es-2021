using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltController : MonoBehaviour
{
    public float force = 100f;
    private Vector3 direction = Vector3.up;
    private Vector3 forward;
    private Vector3 position;
    
    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        Debug.DrawLine( position, position + direction, Color.red );
        Debug.DrawLine( position, position + forward, Color.green );
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Colisão!");
        Rigidbody2D objBody = other.gameObject.GetComponent<Rigidbody2D>();
        if (objBody != null)
        {
            float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
            float sin = Mathf.Sin( angle );
            float cos = Mathf.Cos( angle );
 
            forward = new Vector3(
                direction.x * cos - direction.y * sin,
                direction.x * sin + direction.y * cos,
                0f );
            
            objBody.AddForce(forward * force);
        }
    }
}
