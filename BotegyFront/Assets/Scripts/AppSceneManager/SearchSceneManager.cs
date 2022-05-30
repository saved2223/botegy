using System.Collections.Generic;
using Dialog;
using Entity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppSceneManager
{
    public class SearchSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject searchScene;
        [SerializeField] private GameObject userInfoScene;
        [SerializeField] private GameObject botInfoScene;

        [SerializeField] private BotListDialog botListDialog;
        [SerializeField] private MatchResultDialog matchResultDialog;

        [SerializeField] private Animator animator;

        private RequestProcessor _processor;

        private GameObject _active;
        private Bot _currBot;
        private User _currUser;

        private void Awake()
        {
            _active = searchScene;
            _processor = gameObject.AddComponent<RequestProcessor>();
        }

        /* scene utils */
        private void SetActive(GameObject target)
        {
            _active.SetActive(false);
            target.SetActive(true);
            _active = target;
        }

        private static void Clear(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child.gameObject.activeSelf)
                    Destroy(child.gameObject);
            }
        }

        /* calling scenes */
        public void SearchScene()
        {
            SetActive(searchScene);
        }

        private void BotInfoScene(Bot b)
        {
            _currBot = b;
            _processor.GenerateGetRequest<Match>("getMatchesForBot",
                new Dictionary<string, string>()
                {
                    {"botId", b.id}
                },
                BotInfoCoroutine);
        }

        private void UserInfoScene(User user)
        {
            _currUser = user;

            _processor.GenerateGetRequest<Bot>("getBotsForPlayer",
                new Dictionary<string, string>()
                {
                    {"playerId", user.id}
                },
                UserInfoCoroutine);
        }


        private void BotInfoCoroutine(string json)
        {
            BotMatchWrapper bmw = Serializer.DeserializeObject<BotMatchWrapper>(json);

            BuildBotInfoScene(bmw);
            SetActive(botInfoScene);
        }

        private void UserInfoCoroutine(string json)
        {
            List<Bot> bots = Serializer.DeserializeObjectList<Bot>(json);

            BuildUserInfoScene(bots);
            SetActive(userInfoScene);
        }


        private void BuildBotInfoScene(BotMatchWrapper bmw)
        {
            botInfoScene.transform.Find("Header/Botname").GetComponent<Text>().text = _currBot.name;
            Transform parent = botInfoScene.transform.Find("ScrollRect/Mask/MatchButtons/Image");

            Button userButton = botInfoScene.transform.Find("Header/Image/UserButton").GetComponent<Button>();
            userButton.GetComponentInChildren<Text>().text = _currBot.player.nickname;
            userButton.onClick.RemoveAllListeners();
            userButton.onClick.AddListener(() =>
            {
                animator.SetTrigger("bot_close");
                animator.SetTrigger("user_clicked");
                animator.SetTrigger("user_show");
                UserInfoScene(_currBot.player);
            });

            Clear(parent);
            Transform buttonWrapper = botInfoScene.transform.Find("ScrollRect/Mask/MatchButtons/Image/MatchButton");

            foreach (Match match in bmw.matches)
            {
                Transform inst = Instantiate(buttonWrapper, parent);
                Button b = inst.GetComponent<Button>();
                b.onClick.AddListener(() =>
                {
                    SceneInfo.PrevScene = "SearchScene";
                    SceneInfo.Match = match;
                    SceneManager.LoadScene("MatchPlayBackScene");
                });
                
                string bot = match.bot1.id == _currBot.id ? match.bot2.name : match.bot1.name;
                string result = match.winnerBot.id == null ? "НИЧЬЯ" :
                    match.winnerBot.id == _currBot.id ? "ПОБЕДА" : "ПОРАЖЕНИЕ";

                string text = bot + ": " + result;

                b.transform.Find("Text").GetComponent<Text>().text = text;
                inst.gameObject.SetActive(true);
            }


            if (SceneInfo.CurrUser != null && SceneInfo.CurrUser.LoggedIn)
            {
                Transform matchWrapper = botInfoScene.transform.Find("Footer/Image");
                matchWrapper.gameObject.SetActive(true);
                Button b = matchWrapper.Find("MatchButton").GetComponent<Button>();
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(NewMatch);
            }
        }

        /* building scenes with info from requests */
        public void Search()
        {
            string input = searchScene.transform.Find("SearchParent/SearchInputField").GetComponent<InputField>().text;

            _processor.GenerateGetRequest<Bot>("getBotsByName",
                new Dictionary<string, string>()
                {
                    {"botName", input}
                }, SearchCoroutine);
        }

        private void SearchCoroutine(string json)
        {
            List<Bot> bots = Serializer.DeserializeObjectList<Bot>(json);

            Transform parent = searchScene.transform.Find("ScrollRect/Mask/BotButtons");
            Transform buttonWrapper = parent.Find("Image");

            Clear(parent);

            foreach (Bot b in bots)
            {
                if (b.player.id != SceneInfo.CurrUser?.id)
                {
                    Transform button = Instantiate(buttonWrapper, parent);

                    button.Find("BotButton/Text").GetComponent<Text>().text = b.name;
                    button.Find("BotButton").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        animator.SetTrigger("search_close");
                        animator.SetTrigger("clicked");
                        animator.SetTrigger("bot_clicked");

                        BotInfoScene(b);
                    });
                    button.name = "Image";

                    button.gameObject.SetActive(true);
                }
            }
        }

        private void BuildUserInfoScene(List<Bot> bots)
        {
            userInfoScene.transform.Find("Header/Username").GetComponent<Text>().text = _currUser.nickname;

            Transform parent = userInfoScene.transform.Find("ScrollRect/Mask/BotButtons");
            Transform buttonWrapper = parent.Find("Image");

            Clear(parent);

            foreach (Bot bot in bots)
            {
                Transform inst = Instantiate(buttonWrapper, parent);
                inst.Find("BotButton/Text").GetComponent<Text>().text = bot.name;
                inst.Find("BotButton").GetComponent<Button>()
                    .onClick.AddListener(() =>
                    {
                        animator.SetTrigger("user_close");
                        animator.SetTrigger("user_hide");
                        animator.SetTrigger("bot_clicked");

                        BotInfoScene(bot);
                    });

                inst.gameObject.SetActive(true);
            }

            if (SceneInfo.CurrUser != null && SceneInfo.CurrUser.isModer == 1 && SceneInfo.CurrUser.id != _currUser.id)
            {
                userInfoScene.transform.Find("Footer").gameObject.SetActive(true);
                Button moderButton = userInfoScene.transform.Find("Footer/Image/ModerButton").GetComponent<Button>();
                Button banButton = userInfoScene.transform.Find("Footer/Image/BanButton").GetComponent<Button>();

                moderButton.onClick.RemoveAllListeners();
                banButton.onClick.RemoveAllListeners();


                userInfoScene.transform.Find("Footer/Image/ModerButton/Text").GetComponent<Text>().text
                    = _currUser.isModer == 1 ? "УБРАТЬ ИЗ МОДЕРАТОРОВ" : "НАЗНАЧИТЬ МОДЕРАТОРОМ";
                moderButton.onClick.AddListener(MakeUserModer);

                banButton.onClick.AddListener(BanUser);
            }
            else
            {
                userInfoScene.transform.Find("Footer").gameObject.SetActive(false);
            }
        }

        private void NewMatch()
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
            List<Bot> list = Serializer.DeserializeObjectList<Bot>(json);
            botListDialog.Show(list, () => { }, AddNewMatch);
        }

        private void AddNewMatch(string botId)
        {
            _processor.GenerateGetRequest<Match>("doFight",
                new Dictionary<string, string>()
                {
                    {"bot1Id", botId},
                    {"bot2Id", _currBot.id}
                },
                MatchCoroutine);
        }

        private void MatchCoroutine(string json)
        {
            Match m = Serializer.DeserializeObject<Match>(json);
            ShowResults(m);
        }

        private void ShowResults(Match match)
        {
            AppMetrica.Instance.ReportEvent("MatchFinished");


            string text = "МАТЧ " + match.bot1.name + " И " + match.bot2.name;

            if (match.winnerBot.id == null)
                text += " НИЧЬЯ";
            else
                text += " ПОБЕДИТЕЛЬ: " + match.winnerBot.name;

            matchResultDialog.Show(text, () => { }, () =>
            {
                AppMetrica.Instance.ReportEvent("MatchPlayBackButtonClicked");

                SceneInfo.Match = match;
                SceneInfo.PrevScene = "SearchScene";
                SceneManager.LoadScene("MatchPlayBackScene");
            });
        }

        private void MakeUserModer()
        {
            _processor.GeneratePutRequest<User>(_currUser.isModer == 1 ? "unDoModer" : "doModer",
                new Dictionary<string, string>()
                {
                    {"userId", _currUser.id}
                }, ModerCoroutine
            );
        }

        private void ModerCoroutine(string json)
        {
            UserInfoScene(Serializer.DeserializeObject<User>(json));
        }

        private void BanUser()
        {
            _processor.GeneratePutRequest<User>("ban",
                new Dictionary<string, string>()
                {
                    {"userId", _currUser.id}
                }, BanCoroutine
            );
        }

        private void BanCoroutine(string json)
        {
            animator.SetTrigger("user_close");
            animator.SetTrigger("close");
            Search();
        }

        public void BackButton()
        {
            if (SceneInfo.SearchPrevScene == "StartScene")
                AppMetrica.Instance.ReportEvent("ReturnToStartSceneButtonClicked");

            SceneManager.LoadScene(SceneInfo.SearchPrevScene);
        }
    }
}