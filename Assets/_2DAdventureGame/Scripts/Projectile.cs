using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Автоматичне знищення через 3 секунди, якщо куля нікуди не влучила
        Destroy(gameObject, 3.0f);
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Ігноруємо гравця
        if (other.GetComponent<PlayerController>() != null || other.CompareTag("Player"))
        {
            return;
        }

        // 2. Шукаємо ворога ПЕРШИМ ДІЛОМ (на самому об'єкті або його батьківських об'єктах)
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy == null)
        {
            enemy = other.GetComponentInParent<EnemyController>();
        }

        // Якщо знайшли ворога — полагодити його і знищити кулю
        if (enemy != null)
        {
            enemy.Fix();
            Destroy(gameObject);
            return;
        }

        // 3. Ігноруємо звичайні зони (DamageZone, HealingZone), якщо це був НЕ ворог
        if (other.isTrigger)
        {
            return;
        }

        // 4. Знищуємо кулю при влучанні у стіни та тверді перешкоди
        Destroy(gameObject);
    }
}