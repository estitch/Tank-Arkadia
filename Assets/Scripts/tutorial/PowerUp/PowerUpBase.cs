using UnityEngine;
using System;

public abstract class PowerUpBase : MonoBehaviour
{
    [Header("Power-Up Settings")]
    public float duration = 0f; // Duraci�n del efecto, 0 si es instant�neo.

    //public TankHUDManager hudManager;
    public event Action<GameObject> OnPowerUpCollected;

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si el objeto que colisiona tiene el tag "Tank".
        if (other.CompareTag("Tank"))
        {
            // Busca los componentes del tanque.
            TankHealth tankHealth = other.GetComponent<TankHealth>();
            TankMovement tankMovement = other.GetComponent<TankMovement>();
            TankShooting tankShooting = other.GetComponent<TankShooting>();

            // Aplica el efecto correspondiente.
            ApplyEffect(tankHealth, tankMovement, tankShooting);
            OnPowerUpCollected?.Invoke(gameObject);  // Llama al evento, pasando el power-up que fue recogido

            // Maneja la recogida del power-up.
            OnPickup();
        }
    }

    // M�todo que aplica el efecto al tanque.
    protected abstract void ApplyEffect(TankHealth health, TankMovement movement, TankShooting shooting);

    // M�todo que actualiza el HUD con la informaci�n del tanque.


    // M�todo para manejar la recogida del power-up.
    private void OnPickup()
    {
        // Desaparece el power-up al recogerlo.
        gameObject.SetActive(false);
    }
}
