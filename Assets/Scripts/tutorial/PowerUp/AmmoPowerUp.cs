using UnityEngine;

public class AmmoPowerUp : PowerUpBase
{
    [Header("Ammo Settings")]
    public int ammoIncrease = 5;

    protected override void ApplyEffect(TankHealth health, TankMovement movement, TankShooting shooting)
    {
        if (shooting != null)
        {
            shooting.AddMaxBullets(ammoIncrease);
            Debug.Log($"Ammo max increased by {ammoIncrease}. Current Ammo: {shooting.GetCurrentBullets()}");

            //// Actualiza solo la sección de velocidad del HUD.
            //if (hudManager != null)
            //{
            //    hudManager.UpdateShooting(shooting);
            //}
        }
    }
}
