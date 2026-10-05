using UnityEngine;

public class HealingZone : MonoBehaviour
{
    float timer;

    void OnTriggerStay2D(Collider2D other)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller != null && controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                timer = 1.0f; // відновлювати по 1 HP кожну секунду
            }
        }
    }
}