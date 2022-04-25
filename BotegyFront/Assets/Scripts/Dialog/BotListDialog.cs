using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotListDialog : MonoBehaviour
{
    private Button okButton;
    private Button backButton;

    private int selectedBotIndex;
    private List<Bot> botList;
    private Bot selectedBot;
    private void Awake()
    {
        okButton = transform.Find("SidePanel/LayoutParent2/OKButton").GetComponent<Button>();
        backButton = transform.Find("SidePanel/LayoutParent1/BackButton").GetComponent<Button>();
        
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(List<Bot> botList, Action onCancel, Action<string> onOk)
    {
        okButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        
        BuildBotListScene(botList);
        
        okButton.onClick.AddListener(() =>
        {
            Hide();
            onOk(botList[selectedBotIndex].id);
        });
        backButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });

    }
    
    
    private void BuildBotListScene(List<Bot> botList)
    {
        this.botList = botList;
        Transform parent = transform.Find("SidePanel/ScrollRect/Image/ScrollPanel");
        Clear(parent);
        Button botButton = parent.Find("BotButton").GetComponent<Button>();

        int index = 0;
        
        foreach (Bot bot in botList)
        {
            GameObject instance = Instantiate(botButton, parent).gameObject;
            instance.transform.Find("Text").GetComponent<Text>().text = "B" + index;
            instance.GetComponent<Button>().onClick.AddListener(() => ShowBotCode(index));

            instance.name = "BotButton" + index;
            index++;
            instance.SetActive(true);
        }

        if (botList.Count > 0)
        {
            Button b = parent.Find("BotButton0").GetComponent<Button>();
            b.onClick.Invoke();
            transform.Find("BotPanel").gameObject.SetActive(true);
        }
    }
    
    private void ShowBotCode(int index)
    {
        selectedBotIndex = index;
        
        transform.Find("BotPanel/Header/BotName").GetComponent<Text>().text = botList[selectedBotIndex].name;
        transform.Find("BotPanel/ScrollRect/Mask/CodePanel/Text").GetComponent<Text>().text = botList[selectedBotIndex].filePath; //code
    }
    
    private void Clear(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf)
                Destroy(child.gameObject);
        }
    }
}
