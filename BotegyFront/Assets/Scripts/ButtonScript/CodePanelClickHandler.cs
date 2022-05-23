using System;
using System.Collections;
using System.Collections.Generic;
using AppSceneManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodePanelClickHandler : EventTrigger
{
    [SerializeField] private CodePanelManager _manager;

    private void Awake()
    {
        GetComponent<GameObject>().AddComponent<CodePanelClickHandler>();
        
    }

    public override void OnPointerClick(PointerEventData eventData)
    {   
        _manager.SetSelectedObject(null);
    }
    
    public CodePanelManager Manager
    {
        get => _manager;
        set => _manager = value;
    }
}
