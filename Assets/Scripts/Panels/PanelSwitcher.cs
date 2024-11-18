using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelInicial;      // Panel de Instrucciones
    public GameObject panelDesarrollo;   // Panel de Desarrollo

    void Start()
    {
        // Mostrar solo el Panel Inicial al comienzo
        panelInicial.SetActive(true);
        panelDesarrollo.SetActive(false);
    }

    public void IrAlPanelDeDesarrollo()
    {
        // Ocultar el Panel Inicial y mostrar el Panel de Desarrollo
        panelInicial.SetActive(false);
        panelDesarrollo.SetActive(true);
    }

    public void IniciarJuego()
    {
        // Ocultar el Panel de Desarrollo y empezar el juego
        panelDesarrollo.SetActive(false);
        // Aquí puedes agregar lógica para iniciar el juego (activar gameplay)
        Debug.Log("Juego iniciado");
    }
}
