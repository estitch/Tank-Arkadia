using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI

//using TMPro;

public class CheckpointController : MonoBehaviour
{
    // Singleton
    public static CheckpointController Instance { get; private set; }

    public int totalCheckpoints = 5; // Número total de checkpoints
    private int checkpointsReached = 0; // Contador de checkpoints alcanzados

    // Referencias a los textos de TextMeshPro
    public Text checkpointsTextdescription;  // Texto para describir.
    public Text checkpointsText;  // Texto para "0/5", "1/5", etc.
    public Text totalCheckpointsText; // Texto fijo para mostrar el total "/5"

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta
            return;
        }

        Instance = this; // Asignar esta instancia como el Singleton
        DontDestroyOnLoad(gameObject); // Evitar que el objeto se destruya al cambiar de escena
    }

    private void Start()
    {
        // Inicializar textos
        UpdateCheckpointText();
    }

    public void CheckpointReached()
    {
        checkpointsReached++; // Incrementa el contador

        Debug.Log($"Checkpoint alcanzado: {checkpointsReached}/{totalCheckpoints}");

        // Actualiza los textos de progreso
        UpdateCheckpointText();

        // Verifica si se han alcanzado todos los checkpoints
        if (checkpointsReached >= totalCheckpoints)
        {
            CompletePhase(); // Llama al método para completar la fase
        }
    }

    private void UpdateCheckpointText()
    {
        // Actualiza los textos para reflejar el progreso
        if (checkpointsText != null)
        {
            checkpointsText.text = checkpointsReached.ToString(); // Muestra los checkpoints alcanzados
        }

        if (totalCheckpointsText != null)
        {
            totalCheckpointsText.text = $"/{totalCheckpoints}"; // Muestra el total de checkpoints
        }
    }

    private void CompletePhase()
    {
        Debug.Log("Todos los checkpoints alcanzados. Fase completada.");

        // Desactiva los textos al completar la fase
        if (checkpointsText != null)
            checkpointsText.gameObject.SetActive(false);

        if (totalCheckpointsText != null)
            totalCheckpointsText.gameObject.SetActive(false);

        if(checkpointsTextdescription != null)
            checkpointsTextdescription.gameObject.SetActive(false);

        // Notifica al tutorial manager que la fase está completa
        TutorialManager.Instance.NextPhase();
    }

    public int GetRemainingCheckpoints()
    {
        return totalCheckpoints - checkpointsReached; // Devuelve la cantidad de checkpoints restantes
    }
}
