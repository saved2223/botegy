using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppSceneManager
{
    public class StartSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject startScene;
        [SerializeField] private GameObject loginScene;
        [SerializeField] private GameObject signupScene;

        private RequestProcessor _processor;
        private GameObject _active;
        [SerializeField] private Animator animator;

        [SerializeField] private string webClientId;
        private FirebaseAuth _auth;
        private GoogleSignInConfiguration _configuration;

        private void Awake()
        {
            _processor = gameObject.AddComponent<RequestProcessor>();

            ClearInputField(loginScene);
            ClearInputField(signupScene);
            _active = startScene;
            
            GetConfig();
        }

        private void GetConfig()
        {
            _processor.GenerateGetRequest<bool>("isGoogleLogin", new Dictionary<string, string>() { }, ConfigCoroutine);
        }

        private void ConfigCoroutine(string json)
        {
            bool trigger = Boolean.Parse(json);
            startScene.transform.Find("LayoutParent/Buttons/GoogleImage").gameObject.SetActive(trigger);
            loginScene.transform.Find("LayoutParent/GoogleImage").gameObject.SetActive(trigger);
            signupScene.transform.Find("LayoutParent/GoogleImage").gameObject.SetActive(trigger);
        }

        /* scene utils */
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

        private void ClearInputField(GameObject scene)
        {
            InputField f1 = scene.transform.Find("LayoutParent/EmailInputField").GetComponent<InputField>();
            InputField f2 = scene.transform.Find("LayoutParent/PasswordInputField").GetComponent<InputField>();

            InputField f3 = null;

            Transform t = scene.transform.Find("LayoutParent/UsernameInputField");
            if (t != null)
                f3 = t.GetComponent<InputField>();

            f1.text = "";
            f2.text = "";

            if (f3 != null)
                f3.text = "";

            ShowMessage(scene, false);
        }

        public void NoLoginButtonScript()
        {
            SceneInfo.CurrUser = null;
            SceneInfo.SearchPrevScene = "StartScene";
            AppMetrica.Instance.ReportEvent("WithOutLoginButtonClicked");
            LoadSearchScene();
        }

        public void LoadSearchScene()
        {
            SceneManager.LoadScene("SearchScene");
        }

        /* start scene */
        public void StartScene()
        {
            SetActive(startScene);
        }


        /* login scene */
        public void LoginScene()
        {
            SetActive(loginScene);
            animator.SetTrigger("start_open");
            animator.SetTrigger("login_open");
        }

        public void Login()
        {
            string login = loginScene.transform.Find("LayoutParent/EmailInputField").GetComponent<InputField>().text;
            string pass = loginScene.transform.Find("LayoutParent/PasswordInputField").GetComponent<InputField>().text;

            _processor.GeneratePostRequest<User>("log",
                new Dictionary<string, string>()
                {
                    {"email", login},
                    {"pass", pass}
                },
                LoginCoroutine);
        }

        private void LoginCoroutine(string s)
        {
            SceneInfo.CurrUser = Serializer.DeserializeObject<User>(s);

            if (SceneInfo.CurrUser?.nickname == null)
                ShowMessage(loginScene, true);
            else
            {
                SceneInfo.CurrUser.LoggedIn = true;
                SceneManager.LoadScene("UserScene");
            }
        }

        /* sign up scene */
        public void SignUpScene()
        {
            animator.SetTrigger("start_open");
            animator.SetTrigger("signup_open");
            SetActive(signupScene);
        }

        public void Signup()
        {
            string login = signupScene.transform.Find("LayoutParent/EmailInputField").GetComponent<InputField>().text;
            string pass = signupScene.transform.Find("LayoutParent/PasswordInputField").GetComponent<InputField>().text;
            string username = signupScene.transform.Find("LayoutParent/UsernameInputField").GetComponent<InputField>()
                .text;

            _processor.GeneratePostRequest<User>("logUp",
                new Dictionary<string, string>()
                {
                    {"email", login},
                    {"pass", pass},
                    {"nick", RequestProcessor.PrepareName(username)}
                },
                SignUpCoroutine);
        }

        private void SignUpCoroutine(string s)
        {
            SceneInfo.CurrUser = Serializer.DeserializeObject<User>(s);

            if (SceneInfo.CurrUser?.nickname == null)
            {
                ShowMessage(signupScene, true);
            }
            else
            {
                SceneInfo.CurrUser.LoggedIn = true;
                AppMetrica.Instance.ReportEvent("SuccessfulSignUp");

                SceneManager.LoadScene("UserScene");
            }
        }

        private void ShowMessage(GameObject scene, bool show)
        {
            Text t = scene.transform.Find("LayoutParent/Message").GetComponent<Text>();

            Color c = t.color;

            c.a = show ? 1f : 0f;
            t.color = c;
        }

        public void GoogleLogin()
        {
            
            
            _configuration = new GoogleSignInConfiguration
                {WebClientId = webClientId, RequestEmail = true, RequestIdToken = true};
            CheckFirebaseDependencies();


            OnSignIn();
        }

        private void CheckFirebaseDependencies()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result == DependencyStatus.Available)
                    {
                        _auth = FirebaseAuth.DefaultInstance;
                    }
                }
            });
        }

        private void OnSignIn()
        {
            GoogleSignIn.Configuration = _configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }

        private void OnSignOut()
        {
            GoogleSignIn.DefaultInstance.SignOut();
        }

        public void OnDisconnect()
        {
            GoogleSignIn.DefaultInstance.Disconnect();
        }

        private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                return;
            }
            GoogleSignInRequest(task);
        }

        private void GoogleSignInRequest(Task<GoogleSignInUser> task)
        {
            _processor.GeneratePostRequest<User>("/authGoogle", new Dictionary<string, string>()
            {
                {"email", task.Result.Email},
                {"nick", task.Result.DisplayName}
            }, GoogleCoroutine);
        }

        private void GoogleCoroutine(string json)
        {
            User user = Serializer.DeserializeObject<User>(json);
            
            if (user != null)
            {
                SceneInfo.CurrUser = user;
                SceneInfo.CurrUser.LoggedIn = true;

                AppMetrica.Instance.ReportEvent("SuccessfulSignUp");
                SceneManager.LoadScene("UserScene");
            }
        }

        private void SignInWithGoogleOnFirebase(string idToken)
        {
            Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

            _auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                AggregateException ex = task.Exception;
                if (ex != null)
                {
                    if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                        Debug.Log(inner.ErrorCode + " " + inner.Message);
                }
            });
        }
    }
}