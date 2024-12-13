using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    // Métodos para cambiar de escena
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToLevel1()
    {
        SceneManager.LoadScene("Nivel 1");
    }

    public void GoToLevel2()
    {
        SceneManager.LoadScene("Nivel 2");
    }

    public void GoToLevel3()
    {
        SceneManager.LoadScene("Nivel 3");
    }
    public void GoToLevelTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // Opcional: Método para salir del juego
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting..."); // Este log es solo para confirmar en el editor
    }
}
