using UnityEngine;

namespace Beginner2D
{
    public class NonPlayerCharacter : MonoBehaviour
    {
        public float displayTime = 4.0f;
        public string dialogueText = "Привіт! Полагодь усіх зламаних роботів у цій зоні!";

        public void DisplayDialog()
        {
            if (UIHandler.instance != null)
            {
                UIHandler.instance.DisplayDialogue(dialogueText);
            }
        }
    }
}