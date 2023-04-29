using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PowerupItemUI : MonoBehaviour
{
    [SerializeField] private Sprite maxBulletCount;
    [SerializeField] private Sprite scaleBullet;

    public void RenderPowerUp(POWER_UP powerUp)
    {
        Image image = GetComponent<Image>();
        switch (powerUp)
        {
            case POWER_UP.BULLET_MAX_MODIFIER:
                image.sprite = maxBulletCount;
                return;
            case POWER_UP.BULLET_SCALE:
                image.sprite = scaleBullet;
                return;
            default:
                return;
        }
    }
}
