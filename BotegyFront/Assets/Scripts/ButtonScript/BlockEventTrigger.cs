using UnityEngine;
using UnityEngine.EventSystems;

public class BlockEventTrigger : EventTrigger
{
    [SerializeField] private CodePanelManager _manager;

    public override void OnPointerClick(PointerEventData eventData)
    {   
        _manager.SetSelectedObject(eventData.pointerClick);
    }

    public CodePanelManager Manager
    {
        get => _manager;
        set => _manager = value;
    }
}
    