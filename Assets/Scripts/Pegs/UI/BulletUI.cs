using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    [SerializeField] private IntegerVariable bulletCount;
    [SerializeField] private Text bulletText;

    private void Start()
    {
        OnBulletCountChange();
    }

    public void OnBulletCountChange()
    {
        bulletText.text = $"{bulletCount.value}";
    }
}
