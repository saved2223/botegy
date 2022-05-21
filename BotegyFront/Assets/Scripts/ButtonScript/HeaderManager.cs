using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderManager : MonoBehaviour
{
    [SerializeField] private TextInputWindow inputTextDialog;
    [SerializeField] private Text header;
    
    private void Awake()
    {
        //set bot name came from server

        header.text = "BOT1";
    }

    public void ChangeBotName()
    {
        ShowTextInputWindow("ВВЕДИТЕ НАЗВАНИЕ БОТА");
    }
    
    private void ShowTextInputWindow(string title)
    {
        inputTextDialog.Show(title, header.text, () => { }, (s) =>
        {
            if (s.Length > 0)
                header.text = s;
        });
    }    
}
