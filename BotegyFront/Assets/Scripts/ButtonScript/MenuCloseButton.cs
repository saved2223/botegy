using UnityEngine;
using UnityEngine.UI;

namespace ButtonScript
{
    public class MenuCloseButton : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private Transform menu;
        [SerializeField] private Transform codePanel;

        private void Awake()
        {
            _button = GetComponent<Button>();
        
            _button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            menu.gameObject.SetActive(false);
            codePanel.gameObject.SetActive(true);
        }
    }
}
