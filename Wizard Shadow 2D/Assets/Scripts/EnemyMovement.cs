using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public GameObject player;
    private Vector3 target;
    private bool following;
    private float startTime, journeyLength;
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
        if (Vector2.Distance(transform.position,player.transform.position) < 2.5)
        {
            following = true;
        }
    }
    public void Move()
    {
        // Move the object
        if (target != transform.position)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float journeyFraction = distCovered / journeyLength;
            transform.position = Vector2.Lerp(transform.position, target, LerpTime(journeyFraction));
        }
    }

    float LerpTime(float t)
    {
        t = Mathf.Clamp01(t);
        return t * (1-t) * (1-t);
    }
}
