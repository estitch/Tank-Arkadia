using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    // Singleton
    public static CheckpointController Instance { get; private set; }

    public int totalCheckpoints = 5; // N�mero total de checkpoints
    private int checkpointsReached = 0; // Contador de checkpoints alcanzados

    private void Awake()
    {
        // Configuraci�n del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta
            return;
        }

        Instance = this; // Asignar esta instancia como el Singleton
        DontDestroyOnLoad(gameObject); // Evitar que el objeto se destruya al cambiar de escena
    }

    public void CheckpointReached()
    {
        checkpointsReached++; // Incrementa el contador

        Debug.Log($"Checkpoint alcanzado: {checkpointsReached}/{totalCheckpoints}");

        if (checkpointsReached >= totalCheckpoints)
        {
            CompletePhase(); // Llama al m�todo para completar la fase
        }
    }

    private void CompletePhase()
    {
        Debug.Log("Todos los checkpoints alcanzados. Fase completada.");
        // Aqu� notificamos al tutorial manager que la fase est� completa
        TutorialManager.Instance.NextPhase();
    }

    public int GetRemainingCheckpoints()
    {
        return totalCheckpoints - checkpointsReached; // Devuelve la cantidad de checkpoints restantes
    }
}
