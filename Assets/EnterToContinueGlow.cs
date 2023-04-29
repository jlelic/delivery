using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterToContinueGlow : MonoBehaviour
{
    TMPro.TMP_Text text;
    bool goingUp;
    float value = 1;
    float diff = 0.001f;

    void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            value += diff;
            if (value >= 1)
            {
                value = 1;
                goingUp = false;
            }
        }
        else
        {
            value -= diff;
            if (value <= 0.3f)
            {
                value = 0.3f;
                goingUp = true;
            }
        }

        text.color = new Color(value, value, value);

    }
}
