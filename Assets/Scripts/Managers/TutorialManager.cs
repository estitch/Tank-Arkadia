using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI
using System.Collections;  // Necesario para trabajar con IEnumerator y corutinas

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public Text introText;            // El texto de introducción a mostrar.
    public float introDisplayTime = 5f;  // Tiempo que el texto permanecerá visible (5 segundos).
    public GameObject player;         // El jugador o tanque a mover.
    public float moveSpeed = 5f;      // Velocidad de movimiento del jugador.

    private bool canMove = false;     // Determina si el jugador puede moverse.
    private int currentPhase = 0;     // Fase actual del tutorial.

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta.
            return;
        }

        Instance = this; // Asignar esta instancia como el Singleton
        DontDestroyOnLoad(gameObject); // Evitar que el objeto se destruya al cambiar de escena
    }

    private void Start()
    {
        StartPhase(currentPhase);  // Inicia la primera fase del tutorial.
    }

    // Método para iniciar una fase específica
    private void StartPhase(int phase)
    {
        // Ejecuta la fase correspondiente.
        switch (phase)
        {
            case 0:
                ShowIntroMessage("Usa WASD para moverte");
                break;
            case 1:
                ShowIntroMessage("Ahora, usa el mouse para mirar alrededor");
                break;
            case 2:
                ShowIntroMessage("Presiona 'Espacio' para disparar");
                break;
            case 3:
                ShowIntroMessage("Tutorial completo. ¡Buena suerte!");
                break;
            default:
                Debug.Log("No hay más fases.");
                break;
        }
    }

    // Muestra el mensaje de la fase y espera el tiempo determinado antes de pasar a la siguiente fase
    private void ShowIntroMessage(string message)
    {
        introText.text = message;
        introText.gameObject.SetActive(true);  // Asegura que el texto esté visible

        // Inicia la corutina para ocultar el mensaje después de 5 segundos, pero no avanza a la siguiente fase.
        StartCoroutine(HideIntroTextAfterTime(introDisplayTime));
    }

    private IEnumerator HideIntroTextAfterTime(float time)
    {
        yield return new WaitForSeconds(time);  // Espera el tiempo especificado

        introText.gameObject.SetActive(false);  // Oculta el mensaje

        // Permite al jugador moverse solo después de que el mensaje se haya ocultado
        canMove = true;
    }

    public void NextPhase()
    {
        if (currentPhase < 3)  // Si no hemos alcanzado la última fase
        {
            currentPhase++;  // Incrementamos la fase
            StartPhase(currentPhase);  // Iniciamos la siguiente fase
            canMove = false;  // Desactivamos el movimiento hasta que se muestre el siguiente mensaje
        }
        else
        {
            Debug.Log("Tutorial completado.");
        }
    }
}
