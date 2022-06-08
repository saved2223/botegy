using System.Collections.Generic;
using Dialog;
using Entity;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppSceneManager
{
    public class UserSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainScene;
        [SerializeField] private GameObject userScene;

        [SerializeField] private GameObject changePasswordScene;
        [SerializeField] private GameObject gameInfoScene;
        [SerializeField] private TextInputWindow changeUsernameDialog;
        private RequestProcessor _processor;
        private GameObject _active;

        private void Awake()
        {
            _processor = gameObject.AddComponent<RequestProcessor>();
            _active = mainScene;

            BuildMainScene();
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

        /* main scene */
        public void MainScene()
        {
            BuildMainScene();
            SetActive(mainScene);
        }

        private void BuildMainScene()
        {
            Text t = mainScene.transform.Find("LayoutParent/UserName/Button/Text").GetComponent<Text>();
            t.text = SceneInfo.CurrUser.nickname;
        }


        public void GameInfoScene()
        {
            SetActive(gameInfoScene);
        }

        /* user scene */
        public void UserScene()
        {
            BuildUserScene();
            SetActive(userScene);
        }

        private void BuildUserScene()
        {
            userScene.transform.Find("LayoutParent/Username/Username").GetComponent<Text>().text =
                SceneInfo.CurrUser.nickname;
            userScene.transform.Find("LayoutParent/Email").GetComponent<Text>().text = SceneInfo.CurrUser.email;
            userScene.transform.Find("LayoutParent/ChangePassword").gameObject
                .SetActive(SceneInfo.CurrUser.isGoogle == 0);
        }

        /* change user info */
        public void ChangePasswordScene()
        {
            ShowMessage(false);
            SetActive(changePasswordScene);
        }

        public void ChangePassword()
        {
            string oldPass = changePasswordScene.transform.Find("LayoutParent/OldPasswordInputField")
                .GetComponent<InputField>().text;
            string newPass = changePasswordScene.transform.Find("LayoutParent/NewPasswordInputField")
                .GetComponent<InputField>().text;
            _processor.GeneratePostRequest<User>("updatePass",
                new Dictionary<string, string>()
                {
                    {"userId", SceneInfo.CurrUser.id},
                    {"pass", newPass},
                    {"oldPass", oldPass}
                },
                ChangeUserCoroutine);
        }

        public void BotListScene()
        {
            SceneManager.LoadScene("BotListScene");
        }

        private void ChangeUserName(string username)
        {
            _processor.GeneratePutRequest<User>("updateNick",
                new Dictionary<string, string>()
                {
                    {"userId", SceneInfo.CurrUser.id},
                    {"nick", RequestProcessor.PrepareName(username)}
                },
                ChangeUserCoroutine);
        }

        private void ChangeUserCoroutine(string json)
        {
            User user = Serializer.DeserializeObject<User>(json);

            if (user == null)
            {
                ShowMessage(true);
            }
            else
            {
                SceneInfo.CurrUser = user;
                UserScene();
            }
        }

        public void ShowUserNameInputWindow()
        {
            string text = SceneInfo.CurrUser.nickname;

            changeUsernameDialog.Show("ВВЕДИТЕ НОВОЕ ИМЯ ПОЛЬЗОВАТЕЛЯ", text, () => { }, (s) =>
            {
                if (s.Length > 0)
                    ChangeUserName(s);
            });
        }

        private void ShowMessage(bool show)
        {
            Text t = changePasswordScene.transform.Find("LayoutParent/Message").GetComponent<Text>();

            Color c = t.color;

            c.a = show ? 1f : 0f;
            t.color = c;
        }

        public void LogOut()
        {
            if (SceneInfo.CurrUser.isGoogle == 1)
                GoogleSignIn.DefaultInstance.SignOut();
            
            SceneInfo.CurrUser = null;
            SceneManager.LoadScene("StartScene");
        }

        public void LoadSearchScene()
        {
            SceneInfo.SearchPrevScene = "UserScene";
            SceneManager.LoadScene("SearchScene");
        }
    }
}