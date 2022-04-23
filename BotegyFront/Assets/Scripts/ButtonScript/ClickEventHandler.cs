using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEventHandler : MonoBehaviour
{
    [SerializeField] private CodePanelManager _manager;
    
    public void OnClick()
    {
        _manager.SetSelectedObject(null);
    }
}
