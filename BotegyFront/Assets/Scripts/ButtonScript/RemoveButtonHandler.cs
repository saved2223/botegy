using System;
using AppSceneManager;
using UnityEngine;
using UnityEngine.UI;

public class RemoveButtonHandler : MonoBehaviour
{
    public CodePanelManager _manager;

    public void ButtonClicked()
    {
        _manager.DeleteBlock();
    }
}
