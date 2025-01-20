using UnityEngine;
using UnityEngine.UIElements;

namespace Test.Scripts
{
    public class TestSkillUI : MonoBehaviour
    {
        [SerializeField] private UIDocument m_uiDocument;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            var button1 = m_uiDocument.rootVisualElement.Query<Button>("Skill1_Btn").AtIndex(0);
            var button2 = m_uiDocument.rootVisualElement.Query<Button>("Skill2_Btn").AtIndex(0);

            button1.clicked += () => Debug.Log("Hello 1");
            button2.clicked += () => Debug.Log("Hello 2");

            Time.timeScale = 0;
        }
    }
}
