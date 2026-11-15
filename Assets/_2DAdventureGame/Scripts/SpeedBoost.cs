using System.Collections;
using UnityEngine;

namespace Beginner2D
{
    public class SpeedBoost : MonoBehaviour
    {
        public float speedMultiplier = 1.6f;
        public float duration = 5.0f;
        public AudioClip boostSound;

        void OnTriggerEnter2D(Collider2D other)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                if (boostSound != null)
                {
                    player.PlaySound(boostSound);
                }

                player.StartCoroutine(ApplySpeedBoost(player));

                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, duration + 0.1f);
            }
        }

        IEnumerator ApplySpeedBoost(PlayerController player)
        {
            float originalSpeed = player.speed;
            player.speed *= speedMultiplier;

            yield return new WaitForSeconds(duration);

            player.speed = originalSpeed;
        }
    }
}