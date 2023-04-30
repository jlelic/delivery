using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegKeyboard : MonoBehaviour
{
    Dictionary<char,PegKey> pegKeys;
    bool isInputActive;
    List<KeyCode> keyCodes;

    private void Start()
    {
        pegKeys = new Dictionary<char, PegKey>();
        keyCodes = new List<KeyCode>();
        for (int i = 0; i < 26; i++)
        {
            var child = transform.GetChild(i);
            var pegKey = child.GetComponent<PegKey>();
            pegKeys.Add(child.name[0], pegKey);

            KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), child.name);
            keyCodes.Add(keyCode);

            child.gameObject.SetActive(false);
        }
    }

    public void ShowLetter(LETTER letter)
    {
        //        int letterIndex = (int)letter;
        //       letters[letterIndex].gameObject.SetActive(true);
        pegKeys[letter.ToString()[0]].Activate();
    }

    public void ShowKeyboard()
    {
        foreach (var k in pegKeys)
        {
            Utils.SetTimeout(this, UnityEngine.Random.value, () =>
            {
                k.Value.gameObject.SetActive(true);
            });
        }
    }

    public void ResetKeyboard()
    {
        //     letters.ForEach(it => it.gameObject.SetActive(false));
        foreach (var k in pegKeys)
        {
            Utils.SetTimeout(this, UnityEngine.Random.value, () =>
             {
                 k.Value.Deactivate();
             });
        }
    }

    public void StartScanning()
    {
        isInputActive = true;
    }

    public void StopScanning()
    {
        isInputActive = false;
    }

    private void Update()
    {
        if(isInputActive)
        {
            foreach(var kc in keyCodes)
            {
                if(Input.GetKeyDown(kc))
                {
                    pegKeys[kc.ToString()[0]].ShowFeedback();
                }
            }
        }
    }
}
