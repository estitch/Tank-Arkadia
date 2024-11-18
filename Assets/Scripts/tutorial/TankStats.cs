using UnityEngine;
using System;

public class TankStats : MonoBehaviour
{
    public static TankStats Instance { get; private set; }

    // Eventos para notificar cambios en los valores
    public event Action<int> OnHealthChanged;
    public event Action<float> OnSpeedChanged;
    public event Action<int, int> OnAmmoChanged;

    private int health = 100;
    private float speed = 5f;
    private int ammo = 9;
    private int maxAmmo = 20;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, 100);
            OnHealthChanged?.Invoke(health);
        }
    }

    public float Speed
    {
        get => speed;
        set
        {
            speed = Mathf.Max(0, value);
            OnSpeedChanged?.Invoke(speed);
        }
    }

    public int Ammo
    {
        get => ammo;
        set
        {
            ammo = Mathf.Clamp(value, 0, maxAmmo);
            OnAmmoChanged?.Invoke(ammo, maxAmmo);
        }
    }

    public int MaxAmmo
    {
        get => maxAmmo;
        set
        {
            maxAmmo = Mathf.Max(1, value);
            OnAmmoChanged?.Invoke(ammo, maxAmmo);
        }
    }
}
