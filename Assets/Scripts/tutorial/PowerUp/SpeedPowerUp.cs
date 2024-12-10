using UnityEngine;

public class SpeedPowerUp : PowerUpBase
{
    [Header("Speed Settings")]
    public float speedBoost = 5f;

    protected override void ApplyEffect(TankHealth health, TankMovement movement, TankShooting shooting)
    {
        if (movement != null)
        {
            movement.IncreaseSpeed(speedBoost);
            Debug.Log($"Speed increased by {speedBoost}. Current Speed: {movement.GetCurrentSpeed()}");


            // Actualiza solo la sección de velocidad del HUD.
            //if (hudManager != null)
            //{
            //    hudManager.UpdateSpeed(movement);
            //}
        }
    }
}
