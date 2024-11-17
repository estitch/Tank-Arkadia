using UnityEngine;

public class AmmoPowerUp : PowerUpBase
{
    [Header("Ammo Settings")]
    public int ammoIncrease = 5;

    protected override void ApplyEffect(TankHealth health, TankMovement movement, TankShooting shooting)
    {
        if (shooting != null)
        {
            shooting.AddBullets(ammoIncrease);
            Debug.Log($"Ammo increased by {ammoIncrease}. Current Ammo: {shooting.GetCurrentBullets()}");
        }
    }
}
