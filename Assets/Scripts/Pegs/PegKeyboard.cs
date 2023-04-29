using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegKeyboard : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> letters;

    public void ShowLetter(LETTER letter)
    {
        int letterIndex = (int)letter;
        letters[letterIndex].gameObject.SetActive(true);
    }

    public void ResetKeyboard()
    {
        letters.ForEach(it => it.gameObject.SetActive(false));
    }
}
