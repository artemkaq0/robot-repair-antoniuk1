using UnityEngine;

namespace Beginner2D
{
    public class NonPlayerCharacter : MonoBehaviour
    {
        public float displayTime = 4.0f;
        public string dialogueText = "Вітаю на моєму рівні! Будь обережним на третій ділянці, там бродить дуже швидкий червоний робот!";

        public void DisplayDialog()
        {
            if (UIHandler.instance != null)
            {
                UIHandler.instance.DisplayDialogue(dialogueText);
            }
        }
    }
}