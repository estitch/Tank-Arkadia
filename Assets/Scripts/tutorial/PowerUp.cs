using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string powerUpType = "Health";  // Tipo de powerup, por ejemplo: "Health", "Speed", etc.
    public float powerUpValue = 1f;        // Valor del Power-Up (depende del tipo)

    // M�todo que maneja la recogida del power-up
    public void Collect()
    {
        Debug.Log("Power-up recogido: " + powerUpType);

        // Desactivar o destruir el Power-Up despu�s de ser recogido
        Destroy(gameObject);

        // Aqu� puedes agregar l�gica para el tanque (ejemplo: incrementar salud, velocidad, etc.)
        // TankManager.Instance.ApplyPowerUp(powerUpType, powerUpValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto que entra en el trigger es el tanque
        if (other.CompareTag("Player"))
        {
            // Llamamos al m�todo para recoger el power-up
            Collect();
        }
    }
}
