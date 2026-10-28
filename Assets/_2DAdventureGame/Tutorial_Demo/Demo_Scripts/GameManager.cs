using UnityEngine;
using UnityEngine.SceneManagement;

namespace Beginner2D
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }
        public bool gameOver { get; private set; } = false;

        private int totalEnemies;
        private int fixedEnemies = 0;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Start()
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            totalEnemies = enemies.Length;

            if (UIHandler.instance != null)
            {
                UIHandler.instance.SetCounter(fixedEnemies, totalEnemies);
            }
        }

        public void OnRobotFixed()
        {
            if (gameOver) return;

            fixedEnemies++;
            if (UIHandler.instance != null)
            {
                UIHandler.instance.SetCounter(fixedEnemies, totalEnemies);
            }

            if (fixedEnemies >= totalEnemies)
            {
                WinGame();
            }
        }

        public void PlayerDied()
        {
            if (gameOver) return;

            gameOver = true;
            if (UIHandler.instance != null) UIHandler.instance.DisplayLoseScreen();
            Invoke(nameof(RestartScene), 3.0f);
        }

        private void WinGame()
        {
            gameOver = true;
            if (UIHandler.instance != null) UIHandler.instance.DisplayWinScreen();
            Invoke(nameof(RestartScene), 3.0f);
        }

        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}