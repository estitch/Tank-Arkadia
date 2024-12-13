using UnityEngine;
using UnityEngine.SceneManagement;

public class Gano  : MonoBehaviour
{
    private GameObject metaPanel;

    public string metaPanelName = "MetaPanel";  // Nombre del panel en la escena
    public string mainMenuSceneName = "Menu";

    private bool isGamePaused = false;

    private void Start()
    {
        // Buscar el panel en la escena por nombre
        metaPanel = GameObject.Find(metaPanelName);

        if (metaPanel == null)
        {
            Debug.LogError($"No se encontró un objeto con el nombre '{metaPanelName}' en la escena.");
        }
        else
        {
            // Asegúrate de que el panel esté desactivado al inicio
            metaPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meta") && !isGamePaused)
        {
            OnReachMeta();
        }
    }

    private void OnReachMeta()
    {
        isGamePaused = true;

        // Pausar el juego
        Time.timeScale = 0;

        // Mostrar el panel
        if (metaPanel != null)
        {
            metaPanel.SetActive(true);
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
