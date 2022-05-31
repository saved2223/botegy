using System;
using System.Collections.Generic;
using Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class BotListDialog : MonoBehaviour
    {
        private Button _okButton;
        private Button _backButton;

        private int _selectedBotIndex;
        private List<Bot> _botList;
        private void Awake()
        {
            _okButton = transform.Find("SidePanel/LayoutParent2/OKButton").GetComponent<Button>();
            _backButton = transform.Find("SidePanel/LayoutParent1/BackButton").GetComponent<Button>();
        
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(List<Bot> botList, Action onCancel, Action<string> onOk)
        {
            _okButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
        
            BuildBotListScene(botList);
        
            _okButton.onClick.AddListener(() =>
            {
                Hide();
                onOk(botList[_selectedBotIndex].id);
            });
            _backButton.onClick.AddListener(() =>
            {
                Hide();
                onCancel();
            });
        
            gameObject.SetActive(true);

        }
    
    
        private void BuildBotListScene(List<Bot> botList)
        {
            this._botList = botList;
            Transform parent = transform.Find("SidePanel/ScrollRect/Image/ScrollPanel");
            Clear(parent);
            Button botButton = parent.Find("BotButton").GetComponent<Button>();

            for (int i = 0; i < botList.Count; i++)
            {
                GameObject instance = Instantiate(botButton, parent).gameObject;
                instance.transform.Find("Text").GetComponent<Text>().text = "B" + i;

                int k = i;
                instance.GetComponent<Button>().onClick.AddListener(() => ShowBotCode(k));

                instance.name = "BotButton" + i;
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
            _selectedBotIndex = index;
        
            transform.Find("BotPanel/Header/BotName").GetComponent<Text>().text = _botList[_selectedBotIndex].name;
            transform.Find("BotPanel/Header").gameObject.SetActive(true);
            transform.Find("BotPanel/ScrollRect/Mask/CodePanel/Text").GetComponent<Text>().text
                = _botList[_selectedBotIndex].code.Replace("let ", "").Replace("this.field.", "");
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
}
