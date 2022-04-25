using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonHandler : MonoBehaviour
{
    public CodePanelManager _manager;
   
    public void ButtonClicked()
    {
        _manager.TestConvertToProgram();
    }
}