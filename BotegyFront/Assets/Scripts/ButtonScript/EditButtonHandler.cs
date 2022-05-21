using System.Collections;
using System.Collections.Generic;
using AppSceneManager;
using UnityEngine;

public class EditButtonHandler : MonoBehaviour
{
   public CodePanelManager _manager;
   
   public void ButtonClicked()
   {
      _manager.EditBlock();
   }
}
