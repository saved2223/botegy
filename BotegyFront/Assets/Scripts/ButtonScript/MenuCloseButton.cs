using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuCloseButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private Transform menu;
    [SerializeField] private Transform codePanel;

    private void Awake()
    {
        button = GetComponent<Button>();
        
        button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked()
    {
        menu.gameObject.SetActive(false);
        codePanel.gameObject.SetActive(true);
    }
}
