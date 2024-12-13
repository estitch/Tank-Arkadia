using UnityEngine;

public class CameraManagers : MonoBehaviour
{
    public Camera mainCamera;  // La c�mara principal
    public Transform defaultCameraPosition;  // Posici�n alternativa de la c�mara (por ejemplo, una vista superior)
    private Transform currentTarget;  // El objetivo actual de la c�mara

    private void Update()
    {
        if (currentTarget == null)
        {
            // Si no hay objetivo, mueve la c�mara a la posici�n predeterminada
            mainCamera.transform.position = defaultCameraPosition.position;
            mainCamera.transform.rotation = defaultCameraPosition.rotation;
        }
    }

    public void SetCameraTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    // Llamar a este m�todo cuando el tanque especial muere
    public void OnSpecialTankDeath()
    {
        // Desvincula la c�mara del tanque especial
        SetCameraTarget(null);  // La c�mara ya no sigue al tanque especial.
    }
}
