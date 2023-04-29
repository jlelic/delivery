using System.Collections.Generic;
using UnityEngine;

public enum POWER_UP
{
    BULLET_MAX_MODIFIER,
    BULLET_SCALE,
    BULLET_GRAVITY,
}

public class PowerupUI : MonoBehaviour
{
    [SerializeField] private PowerupItemUI itemPrefab;

    private List<POWER_UP> collectedPowerUps = new List<POWER_UP>();

    public void OnBulletMaxModifierCollected()
    {
        collectedPowerUps.Add(POWER_UP.BULLET_MAX_MODIFIER);

        RerenderPowerups();
    }

    public void OnBulletScaleCollected()
    {
        collectedPowerUps.Add(POWER_UP.BULLET_SCALE);
        RerenderPowerups();
    }

    public void OnBulletGravityCollected()
    {
        collectedPowerUps.Add(POWER_UP.BULLET_GRAVITY);
        RerenderPowerups();
    }

    private void RerenderPowerups()
    {
        foreach (Transform powerUpitem in transform)
        {
            Destroy(powerUpitem.gameObject);
        }

        collectedPowerUps.Sort();
        collectedPowerUps.ForEach(it =>
        {
            PowerupItemUI item = Instantiate<PowerupItemUI>(itemPrefab, transform);
            item.RenderPowerUp(it);
        });
    }
}
