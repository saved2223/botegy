using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppSceneManager
{
    public class SearchSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject searchScene;
        [SerializeField] private GameObject matchPlayBackScene;
        [SerializeField] private GameObject userInfoScene;
        [SerializeField] private GameObject botInfoScene;
    
        [SerializeField] private BotListDialog botListDialog;
        [SerializeField] private MatchResultDialog matchResultDialog;

        private GameObject active;
        private List<Bot> botList;
        private User user;
        private List<Match> matchList;
        private Match match;

        private const string URL = "https://botegy.herokuapp.com/";

        private void Awake()
        {
            active = searchScene;
        }
        /* scene utils */
        private void SetActive(GameObject target)
        {
            active.SetActive(false);
            target.SetActive(true);
            active = target;
        }
    
        private void Clear(Transform parent)
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
    
        private void MatchPlayBackScene(string matchId, string prevBotId, string prevBotName)
        {
            BuildMatchScene(matchId, prevBotId, prevBotName);
            SetActive(matchPlayBackScene);
        }

        private void UserInfoScene(string userId, string userName)
        {
            BuildUserInfoScene(userId, userName);
            SetActive(userInfoScene);
        }
    
        private void BotInfoScene(string botId, string botName)
        {
            BuildBotInfoScene(botId, botName);
            SetActive(botInfoScene);
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene("Scenes/StartScene");
        }
    
        /* building scenes with info from requests */
        public void Search()
        {
            string input = searchScene.transform.Find("SearchParent/SearchInputField").GetComponent<InputField>().text;
        
            // GenerateRequest(String.Format(URL + "/search?q={0}", input), SerializeBotList);
        
            Transform parent = searchScene.transform.Find("ScrollRect/Mask/BotButtons");
            Transform buttonWrapper = parent.Find("Image");

            Clear(parent);
        
            foreach (Bot b in botList)
            {
                Transform button = Instantiate(buttonWrapper, parent);
            
                button.Find("BotButton/Text").GetComponent<Text>().text = b.name;
                button.Find("BotButton").GetComponent<Button>().onClick.AddListener(() => BotInfoScene(b.id, b.name));
                button.name = "Image";
            
                button.gameObject.SetActive(true);
            }
        }

        private void BuildMatchScene(string matchId, string prevBotId, string prevBotName)
        {
            // GenerateRequest(String.Format(URL + "/match?id={0}", matchId), SerializeMatch);
            string text = "МАТЧ " + match.bot1 + " И " + match.bot2;

            matchPlayBackScene.transform.Find("Header/MatchHeader").GetComponent<Text>().text = text;

            Button backButton = matchPlayBackScene.transform.Find("ButtonParent/BackButton").GetComponent<Button>();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => BotInfoScene(prevBotId, prevBotName));
        
            // get and insert animation

        }
        private void BuildUserInfoScene(string userId, string userName)
        {
            // GenerateRequest(String.Format(URL + "/bots?userid={0}", userId), SerializeBotList);
        
            userInfoScene.transform.Find("Header/Username").GetComponent<Text>().text = userName;
        
            Transform parent = userInfoScene.transform.Find("ScrollRect/Mask/BotButtons");
            Transform buttonWrapper = parent.Find("Image");

            Clear(parent);
        
            foreach (Bot bot in botList)
            {
                Transform inst = Instantiate(buttonWrapper, parent);
                inst.Find("BotButton/Text").GetComponent<Text>().text = bot.name;
                inst.Find("BotButton").GetComponent<Button>()
                    .onClick.AddListener(() => BotInfoScene(bot.id, bot.name));
            
                inst.gameObject.SetActive(true);

            }


            if (CurrentUser.CurrUser != null && CurrentUser.CurrUser.isModer == 1)
            {
                userInfoScene.transform.Find("Footer").gameObject.SetActive(true);
                Button moderButton = userInfoScene.transform.Find("Footer/Image/ModerButton").GetComponent<Button>();
                Button banButton = userInfoScene.transform.Find("Footer/Image/BanButton").GetComponent<Button>();
            
                moderButton.onClick.RemoveAllListeners();
                banButton.onClick.RemoveAllListeners();
            
                //add button to delete from moders
                moderButton.onClick.AddListener(() => MakeUserModer(userId));
                banButton.onClick.AddListener(() => BanUser(userId));
            }
            else
            {
                userInfoScene.transform.Find("Footer").gameObject.SetActive(true);
            }
        }
        private void BuildBotInfoScene(string botId, string botName)
        {
            //query for all bot info, serialize user, bot, match

            botInfoScene.transform.Find("Header/Botname").GetComponent<Text>().text = botName;
            Transform parent = botInfoScene.transform.Find("ScrollRect/Mask/MatchButtons");

            Button userButton = botInfoScene.transform.Find("Header/Image/UserButton").GetComponent<Button>();
            userButton.GetComponentInChildren<Text>().text = user.nickname;
            userButton.onClick.RemoveAllListeners();
            userButton.onClick.AddListener(() => UserInfoScene(user.id, user.nickname));

            Clear(parent);
            Transform buttonWrapper = botInfoScene.transform.Find("ScrollRect/Mask/MatchButtons/Image");

            foreach (Match match in matchList)
            {
                Transform inst = Instantiate(buttonWrapper, parent);
                Button b = inst.Find("MatchButton").GetComponent<Button>();
                b.onClick.AddListener(() => MatchPlayBackScene(match.id, botId, botName));
            
            
                //change according to the query data
                string bot = match.bot1 == botId ? match.bot2 : match.bot1;
                string result = match.winnerBot == null ? "НИЧЬЯ" : match.winnerBot == botId ? "ПОБЕДА" : "ПОРАЖЕНИЕ";
            
                string text = bot + ": " + result;

                b.transform.Find("Text").GetComponent<Text>().text = text;
                inst.gameObject.SetActive(true);
            }


            if (CurrentUser.CurrUser != null && CurrentUser.CurrUser.LoggedIn)
            {
                Transform matchWrapper = botInfoScene.transform.Find("Footer/Image");
                matchWrapper.gameObject.SetActive(true);
                Button b = matchWrapper.Find("MatchButton").GetComponent<Button>();
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(() => NewMatch(botId, botName));
            }

        }

        public void NewMatch(string targetBotId, string targetBotName)
        {
            // GenerateRequest(String.Format(URL + "/botList?user={0}", CurrentUser.CurrUser.id), SerializeBotList);

            string selectedBotId = "";
        
            botListDialog.Show(botList, () => { }, (s) =>
            {
                AddNewMatch(targetBotId, targetBotName, s);    
            });
        }
        private void AddNewMatch(string targetId, string targetBotName, string botId)
        {
            // GenerateRequest(String.Format(URL + "/match?bot1={0}&bot2={1}", targetId, botId), SerializeMatch);
            ShowResults(targetId, targetBotName);
        }

        private void ShowResults(string targetId, string targetBotName)
        {
            string text = "МАТЧ " + match.bot1 + " И " + match.bot2 + " ПОБЕДИТЕЛЬ: " + match.winnerBot;
            matchResultDialog.Show(text, () => { }, () =>
            {
                MatchPlayBackScene(match.id, targetId, targetBotName);
            });
        }

        public void MakeUserModer(string userId)
        {
            //get user by id?
            // GenerateRequest(String.Format(URL + "/doModer?userId={0}", userId), SerializeUser);
            GenerateRequest(URL + "/log", SerializeUser);
        }

        private void BanUser(string userId)
        {
            //get user by id?
            // GenerateRequest(String.Format(URL + "/banUser?userId={0}", userId), ()=>{});
        }
    
        /* networking */
    
        private void SerializeBotList(string jsonString)
        {
            // serialize string to botlist
        }

        private void SerializeMatchList(string jsonString)
        {
            //serialize string to matchlist
        }
    
        private void SerializeMatch(string jsonString)
        {
            //serialize string to match
        }
    
        private void SerializeUser(string jsonString)
        {
            User user = JsonUtility.FromJson<User>(jsonString);
            Debug.Log(user.nickname);
        }

    
        private void GenerateRequest(string uri, Action<string> onSuccess)
        {
            StartCoroutine(ProcessRequest(uri, onSuccess));
        }
        private IEnumerator ProcessRequest(string uri, Action<string> onSuccess)
        {
            Guid sameUuid = new Guid("83b68bdf-887f-43a1-85f4-c5fb48aeac63");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("email", "antogandon@mail.ru");
            dic.Add("pass", "123456");
            // dic.Add("nick", "dungeonmaster");

            using (UnityWebRequest request = UnityWebRequest.Post(uri, dic))
            {
                // request.SetRequestHeader("X-RapidAPI-Host", HOST);
            
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    while (!request.isDone)
                        yield return null;
                    byte[] result = request.downloadHandler.data;

                    onSuccess(System.Text.Encoding.Default.GetString(result));
                }
            }
        }
    }
}
