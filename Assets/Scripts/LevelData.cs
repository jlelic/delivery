using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class LevelData : ScriptableObject
{
    [SerializeField] public string jokeSetup;
    [SerializeField] public int maxBullets;
}
