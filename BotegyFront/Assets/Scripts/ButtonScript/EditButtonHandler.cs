using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditButtonHandler : MonoBehaviour
{
   public CodePanelManager _manager;
   
   public void ButtonClicked()
   {
      _manager.EditBlock();
   }
}
