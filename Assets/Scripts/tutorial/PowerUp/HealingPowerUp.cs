using UnityEngine;

public class HealingPowerUp : PowerUpBase
{
    [Header("Healing Settings")]
    public float healAmount = 20f;

    protected override void ApplyEffect(TankHealth health, TankMovement movement, TankShooting shooting)
    {
        if (health != null)
        {
            health.Heal(healAmount);
            Debug.Log($"Healed for {healAmount}. Current Health: {health.GetCurrentHealth()}");


            // Actualiza solo la secci�n de salud del HUD.
            //if (hudManager != null)
            //{
            //    hudManager.UpdateHealth(health);
            //}
        }
    }
}
