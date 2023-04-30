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
    Transform reactionZoneParent;
    [SerializeField]
    TMP_Text enterToContinue;
    [SerializeField]
    PegKeyboard pegKeyboard;
    [SerializeField]
    GameObject pegBoard;

    bool canSubmit;
    bool canContinue;
    HashSet<char> allowedChars;
    List<Transform> reactionZones;
    RectTransform setupRectTransform;
    RectTransform punchlineRectTransform;
    RectTransform keyboardRectTransform;
    Vector2 keyboardOriginalPosition;
    Vector2 setupTargetPosition;
    Vector2 punchlineTargetPosition;
    Vector3 pegBoardTargetPosition;
    Image setupImage;
    Image punchlineImage;

    private void Awake()
    {
        punchlineInputField.onValueChanged.AddListener(delegate { FilterInput(); });
        punchlineInputField.onDeselect.AddListener(delegate { punchlineInputField.ActivateInputField(); });
        punchlineInputField.gameObject.SetActive(false);
        reactionZones = new List<Transform>();
        for (int i = 0; i < reactionZoneParent.childCount; i++)
        {
            reactionZones.Add(reactionZoneParent.GetChild(i));
        }
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
        setupImage = setupBubble.GetComponent<Image>();
        punchlineImage = punchlineInputField.GetComponent<Image>();
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (canSubmit)
            {
                canSubmit = false;
                punchlineInputField.interactable = false;
                pegKeyboard.StopScanning();
                LeanTween.moveY(keyboardRectTransform, -200, 1f).setEase(LeanTweenType.linear);
                enterToContinue.gameObject.SetActive(false);
                StartCoroutine(SubmitJoke());
            }
            else if (canContinue)
            {
                enterToContinue.gameObject.SetActive(false);
                ratingText.text = "";
                Utils.TweenColor(punchlineImage, Utils.ClearWhite);
                Utils.TweenColor(punchlineText, Utils.ClearWhite);
                Utils.TweenColor(setupImage, Utils.ClearWhite);
                Utils.TweenColor(setupText, Utils.ClearWhite);
                foreach (var t in reactionTexts)
                {
                    Utils.TweenColor(t, Utils.ClearWhite);
                }
                Utils.SetTimeout(this, 1.5f, () => GameManager.Instance.LoadNextLevel());
            }
        }
    }

    public void SetUpNewLevel(Level level)
    {
        setupBubble.gameObject.SetActive(true);
        setupImage.color = Color.white;
        setupText.color = Color.black;
        setupRectTransform.anchoredPosition = setupTargetPosition - new Vector2(100, 0);
        LeanTween.move(setupRectTransform, setupTargetPosition, 0.7f);
        pegBoard.transform.position = pegBoardTargetPosition + new Vector3(0, 10, 0);
        setupText.gameObject.SetActive(true);
        setupText.text = "";
        StartCoroutine(SlowlyFillText(setupText, level.data.jokeSetup, 0.7f, () =>
        {
            LeanTween.move(pegBoard, pegBoardTargetPosition, 1f);
            Utils.SetTimeout(this, 1f, () => pegKeyboard.ShowKeyboard());
            Utils.SetTimeout(this, 2f, () => GameManager.Instance.pegGameManager.Enable());
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
        LeanTween.move(pegBoard, pegBoardTargetPosition + new Vector3(0, 15, 0), 1f);
        LeanTween.moveX(keyboardRectTransform, 0, 1).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveY(keyboardRectTransform, -80, 0.5f).setEase(LeanTweenType.linear);
        HashSet<char> allowedChars = new HashSet<char> { '.', '\'', '?', '!', ',', ' ' };
        foreach (var l in allowedLetters)
        {
            var str = l.ToString();
            allowedChars.Add(str.ToUpper()[0]);
            allowedChars.Add(str.ToLower()[0]);
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
        var identifier = SystemInfo.deviceUniqueIdentifier;
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var checksum = Djb2(String.Concat(setup, punchline, identifier, time));
        var payload = string.Format("{{\"setup\":\"{0}\",\"punchline\":\"{1}\",\"id\":\"{2}\",\"time\":\"{3}\",\"checksum\":\"{4}\",\"env\":\"{5}\"}}",
            setup,
            punchline,
            identifier,
            time,
            checksum,
            "dev"
        );
        Debug.Log(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(payload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        //        yield break;
        yield return request.SendWebRequest();

        Debug.Log("Status Code: " + request.responseCode);
        int rating = 5;
        string[] reactions = new string[] { "eyy", "huh", "hmmm" };
        if (request.result != UnityWebRequest.Result.Success)
        {
            // Improvise
            yield break;
        }
        else
        {
            var responseBody = request.downloadHandler.text;
            Debug.Log(responseBody);
            var split = responseBody.Split(';');
            Debug.Log(split[0]);
            rating = (int)float.Parse(split[0]);
            reactions[0] = split[1];
            reactions[1] = split[2];
            reactions[2] = split[3];
        }

        ratingText.text = string.Format("{0}/10", rating);

        Shuffle(reactionZones);
        for (int i = 0; i < reactionTexts.Length; i++)
        {
            reactionTexts[i].color = Color.white;
            reactionTexts[i].text = reactions[i];
            reactionTexts[i].transform.position = reactionZones[i].transform.position;
        }

        Utils.SetTimeout(this, 2, () =>
          {
              canContinue = true;
              enterToContinue.text = "Press Enter to continue";
              enterToContinue.gameObject.SetActive(true);
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

    IEnumerator SlowlyFillText(TMP_Text tmpText, string text, float delay = 0f, Action callback = null, float speed = 0.1f)
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
}
