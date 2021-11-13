using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public GameObject pickupEffect;
    public AudioClip audioClip;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null && controller.health < controller.maxHealth)
        {
            GameObject effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
            controller.PlaySound(audioClip);
            controller.ChangeHealth(1);
            Destroy(gameObject);
        }
    }
}
