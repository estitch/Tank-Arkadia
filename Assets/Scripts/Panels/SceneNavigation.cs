using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    // Métodos para cambiar de escena
    void ResetTimeScale()
    {
        Time.timeScale = 1f; // Restablece la velocidad del tiempo a la normalidad
    }

    // Llamar antes de cambiar de escena

    public void GoToMainMenu()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToLevel1()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Nivel 3");
    }

    public void GoToLevel2()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Nivel 2");
    }

    public void GoToLevel3()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Nivel 1");
    }
    public void GoToLevelTutorial()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Tutorial");
    }

    // Opcional: Método para salir del juego
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting..."); // Este log es solo para confirmar en el editor
    }
}
