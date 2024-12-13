using TMPro;
using UnityEngine;

public class TankHUDManager : MonoBehaviour
{
    [Header("Referencias de la UI")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI bulletsText;
    public TextMeshProUGUI speedText;
    //public TextMeshProUGUI bulletTypeText;
    //public Slider healthSlider;

    // Referencias al tanque para actualizar los valores continuamente
    public TankHealth tankHealth;
    public TankMovement tankMovement;
    public TankShooting tankShooting;

    private void Update()
    {
        // Actualiza la UI continuamente con los valores actuales del tanque
        UpdateHealth();
        UpdateSpeed();
        UpdateShooting();
    }

    private void UpdateHealth()
    {
        if (tankHealth != null)
        {
            // Redondea la salud actual y máxima antes de mostrarlas
            float currentHealth = Mathf.Round(tankHealth.GetCurrentHealth());
            float maxHealth = Mathf.Round(tankHealth.GetMaxHealth());

            // Actualiza el texto de la salud
            healthText.text = $"{currentHealth} / {maxHealth}";
            Debug.Log($"Actualización de salud: {currentHealth} / {maxHealth}");
        }
    }

    private void UpdateSpeed()
    {
        if (tankMovement != null)
        {
            // Redondea la velocidad actual antes de mostrarla
            float currentSpeed = Mathf.Round(tankMovement.GetCurrentSpeed());

            // Actualiza el texto de la velocidad
            speedText.text = $"{currentSpeed}";
            Debug.Log($"Actualización de velocidad: {currentSpeed}");
        }
    }

    private void UpdateShooting()
    {
        if (tankShooting != null)
        {
            // Redondea las balas actuales y máximas antes de mostrarlas
            int currentBullets = Mathf.RoundToInt(tankShooting.GetCurrentBullets());
            int maxBullets = Mathf.RoundToInt(tankShooting.GetMaxBullets());

            // Actualiza el texto de las balas
            bulletsText.text = $"{currentBullets} / {maxBullets}";
            Debug.Log($"Actualización de balas: {currentBullets} / {maxBullets}");
        }
    }
    public void SetTankReferences(TankHealth health, TankMovement movement, TankShooting shooting)
    {
        tankHealth = health;
        tankMovement = movement;
        tankShooting = shooting;
    }

}
