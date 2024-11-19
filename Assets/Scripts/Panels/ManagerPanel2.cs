using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class ManagerPanel2 : MonoBehaviour
{
    // Referencias a los Paneles
    public GameObject panelIntroduccion;  // Panel de Introducción
    public GameObject panelDesarrollo;    // Panel de Desarrollo



    public  void Start()
    {
        // Inicialmente, mostrar el panel de Introducción y ocultar el de Desarrollo
        panelIntroduccion.SetActive(true);
        panelDesarrollo.SetActive(false);

     
    }

    // Método que se ejecuta al hacer clic en "Continuar"
    public void OnContinuarClick()
    {
        // Ocultar el Panel de Introducción y mostrar el Panel de Desarrollo
        panelIntroduccion.SetActive(false);
        panelDesarrollo.SetActive(true);
    }

    // Método que se ejecuta al hacer clic en "Regresar al Menú"
    public void OnRegresarMenuClick()
    {
        // Cargar la escena de menú
        SceneManager.LoadScene("Main Menu"); // Cambia "Menu" por el nombre de tu escena de menú
    }
}
