using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

    public float invincibleDuration = 2f;
    private bool isInvincible;
    private float invincibleCountdown;
    
    private Rigidbody2D rigidbody2d;
    private float horizontal;
    private float vertical;
    public float moveSpeed = 3f;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    public GameObject projectilePrefab;
    
    private AudioSource audioSource;
    public AudioClip hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        /*QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 10;*/
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        isInvincible = false;
    }
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        CogProjectile projectile = projectileObject.GetComponent<CogProjectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else audioSource.Stop();
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleCountdown -= Time.deltaTime;
            if (invincibleCountdown < 0)
                isInvincible = false;
        }
        
        Debug.DrawRay( rigidbody2d.position + Vector2.up * 0.5f, lookDirection, Color.green );
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.5f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
                if (npc != null) npc.DisplayDialog();
            }
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        
        position.x = position.x + moveSpeed * horizontal * Time.deltaTime;
        position.y = position.y + moveSpeed * vertical * Time.deltaTime;
        
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (!isInvincible)
            {
                audioSource.PlayOneShot(hurtSound);
                
                animator.SetTrigger("Hit");
                currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
                
                isInvincible = true;
                invincibleCountdown = invincibleDuration;
            }
            else
                return;
        }
        else
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
}
