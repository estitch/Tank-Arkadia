using UnityEngine;

public class CheckpointObject : MonoBehaviour
{
    public string playerTag = "Tank"; // Tag que identifica al tanque (player)
    public CheckpointController checkpointController; // Referencia al controlador de checkpoints

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Objeto '{gameObject.name}' detect� una colisi�n con: {other.name}"); // Mensaje inicial de colisi�n

        // Comprueba si el objeto que colisiona tiene el tag del jugador
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"'{other.name}' tiene el tag correcto ('{playerTag}'). Procesando checkpoint...");

            // Notifica al controlador que este checkpoint ha sido tocado
            if (checkpointController != null)
            {
                Debug.Log($"Notificando al controlador '{checkpointController.name}' que el checkpoint fue alcanzado.");
                CheckpointController.Instance.CheckpointReached();
            }
            else
            {
                Debug.LogWarning("�CheckpointController no est� asignado en el Inspector!");
            }

            // Desactiva este objeto en la escena
            Debug.Log($"Checkpoint '{gameObject.name}' desactivado.");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"'{other.name}' no tiene el tag correcto ('{playerTag}'). Ignorando colisi�n.");
        }
    }
}
