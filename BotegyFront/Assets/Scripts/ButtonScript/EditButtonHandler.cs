using AppSceneManager;
using UnityEngine;

namespace ButtonScript
{
   public class EditButtonHandler : MonoBehaviour
   {
      public BotEditorSceneManager _manager;
   
      public void ButtonClicked()
      {
         _manager.EditBlock();
      }
   }
}
