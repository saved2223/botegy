using System.Collections.Generic;
using Entity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppSceneManager
{
    public class MatchPlayBackSceneManager : MonoBehaviour
    {
        [SerializeField] private Transform scene;

        private RequestProcessor _processor;
        private Match _match;

        private void Awake()
        {
            AppMetrica.Instance.SetStatisticsSending(true);
            _processor = gameObject.AddComponent<RequestProcessor>();

            MatchPlayBackScene();
        }

        private void MatchPlayBackScene()
        {
            _match = SceneInfo.Match;
            _processor.GenerateGetRequest<History>("getMatchHistory",
                new Dictionary<string, string>()
                {
                    {"matchId", _match.id}
                },
                MatchPlayBackCoroutine);
        }

        private void MatchPlayBackCoroutine(string json)
        {
            History h = Serializer.DeserializeObject<History>(json);

            BuildMatchScene(h);
        }

        private void BuildMatchScene(History h)
        {
            string text = "МАТЧ " + _match.bot1.name + " И " + _match.bot2.name;

            scene.Find("Header/MatchHeader").GetComponent<Text>().text = text;

            Button backButton = scene.Find("ButtonParent/GameObject/BackButton").GetComponent<Button>();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneInfo.PrevScene); });

            Text t = scene.Find("ScrollRect/Mask/Layout/MatchText").GetComponent<Text>();
            t.text = h.history;
        }
    }
}