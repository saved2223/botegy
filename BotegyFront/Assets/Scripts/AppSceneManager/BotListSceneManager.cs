using System.Collections.Generic;
using Dialog;
using Entity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppSceneManager
{
    public class BotListSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject botListScene;
        [SerializeField] private GameObject botInfoScene;

        [SerializeField] private DeleteConfirmDialog deleteConfirmDialog;
        [SerializeField] private EditConfirmDialog editConfirmDialog;
        [SerializeField] private TextInputWindow changeUsernameDialog;

        [SerializeField] private Animator animator;

        private RequestProcessor _processor;
        private GameObject _active;

        private List<Bot> _botList;
        private int _selectedBotIndex;


        private void Awake()
        {
            _processor = gameObject.AddComponent<RequestProcessor>();
            _active = botListScene;

            BotListScene();
        }

        private void SetActive(GameObject target)
        {
            _active.SetActive(false);
            target.SetActive(true);
            _active = target;
        }

        private void Clear(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child.gameObject.activeSelf)
                    Destroy(child.gameObject);
            }
        }

        public void UserScene()
        {
            SceneManager.LoadScene("UserScene");
        }

        public void BotListScene()
        {
            _processor.GenerateGetRequest<Bot>("getBotsForPlayer",
                new Dictionary<string, string>()
                {
                    {"playerId", SceneInfo.CurrUser.id}
                },
                BotListCoroutine);
        }


        private void BotListCoroutine(string json)
        {
            _botList = Serializer.DeserializeObjectList<Bot>(json);

            BuildBotListScene();
            SetActive(botListScene);
        }

        private void BuildBotListScene()
        {
            Transform parent = botListScene.transform.Find("SidePanel/ScrollRect/Image/ScrollPanel");

            Clear(parent);
            Button botButton = parent.Find("BotButton").GetComponent<Button>();

            for (int i = 0; i < _botList.Count; i++)
            {
                GameObject instance = Instantiate(botButton, parent).gameObject;
                instance.transform.Find("Text").GetComponent<Text>().text = "B" + i;

                int k = i;
                instance.GetComponent<Button>().onClick.AddListener(() => ShowBotCode(k));

                instance.name = "BotButton" + i;
                instance.SetActive(true);
            }

            if (_botList.Count > 0)
            {
                Button b = parent.Find("BotButton0").GetComponent<Button>();
                b.onClick.Invoke();
                botListScene.transform.Find("BotPanel").gameObject.SetActive(true);
            }
            else
            {
                botListScene.transform.Find("BotPanel/ScrollRect/Mask/CodePanel/Text").GetComponent<Text>().text = "";
                botListScene.transform.Find("BotPanel/Header").gameObject.SetActive(false);
                botListScene.transform.Find("BotPanel/EditButtons").gameObject.SetActive(false);
            }
        }


        private void ShowBotCode(int index)
        {
            _selectedBotIndex = index;

            _processor.GenerateGetRequest<BotMatchWrapper>("getMatchesForBot",
                new Dictionary<string, string>()
                {
                    {"botId", _botList[_selectedBotIndex].id}
                },
                ShowBotCodeCoroutine);
        }

        private void ShowBotCodeCoroutine(string json)
        {
            BotMatchWrapper m = Serializer.DeserializeObject<BotMatchWrapper>(json);

            botListScene.transform.Find("BotPanel/Header/BotName").GetComponent<Text>().text =
                _botList[_selectedBotIndex].name;
            botListScene.transform.Find("BotPanel/Header").gameObject.SetActive(true);
            botListScene.transform.Find("BotPanel/EditButtons").gameObject.SetActive(true);

            botListScene.transform.Find("BotPanel/ScrollRect/Mask/CodePanel/Text").GetComponent<Text>().text =
                _botList[_selectedBotIndex].code.Replace("let ", "").Replace("this.field.", "");

            Button button = botListScene.transform.Find("BotPanel/EditButtons/InfoButton").GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                animator.SetTrigger("botlist_close");
                animator.SetTrigger("botinfo_show");

                BotInfoScene(m);
            });
        }

        private void DeleteBot()
        {
            _processor.GenerateDeleteRequest("deleteBot",
                new Dictionary<string, string>()
                {
                    {"botId", _botList[_selectedBotIndex].id}
                },
                DeleteBotCoroutine);
        }


        private void DeleteBotCoroutine()
        {
            SceneInfo.EditedBot = null;
            _selectedBotIndex = -1;
            _botList = null;
            BotListScene();
        }

        private void EditBot()
        {
            SceneInfo.EditedBot = _botList[_selectedBotIndex];
            SceneManager.LoadScene("BotEditorScene");
        }

        private void DuplicateBot()
        {
            SceneInfo.EditedBot = new Bot
            {
                code = _botList[_selectedBotIndex].code
            };
            SceneManager.LoadScene("BotEditorScene");
        }

        private void ChangeBotName(string name)
        {
            _processor.GeneratePutRequest("updateBot",
                new Dictionary<string, string>()
                {
                    {"botId", _botList[_selectedBotIndex].id},
                    {"name", RequestProcessor.PrepareName(name)},
                    {"code", _botList[_selectedBotIndex].code}
                },
                ChangeBotCoroutine);
        }

        private void ChangeBotCoroutine()
        {
            BotListScene();
        }

        public void ShowBotNameInputWindow()
        {
            string text = _botList[_selectedBotIndex].name;

            changeUsernameDialog.Show("ВВЕДИТЕ НОВОЕ НАЗВАНИЕ БОТА", text, () => { }, (s) =>
            {
                if (s.Length > 0)
                    ChangeBotName(s);
            });
        }

        public void AddNewBot()
        {
            SceneInfo.EditedBot = new Bot();
            SceneManager.LoadScene("BotEditorScene");
        }

        public void ShowDeleteConfirmDialog()
        {
            deleteConfirmDialog.Show(() => { }, DeleteBot);
        }

        public void ShowEditConfirmDialog()
        {
            editConfirmDialog.Show(() => { }, EditBot, DuplicateBot);
        }

        private void BotInfoScene(BotMatchWrapper w)
        {
            Transform parent = botInfoScene.transform.Find("ScrollRect/Mask/MatchButtons/Image");

            Clear(parent);
            Transform buttonWrapper = parent.Find("MatchButton");

            botInfoScene.transform.Find("Header/Botname").GetComponent<Text>().text = w.bot.name;
            foreach (Match match in w.matches)
            {
                Transform inst = Instantiate(buttonWrapper, parent);
                Button b = inst.GetComponent<Button>();
                b.onClick.AddListener(() =>
                {
                    SceneInfo.Match = match;
                    SceneInfo.PrevScene = "BotListScene";
                    SceneManager.LoadScene("MatchPlayBackScene");
                });

                string bot = match.bot1.id == _botList[_selectedBotIndex].id ? match.bot2.name : match.bot1.name;
                string name = bot.Length > 7 ? bot.Substring(0, 5) + "..." : bot;

                string result = match.winnerBot.id == null ? "НИЧЬЯ" :
                    match.winnerBot.id == _botList[_selectedBotIndex].id ? "ПОБЕДА" : "ПОРАЖЕНИЕ";

                string text = name + ": " + result;

                b.transform.Find("Text").GetComponent<Text>().text = text;
                inst.gameObject.SetActive(true);
            }

            SetActive(botInfoScene);
        }
    }
}