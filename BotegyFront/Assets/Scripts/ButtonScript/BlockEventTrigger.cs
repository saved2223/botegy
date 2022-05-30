using AppSceneManager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ButtonScript
{
    public class BlockEventTrigger : EventTrigger
    {
        [SerializeField] private BotEditorSceneManager _manager;

        public override void OnPointerClick(PointerEventData eventData)
        {   
            _manager.SetSelectedObject(eventData.pointerClick);
        }

        public BotEditorSceneManager Manager
        {
            get => _manager;
            set => _manager = value;
        }
    }
}
    