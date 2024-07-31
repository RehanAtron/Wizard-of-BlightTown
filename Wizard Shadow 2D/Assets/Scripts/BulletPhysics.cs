using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    public float timePassed;
    void Start() 
    {
        
    }
    void Update() 
    {
        if (timePassed >= 0)
        {
            timePassed -= Time.deltaTime;
        }
        if (gameObject.activeInHierarchy && timePassed <= 0)
        {
            gameObject.SetActive(false);  
        }    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStatus enemy = other.GetComponent<EnemyStatus>();
            if (enemy == null)
            {
                return;
            }
            enemy.hit = true;
            enemy.health -= 1 + Inventory.Instance.damage ;
            
            bool fireDamage = false;
            bool iceDamage = false;
            float burnTimer = 0;
            float freezeTimer = 0;
            int burnDamage = 0;

            Torch firstTorch = Inventory.Instance.torches[0];
            if (firstTorch != null)
            {
                fireDamage = firstTorch.fireDamage;
                iceDamage = firstTorch.iceDamage;
                burnTimer += firstTorch.burnTimer;
                freezeTimer += firstTorch.freezeTimer;
                burnDamage += firstTorch.burnDamage;
            }
            Torch secondTorch = Inventory.Instance.torches[1];
            if (secondTorch != null)
            {
                fireDamage = fireDamage || secondTorch.fireDamage;
                iceDamage = iceDamage || secondTorch.iceDamage;
                burnTimer += secondTorch.burnTimer;
                freezeTimer += secondTorch.freezeTimer;
                burnDamage += secondTorch.burnDamage;
            }

            if (fireDamage)
            {
                enemy.Burn(burnTimer, burnDamage);
            }

            if (iceDamage)
            {
                enemy.Freeze(freezeTimer);
            }

            if (enemy.health <= 0)
            {
                enemy.Die();
            }
        }
        gameObject.SetActive(false);
    }
}
