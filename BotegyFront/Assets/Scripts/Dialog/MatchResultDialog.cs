using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchResultDialog : MonoBehaviour
{
    private Button showMatchButton;
    private Button closeButton;
    
    private void Awake()
    {
        showMatchButton = transform.Find("Buttons/Image/PlayBackButton").GetComponent<Button>();
        closeButton = transform.Find("Buttons/Image/CloseButton").GetComponent<Button>();

        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(string text, Action onCancel, Action onOk)
    {
        transform.Find("Text").GetComponent<Text>().text = text;
        gameObject.SetActive(true);
        
        showMatchButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        
        showMatchButton.onClick.AddListener(() =>
        {
            Hide();
            onOk();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });

    }
}
