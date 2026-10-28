using UnityEngine;
using UnityEngine.InputSystem;
using Beginner2D;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    private Rigidbody2D rigidbody2d;
    private Vector2 move;
    public float speed = 3.0f;

    public InputAction LaunchAction;
    public InputAction TalkAction;

    public int maxHealth = 5;
    private int currentHealth;
    public int health { get { return currentHealth; } }

    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float damageCooldown;

    private Animator animator;
    private Vector2 moveDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;

    private AudioSource audioSource;
    public AudioClip playerHit;
    public AudioClip projectileThrow;

    public ParticleSystem hitParticleEffect;

    private void Awake()
    {
        Helpers.RecursiveLayerSet(transform, Helpers.PlayerLayer);
    }

    private void OnEnable()
    {
        Helpers.RecursiveLayerSet(transform, Helpers.PlayerLayer);
    }

    void Start()
    {
        MoveAction.Enable();
        TalkAction.Enable();
        LaunchAction.Enable();

        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        // Шукаємо Animator на об'єкті або його дочірніх об'єктах
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (UIHandler.instance != null)
        {
            UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        }
    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        // Перевіряємо наявність аніматора перед використанням
        if (animator != null)
        {
            animator.SetFloat("Look X", moveDirection.x);
            animator.SetFloat("Look Y", moveDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        }

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        if (LaunchAction.WasPerformedThisFrame())
        {
            Launch();
        }

        if (TalkAction.WasPerformedThisFrame())
        {
            InteractWithNPC();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return;

            PlaySound(playerHit);
            if (hitParticleEffect != null)
            {
                Instantiate(hitParticleEffect, transform.position + Vector3.up * 0.6f, Quaternion.identity);
            }
            isInvincible = true;
            damageCooldown = timeInvincible;

            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (UIHandler.instance != null)
        {
            UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        }

        if (currentHealth <= 0 && GameManager.instance != null)
        {
            GameManager.instance.PlayerDied();
        }
    }

    void Launch()
    {
        if (projectilePrefab == null) return;

        PlaySound(projectileThrow);
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Launch(moveDirection, 300);
        }

        if (animator != null)
        {
            animator.SetTrigger("Launch");
        }
    }

    private void InteractWithNPC()
    {
        Debug.Log("--> [1] Кнопка розмови натиснута!");

        int npcLayer = LayerMask.NameToLayer("NPC");
        int layerMask = (npcLayer != -1) ? (1 << npcLayer) : (1 << Helpers.NPCLayer);

        Collider2D hit = Physics2D.OverlapCircle(rigidbody2d.position, 1.5f, layerMask);

        if (hit != null)
        {
            Debug.Log($"--> [2] УСПІХ: Знайдено об'єкт '{hit.name}'!");

            // Шукаємо скрипт як на самому об'єкті, так і на його батьківських об'єктах
            NonPlayerCharacter npc = hit.GetComponent<NonPlayerCharacter>();
            if (npc == null)
            {
                npc = hit.GetComponentInParent<NonPlayerCharacter>();
            }

            if (npc != null)
            {
                Debug.Log("--> [3] Запуск діалогу!");
                npc.DisplayDialog();
            }
            else
            {
                Debug.LogError($"--> [ПОМИЛКА] Об'єкт '{hit.name}' має шар NPC, але на ньому (і його батьках) немає скрипта NonPlayerCharacter!");
            }
        }
        else
        {
            Debug.LogWarning("--> [ЗБОЙ] Поруч немає об'єктів із шаром 'NPC'.");
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}