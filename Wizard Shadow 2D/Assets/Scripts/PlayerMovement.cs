using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] AnimationManager animator;

    Rigidbody2D rb;
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] float maxmoveSpeed;
    public Vector2 movement;
    
    [SerializeField] GameObject torch;
    [Space]
    public float knockbackForce, flipSpeed;
    private float kbCounter, hitCooldown;
    public float teleportCooldown;
    [Space]
    private const string playerIdle = "Player_Idle";
    private const string playerMove = "Player_Move";
    private const string playerHit = "Player_Hit";
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        kbCounter -= Time.unscaledDeltaTime;
        teleportCooldown -= Time.deltaTime;
        if (kbCounter <= 0)
        {
            Time.timeScale = 1;
            if (hitCooldown > 0)
            {
                hitCooldown -= Time.deltaTime;
            }
            Move();
            Flip();
        }

    }

    void Move()
    {
        //Inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Diagonal movement
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }

        //movement
        float currentSpeedX = maxmoveSpeed * movement.x * Time.deltaTime;
        float currentSpeedY = maxmoveSpeed * movement.y * Time.deltaTime;

        //Animation
        if (Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.y) > 0)
        {
            animator.ChangeAnimation(playerMove);
        }
        else
        {
            animator.ChangeAnimation(playerIdle);
        }

        transform.Translate(new Vector3(currentSpeedX,currentSpeedY));
    }
    void Flip()
    {
        Vector3 targetScale = transform.localScale;
        float rotZ = playerShoot.rotZ;
        if (rotZ <= -90 || rotZ >= 90)
        {
            targetScale.x = -1;
        }
        else if (rotZ >= -90 || rotZ <= 90)
        {
            targetScale.x = 1;
        }
        float t = flipSpeed * Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Mathf.Sqrt(t));
        torch.transform.localScale = new Vector3(torch.transform.localScale.x, 0.2f * targetScale.x, 0);
    }
    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.collider.CompareTag("Enemy") && kbCounter <= 0 && hitCooldown <= 0)
        {
            Vector2 direction = (transform.position - other.collider.transform.position).normalized;
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            Time.timeScale = 0;
            kbCounter = 0.5f;
            animator.ChangeAnimation(playerHit);
            transform.localScale = new Vector3(1,1,1);
            hitCooldown = 1;
        }
        if (other.collider.CompareTag("Door") && teleportCooldown <= 0)
        {
            teleportCooldown = 2;
        }
    }

}
