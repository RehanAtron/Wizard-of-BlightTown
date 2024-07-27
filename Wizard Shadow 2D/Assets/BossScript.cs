using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] private GameObject head, spider;
    public float attackCooldown, attackDuration;
    [SerializeField] private GameObject[] arms;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private GameObject[] bones;
    Vector3[] orignalPositionTargets = new Vector3[6];
    Vector3[] originalPositionBones = new Vector3[6];
    public bool runAttack, hasReached;
    void Start()
    {
        OriginalCalculation();
    }

    // Update is called once per frame
    void Update()
    {
        if (head == null)
        {
            Destroy(gameObject);
        }
        if (attackCooldown <= 0)
        {
            AttackDecider();
        }
        else 
        {
            attackCooldown -= Time.deltaTime;
        }
        if (runAttack)
        {
            Elongate();
        }
    }

    void AttackDecider()
    {
        int attackNumber = Random.Range(1,4);
        if (attackNumber == 1)
        {
            SpiderSwarm();
        }
        if (attackNumber == 2)
        {
            runAttack = true;
            StartCoroutine(hasReachedStuff());
        }
        attackCooldown =  10;
    }

    void SpiderSwarm()
    {
        Vector3[] spawnLocations = new Vector3[6]{
            new Vector2(-3,4),
            new Vector2(-3,-4),
            new Vector2(-3,-8),
            new Vector2(3,4),
            new Vector2(3,-4),
            new Vector2(3,-8),
        };
        for (int i = 0; i < 6; i++)
        {
            if (arms[i] != null)
            {GameObject _spider = Instantiate(spider,transform.position + spawnLocations[i], Quaternion.identity);
            _spider.GetComponent<EnemyMovement>().range = 100;}
        }
    }

    void Elongate()
    {
        if (!hasReached)
        {
            for(int i = 0; i < 6; i++)
            {
                var target = targets[i];
                var bone = bones[i] ;
                if (target != null || bone != null)
                {
                    target.transform.position = Vector2.MoveTowards(target.transform.position,new Vector2(0,target.transform.position.y), 8 * Time.deltaTime);
                    bone.transform.position = Vector2.MoveTowards(bone.transform.position,target.transform.position, 4 * Time.deltaTime);
                }
            }
        }
        else
        {
            for(int i = 0; i < 6; i++)
            {
                var target = targets[i];
                var bone = bones[i] ;
                if (target != null || bone != null)
                {
                    target.transform.position = Vector2.MoveTowards(target.transform.position,orignalPositionTargets[i] , 4 * Time.deltaTime);
                    bone.transform.position = Vector2.MoveTowards(bone.transform.position,originalPositionBones[i], 4 * Time.deltaTime);
                }
            }
        }
    }
    IEnumerator hasReachedStuff()
    {
        yield return null;
        hasReached = false;
        yield return new WaitForSeconds(1);
        foreach (var arm in arms)
        {
            if (arm != null)
            {
                arm.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.5f);
        hasReached = true;
        yield return null;
        foreach (var arm in arms)
        {
            if (arm != null)
            {
                arm.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        yield return new WaitForSeconds(2);
        runAttack = false;
        hasReached = false;
    }

    void OriginalCalculation()
    {
        for (int i = 0; i < 6; i++)
        {
            originalPositionBones[i] = bones[i].transform.position;
            orignalPositionTargets[i] = targets[i].transform.position;
        }
    }
}
