using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public Text introText;            // El texto de introducción a mostrar.
    public float introDisplayTime = 5f;  // Tiempo que el texto permanecerá visible.
    public GameObject player;         // El jugador o tanque a mover.
    public GameObject enemyTankPrefab;  // Prefab del tanque enemigo a instanciar.
    public GameObject tankHUDCanvas;  // Canvas que muestra los atributos del tanque.
    public float moveSpeed = 5f;      // Velocidad de movimiento del jugador.

    private bool canMove = false;     // Determina si el jugador puede moverse.
    private int currentPhase = 0;     // Fase actual del tutorial.
    private GameObject currentEnemyTank;  // Referencia al tanque enemigo instanciado
    private TankHealth enemyTankHealth;   // Referencia al componente TankHealth del tanque enemigo
    private bool hasPassedPhase = false;  // Bandera para asegurar que la fase solo pase una vez

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
        // Asegurarse de que el HUD esté desactivado al inicio
        if (tankHUDCanvas != null)
        {
            tankHUDCanvas.SetActive(false);
        }

        StartPhase(currentPhase);  // Inicia la primera fase del tutorial.
    }

    private void StartPhase(int phase)
    {
        switch (phase)
        {
            case 0:
                ShowIntroMessage("Usa WASD para moverte");
                break;
            case 1:
                ShowIntroMessage("Haz clic izquierdo o el ctrl izquiedo para disparar y manten presionado para disparo cargado");
                StartShootingPhase();
                break;
            case 2:
                ShowIntroMessage("Ahora recoge los powerup para tener nuevas mejoras");
                ActivateHUD(); // Activar el HUD en la fase 3
                break;
            case 3:
                ShowIntroMessage("Tutorial completo. ¡Buena suerte!");
                break;
            default:
                Debug.Log("No hay más fases.");
                break;
        }
    }

    private void ShowIntroMessage(string message)
    {
        introText.text = message;
        introText.gameObject.SetActive(true);

        // Inicia la corutina para ocultar el mensaje después de un tiempo.
        StartCoroutine(HideIntroTextAfterTime(introDisplayTime));
    }

    private IEnumerator HideIntroTextAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        introText.gameObject.SetActive(false);
        canMove = true; // Permitir movimiento después del mensaje
    }

    public void NextPhase()
    {
        if (currentPhase < 3) // Si no hemos alcanzado la última fase
        {
            currentPhase++;
            StartPhase(currentPhase);
            canMove = false; // Desactivamos el movimiento hasta que se muestre el siguiente mensaje
        }
        else
        {
            Debug.Log("Tutorial completado.");
        }
    }

    public void StartShootingPhase()
    {
        InstantiateEnemyTank();
    }

    private void InstantiateEnemyTank()
    {
        Vector3 spawnPosition = player.transform.position + player.transform.forward * 15f;
        currentEnemyTank = Instantiate(enemyTankPrefab, spawnPosition, Quaternion.identity);

        enemyTankHealth = currentEnemyTank.GetComponent<TankHealth>();
    }

    private void Update()
    {
        if (!hasPassedPhase && enemyTankHealth != null && enemyTankHealth.IsDead())
        {
            hasPassedPhase = true;
            NextPhase();
        }
    }

    private void ActivateHUD()
    {
        Debug.Log("Inicio la fase 3");
        if (tankHUDCanvas != null)
        {
            tankHUDCanvas.SetActive(true);

            // Opcional: inicializar atributos desde el TutorialManager si es necesario
            TankStats.Instance.Health = 100; // Ejemplo
            TankStats.Instance.Speed = 5f;
            TankStats.Instance.Ammo = 9;
            TankStats.Instance.MaxAmmo = 20;
        }
        else
        {
            Debug.LogWarning("Tank HUD Canvas no está asignado en el TutorialManager.");
        }
    }

}
