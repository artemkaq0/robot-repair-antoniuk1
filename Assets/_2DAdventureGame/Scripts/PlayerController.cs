using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public float speed = 3.0f; // Швидкість руху (можна змінювати в Inspector)

    Rigidbody2D rigidbody2d;
    Vector2 move;

    void Start()
    {
        MoveAction.Enable();
        // Отримуємо компонент фізики
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Зчитуємо ввід у кожному кадрі
        move = MoveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Рухаємо персонажа через фізичний рушій
        Vector2 position = rigidbody2d.position + move * speed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }
}