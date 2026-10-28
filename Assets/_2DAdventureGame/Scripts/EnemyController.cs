using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical = true;
    public float changeTime = 3.0f;

    private Rigidbody2D rb;
    private float timer;
    private int direction = 1;
    private Animator animator;
    private bool broken = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!broken) return;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken) return;

        Vector2 position = rb.position;

        if (vertical)
        {
            position.y += speed * direction * Time.fixedDeltaTime;
            if (animator != null)
            {
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", direction);
            }
        }
        else
        {
            position.x += speed * direction * Time.fixedDeltaTime;
            if (animator != null)
            {
                animator.SetFloat("Move X", direction);
                animator.SetFloat("Move Y", 0);
            }
        }

        rb.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null && broken)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rb.simulated = false;
        if (animator != null)
        {
            animator.SetTrigger("Fixed");
        }
    }
}