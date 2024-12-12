using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveDisparadora : MonoBehaviour
{
    // Prefab de la bala
    public GameObject balaPrefab;

    // Fuerza con la que se dispara la bala (ajusta según la física de tu juego)
    public float fuerzaDisparo = 10f;

    // Tiempo entre disparos (en segundos)
    public float tiempoEntreDisparos = 1f;

    // Daño que hace la bala a otros prefabs
    public int dañoBala = 10;

    private float proximoDisparo = 0f; // Tiempo para el próximo disparo

    void Update()
    {
        // Disparar si el tiempo lo permite
        if (Time.time > proximoDisparo)
        {
            DispararBala();
            proximoDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    void DispararBala()
    {
    // Instanciar la bala en la posición de la nave
    GameObject bala = Instantiate(balaPrefab, transform.position, Quaternion.identity);

    // Ignorar colisiones entre la bala y la nave
    Collider balaCollider = bala.GetComponent<Collider>();
    Collider naveCollider = GetComponent<Collider>();

    if (balaCollider != null && naveCollider != null)
    {
        Physics.IgnoreCollision(balaCollider, naveCollider);
    }

    // Agregar componente de daño a la bala si no lo tiene
    if (!bala.GetComponent<DañoBala>())
    {
        bala.AddComponent<DañoBala>().daño = dañoBala;
    }
    else
    {
        // Si ya tiene el componente, asignar el daño
        bala.GetComponent<DañoBala>().daño = dañoBala;
    }

    // Aplicar fuerza para que la bala caiga hacia abajo
    bala.GetComponent<Rigidbody>().AddForce(Vector3.down * fuerzaDisparo, ForceMode.Impulse);
    }

}

// Script separado para el daño de la bala (puede ser reutilizado en otros proyectos)
public class DañoBala : MonoBehaviour
{
    public int daño = 10;

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto golpeado tiene un script que maneje salud
        if (collision.gameObject.GetComponent<Salud>())
        {
            collision.gameObject.GetComponent<Salud>().RestarSalud(daño);
            // Destruir la bala después de impactar
            Destroy(gameObject);
        }
    }
}

// Script básico para manejar la salud de otros prefabs (enemigos, edificios, etc.)
public class Salud : MonoBehaviour
{
    public int salud = 100;

    public void RestarSalud(int daño)
    {
        salud -= daño;
        if (salud <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        // Acciones a realizar al morir (como destruir el objeto o jugar una animación)
        Destroy(gameObject);
    }
}