using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using System;
using System.Text;

public class JokeHandler : MonoBehaviour
{
    [SerializeField]
    GameObject setupBubble;
    [SerializeField]
    TMP_Text setupText;
    [SerializeField]
    TMP_InputField punchlineInputField;
    [SerializeField]
    TMP_Text ratingText;
    [SerializeField]
    TMP_Text[] reactionTexts;
    [SerializeField]
    Transform reactionZoneParent;

    bool canSubmit;
    HashSet<char> allowedChars;
    List<Transform> reactionZones;


    private void Awake()
    {
        punchlineInputField.onValueChanged.AddListener(delegate { FilterInput(); });
        punchlineInputField.gameObject.SetActive(false);
        reactionZones = new List<Transform>();
        for (int i = 0; i < reactionZoneParent.childCount; i++)
        {
            reactionZones.Add(reactionZoneParent.GetChild(i));
        }
    }

    void FilterInput()
    {
        var currentValue = punchlineInputField.text;
        Debug.Log(currentValue);
        var newValueBuilder = new StringBuilder();
        foreach(var ch in currentValue)
        {
            if(allowedChars.Contains(ch))
            {
                newValueBuilder.Append(ch);
            }
        }
        punchlineInputField.text = newValueBuilder.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSubmit && Input.GetKeyDown(KeyCode.Return))
        {
            canSubmit = false;
            punchlineInputField.interactable = false;
            StartCoroutine(SubmitJoke());
        }
    }

    public void SetUpNewLevel(int number, Level level)
    {
        setupBubble.gameObject.SetActive(true);
        setupText.gameObject.SetActive(true);
        setupText.text = level.data.jokeSetup;
        canSubmit = false;
        punchlineInputField.gameObject.SetActive(true);
        punchlineInputField.interactable = false;
    }

    public void ShowPunchlineInput(HashSet<LETTER> allowedLetters)
    {
        HashSet<char> allowedChars = new HashSet<char> { '.','\'','?','!',',', ' ' };
        foreach(var l in allowedLetters)
        {
            var str = l.ToString();
            allowedChars.Add(str.ToUpper()[0]);
            allowedChars.Add(str.ToLower()[0]);
        }
        this.allowedChars = allowedChars;
        punchlineInputField.gameObject.SetActive(true);
        punchlineInputField.interactable = true;
        punchlineInputField.ActivateInputField();
        canSubmit = true;
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

        punchlineInputField.gameObject.SetActive(false);
        punchlineInputField.interactable = true;
        canSubmit = true;

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
            reactionTexts[i].text = reactions[i];
            reactionTexts[i].transform.position = reactionZones[i].transform.position;
        }

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
