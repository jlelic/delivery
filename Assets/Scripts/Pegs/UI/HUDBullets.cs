using UnityEngine;
using UnityEngine.UI;

public class HUDBullets : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text bulletsText;
    [SerializeField] private Text maxBulletsText;

    [Header("Values")]
    [SerializeField] private IntegerVariable bulletCount;
    [SerializeField] private IntegerVariable maxBulletCount;

    public void OnBulletCountChange()
    {
        bulletsText.text = $"{bulletCount.value}";

    }

    public void OnMaxBulletCountChange()
    {
        maxBulletsText.text = $"{maxBulletCount.value}";
    }
}
