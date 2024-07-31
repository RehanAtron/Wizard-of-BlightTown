using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public GameObject player,tip1,tip2;
    public bool isSpider, isDog, isHuge, isHuman, isStill, isHit;
    private Vector3 target;
    private bool following;
    private float startTime, journeyLength;
    public float  range;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            target = player.transform.position;
            startTime = Time.time;
            journeyLength = Vector2.Distance(transform.position,player.transform.position);
        }
        if (Vector2.Distance(transform.position,player.transform.position) < range && player.GetComponent<PlayerMovement>().teleportCooldown < 0.5)
        {
            following = true;
        }
    }
    public void Move()
    {
        // Move the object
        if (target != transform.position)
        {
            if (isSpider || isHuge)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float journeyFraction = distCovered / journeyLength;
                transform.position = Vector2.Lerp(transform.position, target, LerpTime(journeyFraction));
                if(isHuge)
                {
                    if (target.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-3.3f,3.3f,3.3f);
                    }
                    else 
                    {
                        transform.localScale = new Vector3(3.3f,3.3f,3.3f);
                    }
                }
            }
            if (isStill)
            {
                tip1.transform.position = Vector2.Lerp(tip1.transform.position, target, moveSpeed * Time.deltaTime);
                tip2.transform.position = Vector2.Lerp(tip2.transform.position, target, moveSpeed * Time.deltaTime);
            }
            if (isHuman && isHit)
            {
                transform.position  = Vector2.Lerp(transform.position, target, moveSpeed * 4 * Time.deltaTime);
                GetComponent<Collider2D>().isTrigger = false;;
            }
        }
    }

    float LerpTime(float t)
    {
        t = Mathf.Clamp01(t);
        return t * (1-t) * (1-t);
    }
}
