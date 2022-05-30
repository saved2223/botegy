using AppSceneManager;
using UnityEngine;
using UnityEngine.UI;

namespace ButtonScript
{
    public class BlockButtonHandler : MonoBehaviour
    {
        private string _category;
        private string _targetClass;
        private Button _button;
        private BotEditorSceneManager _manager;


        public string Category
        {
            get => _category;
            set => _category = value;
        }

        public string TargetClass
        {
            get => _targetClass;
            set => _targetClass = value;
        }

        public BotEditorSceneManager Manager
        {
            get => _manager;
            set => _manager = value;
        }

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            _manager.AddBlock(_category, _targetClass);
       
        }
    }
}
