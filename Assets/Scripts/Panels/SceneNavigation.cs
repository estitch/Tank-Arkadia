using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigation : MonoBehaviour
{
    // Métodos para cambiar de escena
    public void GoToGeneralScene()
    {
        SceneManager.LoadScene("general-scene");
    }

    public void GoToTutorialScene()
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
