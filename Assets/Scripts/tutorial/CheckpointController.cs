using UnityEngine;
using UnityEngine.UI;

public class CheckpointController : MonoBehaviour
{
    // Singleton
    public static CheckpointController Instance { get; private set; }

    public int totalCheckpoints = 5; // Número total de checkpoints
    private int checkpointsReached = 0; // Contador de checkpoints alcanzados

    // Referencias a los textos de UI
    public Text checkpointsTextdescription;  // Texto para describir.
    public Text checkpointsText;  // Texto para "0/5", "1/5", etc.
    public Text totalCheckpointsText; // Texto fijo para mostrar el total "/5"

    // Referencias a los Canvas
    public Canvas canvasMovimiento;    // Canvas que muestra los controles y progreso
    public Canvas canvasFinal;         // Canvas que se muestra al completar la fase

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Inicializar textos
        UpdateCheckpointText();

        // Asegurarse de que el Canvas final esté inicialmente oculto
        if (canvasFinal != null)
        {
            canvasFinal.gameObject.SetActive(false);
        }
    }

    public void CheckpointReached()
    {
        checkpointsReached++;

        Debug.Log($"Checkpoint alcanzado: {checkpointsReached}/{totalCheckpoints}");

        // Actualiza los textos de progreso
        UpdateCheckpointText();

        // Verifica si se han alcanzado todos los checkpoints
        if (checkpointsReached >= totalCheckpoints)
        {
            CompletePhase();
        }
    }

    private void UpdateCheckpointText()
    {
        // Actualiza los textos para reflejar el progreso
        if (checkpointsText != null)
        {
            checkpointsText.text = checkpointsReached.ToString();
        }

        if (totalCheckpointsText != null)
        {
            totalCheckpointsText.text = $"/{totalCheckpoints}";
        }
    }

    private void CompletePhase()
    {
        Debug.Log("Todos los checkpoints alcanzados. Fase completada.");

        // Ocultar CanvasMovimiento y mostrar CanvasFinal
        if (canvasMovimiento != null)
        {
            canvasMovimiento.gameObject.SetActive(false); // Oculta Canvas de movimiento
        }

        if (canvasFinal != null)
        {
            canvasFinal.gameObject.SetActive(true); // Muestra el Canvas final
        }

        // Opcional: desactivar textos de progreso
        if (checkpointsText != null)
            checkpointsText.gameObject.SetActive(false);

        if (totalCheckpointsText != null)
            totalCheckpointsText.gameObject.SetActive(false);

        if (checkpointsTextdescription != null)
            checkpointsTextdescription.gameObject.SetActive(false);
    }

    public int GetRemainingCheckpoints()
    {
        return totalCheckpoints - checkpointsReached;
    }
}
