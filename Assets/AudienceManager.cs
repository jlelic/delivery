using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    AudienceMember[] members;
    [SerializeField]
    Sprite[] frontSprites;
    [SerializeField]
    Sprite[] sideSprites;

    private void Start()
    {
        members = FindObjectsOfType<AudienceMember>();
        RandomizeAudience();
    }

    public void RandomizeAudience()
    {
        var unusedFront = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        var unusedSide = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        foreach (var m in members)
        {
            var isFront = m.position == AudienceMemberPosition.Back;
            var unused = isFront ? unusedFront : unusedSide;
            var sprites = isFront ? frontSprites : sideSprites;
            var index = unused[UnityEngine.Random.Range(0, unused.Count)];
            unused.Remove(index);
            m.spriteSheet = new Sprite[10];
            Array.Copy(sprites, index * 10, m.spriteSheet, 0, 10);
        }
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
