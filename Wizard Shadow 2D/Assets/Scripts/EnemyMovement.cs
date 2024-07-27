using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public GameObject player,tip1,tip2;
    public bool isSpider, isDog, isHuge, isHuman, isStill;
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
        if (isSpider)
        {
            if (target != transform.position)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float journeyFraction = distCovered / journeyLength;
                transform.position = Vector2.Lerp(transform.position, target, LerpTime(journeyFraction));
            }
        }
        if (isStill)
        if (target != transform.position)
            {
                tip1.transform.position = Vector2.Lerp(tip1.transform.position, target, moveSpeed * Time.deltaTime);
                tip2.transform.position = Vector2.Lerp(tip2.transform.position, target, moveSpeed * Time.deltaTime);
            }

    }

    float LerpTime(float t)
    {
        t = Mathf.Clamp01(t);
        return t * (1-t) * (1-t);
    }
}
