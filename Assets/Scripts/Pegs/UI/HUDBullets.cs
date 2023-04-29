using UnityEngine;
using UnityEngine.UI;

public class HUDBullets : MonoBehaviour
{
    [SerializeField] private IntegerVariable bulletCount;

    private Text bulletText;

    private void Start()
    {
        bulletText = GetComponent<Text>();
        OnBulletCountChange();
    }

    public void OnBulletCountChange()
    {
        bulletText.text = $"{bulletCount.value}";
    }
}
