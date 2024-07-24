using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] private GameObject itemGenerator;
    private EnemyMovement enemyMovement;
    public bool hit;
    public int health, burnDamage;
    private bool frozen, burning;
    public float freezeTimer, burnTimer;
    private float freezeCooldown;
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        HitIdentify();
        if (frozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                frozen = false;
            }
        }
        else
        {
            enemyMovement.Move();
            if (freezeCooldown > 0)
            {
                freezeCooldown -= Time.deltaTime;
            }
        }
        
        if (burnTimer > 0)
        {
            burnTimer -= Time.deltaTime;
            if (!burning)
            {
                InvokeRepeating("BurnEffect",1,2);
                burning = true;
            }
        }
        else
        {
            burning = false;
            CancelInvoke("BurnEffect");
        }
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        int random = Random.Range(0,101);
        if (random < 90)
        {
            Instantiate(itemGenerator, transform.position,Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void HitIdentify()
    {
        if (hit)
        {
            StartCoroutine(ColorChange());
            hit = false;
        }
    }
    public void Freeze(float duration)
    {
        if (freezeCooldown <= 0)
        {
            frozen = true;
            freezeTimer = duration;
            freezeCooldown = 5;
        }
    }
    public void Burn(float duration, int Damage)
    {
        if (burnTimer <= 0)
        {
            burnTimer = duration;
        }
        burnDamage = Damage;
    }
    public void BurnEffect()
    {
        health -= burnDamage;
        StartCoroutine(ColorChange());
    }

    IEnumerator ColorChange()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f,0,0);
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }
}
