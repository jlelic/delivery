using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using System;
using System.Text;
using UnityEngine.UI;

public class JokeHandler : MonoBehaviour
{
    [SerializeField]
    GameObject setupBubble;
    [SerializeField]
    GameObject setupTail;
    [SerializeField]
    TMP_Text setupText;
    [SerializeField]
    TMP_InputField punchlineInputField;
    [SerializeField]
    TMP_Text punchlineText;
    [SerializeField]
    TMP_Text ratingText;
    [SerializeField]
    TMP_Text[] reactionTexts;
    [SerializeField]
    TMP_Text enterToContinue;
    [SerializeField]
    PegKeyboard pegKeyboard;
    [SerializeField]
    GameObject pegBoard;
    [SerializeField]
    AudienceManager audienceManager;
    [SerializeField]
    Jerry jerry;
    [SerializeField]
    ScoreBar scoreBar;
    [SerializeField]
    GameObject hint1;
    [SerializeField]
    GameObject hint2;
    [SerializeField]
    TMP_Text errorText;

    bool canSubmit;
    bool canContinue;
    bool doneTutorial;
    HashSet<char> allowedChars;
    RectTransform setupRectTransform;
    RectTransform punchlineRectTransform;
    RectTransform keyboardRectTransform;
    RectTransform scoreBarRectTransform;
    Vector2 keyboardOriginalPosition;
    Vector2 setupTargetPosition;
    Vector2 punchlineTargetPosition;
    Vector2 scoreBarTargetPosition;
    Vector3 pegBoardTargetPosition;
    Image setupImage;
    SpriteRenderer setupTailImage;
    Image punchlineImage;
    Guid currentGameId;
    int availableLettersCount;

    string[] offlineReactions = new string[]{
                "hahaha", "meh", "good one!", "not bad", "clever!", "nice try", "almost", "what a punchline", "seen better", "ouch", "interesting"
            };

    private void Awake()
    {
        punchlineInputField.onValueChanged.AddListener(delegate { FilterInput(); });
        punchlineInputField.onDeselect.AddListener(delegate { punchlineInputField.ActivateInputField(); });
        punchlineInputField.gameObject.SetActive(false);
    }

    private void Start()
    {
        keyboardRectTransform = pegKeyboard.GetComponent<RectTransform>();
        keyboardOriginalPosition = keyboardRectTransform.anchoredPosition;
        setupRectTransform = setupBubble.GetComponent<RectTransform>();
        setupTargetPosition = setupRectTransform.anchoredPosition;
        pegBoardTargetPosition = pegBoard.transform.position;
        punchlineRectTransform = punchlineInputField.GetComponent<RectTransform>();
        punchlineTargetPosition = punchlineRectTransform.anchoredPosition;
        scoreBarRectTransform = scoreBar.GetComponent<RectTransform>();
        scoreBarTargetPosition = scoreBarRectTransform.anchoredPosition;
        setupImage = setupBubble.GetComponent<Image>();
        setupTailImage = setupTail.GetComponent<SpriteRenderer>();
        punchlineImage = punchlineInputField.GetComponent<Image>();
        scoreBar.gameObject.SetActive(false);
        setupBubble.gameObject.SetActive(false);
        hint1.gameObject.SetActive(false);
        hint2.gameObject.SetActive(false);
        errorText.transform.parent.gameObject.SetActive(false);
        foreach (var t in reactionTexts)
        {
            t.gameObject.SetActive(false);
        }

        JokeDatabase.Reset();
    }

    void FilterInput()
    {
        var currentValue = punchlineInputField.text;
        Debug.Log(currentValue);
        var newValueBuilder = new StringBuilder();
        foreach (var ch in currentValue)
        {
            if (allowedChars.Contains(ch))
            {
                newValueBuilder.Append(ch);
            }
        }
        punchlineInputField.text = newValueBuilder.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (canSubmit)
            {
                errorText.transform.parent.gameObject.SetActive(false);
                canSubmit = false;
                punchlineInputField.interactable = false;
                pegKeyboard.StopScanning();
                LeanTween.moveY(keyboardRectTransform, -200, 1f).setEase(LeanTweenType.linear);
                scoreBar.StartLoading();
                LeanTween.move(scoreBarRectTransform, scoreBarTargetPosition, 1f).setEase(LeanTweenType.easeInBounce);
                enterToContinue.gameObject.SetActive(false);
                hint2.gameObject.SetActive(false);
                jerry.Talk();
                StartCoroutine(SubmitJoke());
            }
            else if (canContinue)
            {
                enterToContinue.gameObject.SetActive(false);
                Utils.TweenColor(ratingText, Utils.ClearWhite);
                Utils.TweenColor(punchlineImage, Utils.ClearWhite);
                Utils.TweenColor(punchlineText, Utils.ClearWhite);
                Utils.TweenColor(setupImage, Utils.ClearWhite);
                Utils.TweenColor(setupText, Utils.ClearWhite);
                Utils.TweenColor(setupTailImage, Utils.ClearWhite);
                foreach (var t in reactionTexts)
                {
                    if (t.gameObject.activeInHierarchy)
                    {
                        Utils.TweenColor(t, Utils.ClearWhite);
                    }
                }
                Utils.SetTimeout(this, 1.5f, () => GameManager.Instance.LoadNextLevel());
            }
        }
    }

    public void SetUpNewLevel(int number, Guid gameId)
    {
        currentGameId = gameId;
        JokeCategory category = number == 0 ? JokeCategory.Simple : JokeCategory.General;
        var jokeSetup = JokeDatabase.GetJokeSetup(category);
        jerry.Talk();
        audienceManager.Neutral();
        scoreBar.gameObject.SetActive(true);
        scoreBarRectTransform.anchoredPosition = scoreBarTargetPosition + 200 * Vector2.up;
        setupBubble.gameObject.SetActive(true);
        setupImage.color = Color.white;
        setupText.color = Color.black;
        setupRectTransform.anchoredPosition = setupTargetPosition - new Vector2(100, 0);
        LeanTween.move(setupRectTransform, setupTargetPosition, 0.7f);
        pegBoard.transform.position = pegBoardTargetPosition + new Vector3(0, 10, 0);
        setupText.gameObject.SetActive(true);
        setupText.text = "";
        StartCoroutine(SlowlyFillText(setupText, jokeSetup, 0.7f, () =>
        {
            LeanTween.move(pegBoard, pegBoardTargetPosition, 1f);
            Utils.SetTimeout(this, 1f, () => pegKeyboard.ShowKeyboard());
            Utils.SetTimeout(this, 2f, () => GameManager.Instance.pegGameManager.Enable());
            if (!doneTutorial)
            {
                hint1.gameObject.SetActive(true);
            }
            jerry.Think();
        }));
        canSubmit = false;
        punchlineInputField.gameObject.SetActive(false);
        punchlineImage.color = Color.white;
        punchlineInputField.interactable = false;
        punchlineText.color = Color.black;
        keyboardRectTransform.anchoredPosition = keyboardOriginalPosition;
    }

    public void ShowPunchlineInput(HashSet<LETTER> allowedLetters)
    {
        if (!doneTutorial)
        {
            doneTutorial = true;
            hint1.gameObject.SetActive(false);
            hint2.gameObject.SetActive(true);
        }
        LeanTween.move(pegBoard, pegBoardTargetPosition + new Vector3(0, 15, 0), 1f);
        LeanTween.moveX(keyboardRectTransform, 0, 1).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveY(keyboardRectTransform, -80, 0.5f).setEase(LeanTweenType.linear);
        HashSet<char> allowedChars = new HashSet<char> { '.', '\'', '?', '!', ',', ' ', '-', '"' };
        availableLettersCount = 0;
        foreach (var l in allowedLetters)
        {
            var str = l.ToString();
            allowedChars.Add(str.ToUpper()[0]);
            allowedChars.Add(str.ToLower()[0]);
            availableLettersCount++;
        }
        this.allowedChars = allowedChars;
        punchlineRectTransform.anchoredPosition = punchlineTargetPosition - new Vector2(0, 100);
        punchlineInputField.gameObject.SetActive(true);
        punchlineInputField.text = "";

        pegKeyboard.StartScanning();

        LeanTween.move(punchlineRectTransform, punchlineTargetPosition, 1);
        Utils.SetTimeout(this, 1, () =>
        {
            punchlineInputField.interactable = true;
            punchlineInputField.ActivateInputField();
            enterToContinue.text = "Press Enter to submit";
            enterToContinue.gameObject.SetActive(true);
            canSubmit = true;
        });
    }

    IEnumerator SubmitJoke()
    {
        var request = new UnityWebRequest("https://europe-west1-delivery-385208.cloudfunctions.net/rate-joke", "POST");
        var setup = setupText.text;
        var punchline = punchlineInputField.text;
        if (punchline.Length == 0)
        {
            punchline = "*The comedian stammers and doesn't say anything*";
        }
        var identifier = SystemInfo.deviceUniqueIdentifier;
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var checksum = Djb2(String.Concat(setup, punchline, identifier, time));
        var payload = string.Format("{{\"setup\":\"{0}\",\"punchline\":\"{1}\",\"id\":\"{2}\",\"time\":\"{3}\",\"checksum\":\"{4}\",\"platform\":\"{5}\",\"game\":\"{6}\",\"letters\":{7}}}",
            setup,
            punchline,
            identifier,
            time,
            checksum,
            Application.platform.ToString(),
            currentGameId.ToString(),
            availableLettersCount
        );
        Debug.Log(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(payload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        //yield return new WaitForSeconds(3);
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        int rating = 5;
        string[] reactions = new string[] { "eyy", "huh", "hmmm" };
        try
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception(request.error);
            }

            var responseBody = request.downloadHandler.text;
            Debug.Log(responseBody);
            var split = responseBody.Split(';');
            Debug.Log(split[0]);
            rating = (int)float.Parse(split[0]);
            reactions[0] = split[1];
            reactions[1] = split[2];
            reactions[2] = split[3];
        }
        catch(Exception e)
        {
            Debug.Log("ERROR WHILE GETTING RATING");
            Debug.LogError(e);
            errorText.transform.parent.gameObject.SetActive(true);
            if (request.error != null)
            {
                errorText.text = request.error.ToString();
            }
            var uniqueChars = CountUniqueCharacters(punchline);
            Debug.Log(uniqueChars);
            var ratio = (float)uniqueChars / Mathf.Clamp(punchline.Length, 6, 30);
            Debug.Log(ratio);
            rating = (int)Mathf.Clamp(Mathf.Round(ratio * 10) - 1.5f + UnityEngine.Random.value * 3, 0, 10);
            Debug.Log(rating);
            Shuffle(offlineReactions);
            reactions[0] = offlineReactions[0];
            reactions[1] = offlineReactions[2];
            reactions[2] = offlineReactions[1];
        }

        Utils.TweenColor(ratingText, Color.white);
        ratingText.text = string.Format("{0}/10", rating);
        audienceManager.React(rating);
        scoreBar.AddScore(rating);
        MusicMixer.instance.HandleRatingReceived(rating);

        Shuffle(reactionTexts);
        for (int i = 0; i < reactions.Length; i++)
        {
            reactionTexts[i].gameObject.SetActive(true);
            reactionTexts[i].color = Color.white;
            reactionTexts[i].text = reactions[i];
        }

        Utils.SetTimeout(this, 4, () =>
          {
              canContinue = true;
              enterToContinue.text = "Press Enter to continue";
              enterToContinue.gameObject.SetActive(true);
              jerry.Stop();
          });
    }

    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    IEnumerator SlowlyFillText(TMP_Text tmpText, string text, float delay = 0f, Action callback = null, float speed = 0.05f)
    {
        tmpText.text = "";
        yield return new WaitForSeconds(delay);
        foreach (var ch in text)
        {
            tmpText.text = tmpText.text + ch;
            yield return new WaitForSeconds(speed);
        }
        callback?.Invoke();
    }

    uint Djb2(string str)
    {
        //Debug.Log(str);
        uint hash = 7523;
        foreach (char c in str)
        {
            unchecked
            {
                //Debug.Log(c + " - " + (int)c);
                hash = ((hash << 5) + hash) + c; // hash * 33 + c
                //Debug.Log(hash);
            }
        }
        return hash;
    }

    private static int CountUniqueCharacters(string input)
    {
        HashSet<char> uniqueChars = new HashSet<char>();
        foreach (char c in input)
        {
            uniqueChars.Add(c);
        }
        return uniqueChars.Count;
    }
}
