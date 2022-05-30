using AppSceneManager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ButtonScript
{
    public class CodePanelClickHandler : EventTrigger
    {
        [SerializeField] private BotEditorSceneManager _manager;

        private void Awake()
        {
            GetComponent<GameObject>().AddComponent<CodePanelClickHandler>();
        
        }

        public override void OnPointerClick(PointerEventData eventData)
        {   
            _manager.SetSelectedObject(null);
        }
    
        public BotEditorSceneManager Manager
        {
            get => _manager;
            set => _manager = value;
        }
    }
}
