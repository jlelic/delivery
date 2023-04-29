using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private IntegerVariable maxBullets;
    [SerializeField] private IntegerVariable currentBullets;

    private void Awake()
    {
        maxBullets.SetValue(level.maxBullets);
        currentBullets.SetValue(level.maxBullets);
    }
}
