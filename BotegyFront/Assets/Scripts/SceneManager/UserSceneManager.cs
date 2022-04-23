using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject startScene;
    [SerializeField] private GameObject mainScene;
    [SerializeField] private GameObject userScene;
    [SerializeField] private GameObject loginScene;
    [SerializeField] private GameObject signupScene;
    [SerializeField] private GameObject botListScene;
    [SerializeField] private GameObject changePasswordScene;

    [SerializeField] private DeleteConfirmDialog deleteConfirmDialog;
    [SerializeField] private EditConfirmDialog editConfirmDialog;
    [SerializeField] private TextInputWindow changeUsernameDialog;
    
    
    private GameObject active;

    private List<Bot> botList;
    private int selectedBotIndex;

    private const string URL = "";
    private const string HOST = "";
    private string response;
    
    private void Awake()
    {
        // <- get user from memory
        
        active = startScene;

        if (CurrentUser.CurrUser != null && CurrentUser.CurrUser.LoggedIn)
            MainScene();
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

    /* showing scenes */
    public void StartScene()
    {
        SetActive(startScene);
    }
    public void MainScene()
    {
        BuildMainScene();
        SetActive(mainScene);
    }
    public void LoginScene()
    {
        SetActive(loginScene);
    }
    public void SignUpScene()
    {
       SetActive(signupScene);
    }
    public void BotListScene()
    { 
        BuildBotListScene();
        SetActive(botListScene);
    }

    public void UserScene()
    {
        BuildUserScene();
        SetActive(userScene);
    }
    
    public void ChangePasswordScene()
    {
        SetActive(changePasswordScene);
    }

    public void NoLoginButtonScript()
    {
        CurrentUser.CurrUser = null;
        LoadSearchScene();
    }
    
    public void LoadSearchScene()
    {
        var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
        Scene s = SceneManager.LoadScene("Scenes/SearchScene", parameters);
    }
    
    /* scenes building based on info */
    
    private void BuildMainScene()
    {
        Text t = mainScene.transform.Find("LayoutParent/UserName/Button/Text").GetComponent<Text>();
        t.text = CurrentUser.CurrUser.nickname;
    }

    private void BuildUserScene()
    {
        userScene.transform.Find("LayoutParent/Username/Username").GetComponent<Text>().text = CurrentUser.CurrUser.nickname;
        userScene.transform.Find("LayoutParent/Email").GetComponent<Text>().text = CurrentUser.CurrUser.email;
    }

    private void BuildBotListScene()
    {
        // GenerateRequest(String.Format(URL + "/getBotsForPlayer?playerId={0}", CurrentUser.CurrUser.id), SerializeBotList);

        Transform parent = botListScene.transform.Find("SidePanel/ScrollRect/Image/ScrollPanel");
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
            botListScene.transform.Find("BotPanel").gameObject.SetActive(true);
        }
    }

    private void ShowBotCode(int index)
    {
        //GenerateRequest(String.Format(URL + "/bot?id={0}", botId), SerializeBotList);
        //response gives Bot list, getting code from file needed
        selectedBotIndex = index;
        
        botListScene.transform.Find("BotPanel/Header/BotName").GetComponent<Text>().text = botList[index].name;
        botListScene.transform.Find("BotPanel/ScrollRect/Mask/CodePanel/Text").
            GetComponent<Text>().text = botList[index].filePath; //code
    }
    
    /* login, logout, signup methods */

    public void Login()
    {
        string login = loginScene.transform.Find("LayoutParent/EmailInputField").GetComponent<InputField>().text;
        string pass = loginScene.transform.Find("LayoutParent/PasswordInputField").GetComponent<InputField>().text;

        // GenerateRequest(String.Format(URL + "/login?email={0}&pass={1}", login, pass), SerializeUser);
        // GenerateRequest("https://jsonplaceholder.typicode.com/users/1", SerializeUser);
    }
    
    public void Signup()
    {
        string login = loginScene.transform.Find("LayoutParent/EmailInputField").GetComponent<InputField>().text;
        string pass = loginScene.transform.Find("LayoutParent/PasswordInputField").GetComponent<InputField>().text;
        string username = loginScene.transform.Find("LayoutParent/UsernameInputField").GetComponent<InputField>().text;

        // GenerateRequest(String.Format(URL + "/logUp?email={0}&pass={1}&nick={2}", login, pass, username), SerializeUser);
    }

    public void LogOut()
    {
        CurrentUser.CurrUser = null;
        StartScene();
    }

    public void GoogleLogin()
    {
        
    }

    public void GoogleSignUp()
    {
        
    }
    

    /* user utils */
    
    private void ChangeUserName(string username)
    {
    //     GenerateRequest(String.Format(URL + "/updateNick?userId={0}&nick={1}",
    //     CurrentUser.CurrUser.id, username), SerializeUpdatedUser);
    } 
    
    
    public void ChangePassword()
    {
        string oldPass = loginScene.transform.Find("LayoutParent/OldPasswordInputField").GetComponent<InputField>().text;
        string newPass = loginScene.transform.Find("LayoutParent/NewPasswordInputField").GetComponent<InputField>().text;
        
        // GenerateRequest(String.Format(URL + "/updatePass?userId={0}&old={1}&new={2}", CurrentUser.CurrUser.id, oldPass, newPass), SerializeUpdatedUser);
        //need to give to url old pass
    }
    public void ShowUserNameInputWindow()
    {
        string text = CurrentUser.CurrUser.nickname;

        changeUsernameDialog.Show("ВВЕДИТЕ НОВОЕ ИМЯ ПОЛЬЗОВАТЕЛЯ", text, () => { }, (s) =>
        {
            if (s.Length > 0)
                ChangeUserName(s);
        });
    }
    
    /* bot utils */
    
    private void DeleteBot()
    {
        // GenerateRequest(String.Format(URL + "/deleteBot?botId={0}", botList[selectedBotIndex].id), BotListScene);
    }
    
    private void EditBot()
    {
        //Add static class or field to store bot code
        // add flag for saving in editor
        // e.g. id of a bot
        SceneManager.LoadScene("BotEditorScene");
    }

    private void DuplicateBot()
    {
        //Add static class or field to store bot code
        // add flag for saving in editor
        // e.g. id of a bot
        SceneManager.LoadScene("BotEditorScene");
    }

    private void ChangeBotName(string name)
    {
        //     GenerateRequest(String.Format(URL + "/updateBotName?botId={0}&name={1}",
        //     botList[selectedBoxIndex].id, name), SerializeBotList);
        //edit according to what is returned
        //maybe serialize new bot for list, not the whole list
    }

    public void ShowBotNameInputWindow()
    {
        string text = botList[selectedBotIndex].name;

        changeUsernameDialog.Show("ВВЕДИТЕ НОВОЕ НАЗВАНИЕ БОТА", text, () => { }, (s) =>
        {
            if (s.Length > 0)
                ChangeBotName(s);
        });
    }

    public void AddNewBot()
    {
        //Add static class or field to store bot code
        // add flag for saving in editor
        // e.g. id of a bot
        SceneManager.LoadScene("BotEditorScene");
    }
    public void ShowDeleteConfirmDialog()
    {
        deleteConfirmDialog.Show(() => { }, () => { DeleteBot(); });
    }
    
    public void ShowEditConfirmDialog()
    {
        editConfirmDialog.Show(() => { }, () => {EditBot();}, () => {DuplicateBot();});
    }
    
    /* networking */
    private void SerializeUser(string jsonString)
    {
        CurrentUser.CurrUser = JsonUtility.FromJson<User>(jsonString);
        
        if (CurrentUser.CurrUser != null)
            CurrentUser.CurrUser.LoggedIn = true;
        
        
        if (CurrentUser.CurrUser != null && CurrentUser.CurrUser.LoggedIn)
            MainScene();
    }

    private void SerializeUpdatedUser(string jsonString)
    {
        CurrentUser.CurrUser = JsonUtility.FromJson<User>(jsonString);

        //check what response has error
        if (CurrentUser.CurrUser != null)
            CurrentUser.CurrUser.LoggedIn = true;
        
        UserScene();
    }

    private void SerializeBotList(string jsonString)
    {
        // serialize string to botlist
    }

    private void SerializeBot(string jsonString)
    {
        //serialize string to Bot
        
        //selectedBot = serialized bot
    }
    
    
    private void GenerateRequest(string uri, Action onSuccess)
    {
        StartCoroutine(ProcessRequest(uri, onSuccess));
    }
    
    private void GenerateRequest(string uri, Action<string> onSuccess)
    {
        StartCoroutine(ProcessRequest(uri, onSuccess));
    }

    private IEnumerator ProcessRequest(string uri, Action onSuccess)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            // request.SetRequestHeader("X-RapidAPI-Host", HOST);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            
            else
            {
                onSuccess();
            }
        }
    }
    private IEnumerator ProcessRequest(string uri, Action<string> onSuccess)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
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

