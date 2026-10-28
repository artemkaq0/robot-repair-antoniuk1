using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Налаштування руху
    public float speed = 4.0f;

    // Налаштування здоров'я
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    private int currentHealth;

    // Невразливість після отримання шкоди
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float invincibleTimer;

    // Префаб кулі
    public GameObject projectilePrefab;

    // Фізика та напрямок
    private Rigidbody2D rb;
    private Vector2 move;
    private Vector2 lookDirection = new Vector2(1, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveInput = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(moveInput.x, 0.0f) || !Mathf.Approximately(moveInput.y, 0.0f))
        {
            lookDirection.Set(moveInput.x, moveInput.y);
            lookDirection.Normalize();
        }

        move = moveInput;

        // Відлік таймера невразливості
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        // Постріл на клавішу C
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += move * speed * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }

    // Метод зміни здоров'я (лікування або шкода)
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("Здоров'я гравця: " + currentHealth + "/" + maxHealth);
    }

    void Launch()
    {
        if (projectilePrefab == null) return;

        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Launch(lookDirection, 300f);
        }
    }
}