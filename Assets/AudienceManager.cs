using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    AudienceMember[] members;

    private void Start()
    {
        members = FindObjectsOfType<AudienceMember>();
    }

    public void React(int score)
    {
        foreach (var m in members)
        {
            m.React(score);
        }
    }
    public void Neutral()
    {
        foreach (var m in members)
        {
            m.Neutral();
        }
    }
}
