using UnityEngine;
using UnityEngine.UIElements;

namespace Beginner2D
{
    public class UIHandler : MonoBehaviour
    {
        public static UIHandler instance { get; private set; }

        private VisualElement m_HealthBar;
        private Label m_CounterLabel;
        private VisualElement m_DialogueElement;
        private Label m_DialogueLabel;
        private VisualElement m_WinScreen;
        private VisualElement m_LoseScreen;

        private float m_TimerDisplay = -1.0f;

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
            UIDocument uiDocument = GetComponent<UIDocument>();
            if (uiDocument == null) return;

            VisualElement root = uiDocument.rootVisualElement;

            m_HealthBar = root.Q<VisualElement>("HealthBar");
            m_CounterLabel = root.Q<Label>("CounterLabel");
            m_DialogueElement = root.Q<VisualElement>("NPCDialogue");
            m_DialogueLabel = root.Q<Label>("DialogueLabel");
            m_WinScreen = root.Q<VisualElement>("WinScreenContainer");
            m_LoseScreen = root.Q<VisualElement>("LoseScreenContainer");

            if (m_DialogueElement != null)
            {
                m_DialogueElement.style.display = DisplayStyle.None;
            }
        }

        private void Update()
        {
            if (m_TimerDisplay > 0)
            {
                m_TimerDisplay -= Time.deltaTime;
                if (m_TimerDisplay <= 0)
                {
                    if (m_DialogueElement != null)
                    {
                        m_DialogueElement.style.display = DisplayStyle.None;
                    }
                }
            }
        }

        public void SetHealthValue(float percentage)
        {
            if (m_HealthBar != null)
            {
                m_HealthBar.style.width = Length.Percent(percentage * 100);
            }
        }

        public void SetCounter(int current, int total)
        {
            if (m_CounterLabel != null)
            {
                m_CounterLabel.text = $"Полагоджено: {current} / {total}";
            }
        }

        public void DisplayDialogue(string text)
        {
            if (m_DialogueElement != null && m_DialogueLabel != null)
            {
                m_DialogueLabel.text = text;
                m_DialogueElement.style.display = DisplayStyle.Flex;
                m_TimerDisplay = 4.0f;
            }
            else
            {
                Debug.LogWarning("UIHandler: Не знайдено елемент NPCDialogue або DialogueLabel у UXML!");
            }
        }

        public void DisplayWinScreen()
        {
            if (m_WinScreen != null) m_WinScreen.style.opacity = 1;
        }

        public void DisplayLoseScreen()
        {
            if (m_LoseScreen != null) m_LoseScreen.style.opacity = 1;
        }
    }
}