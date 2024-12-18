﻿using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    // This class is to manage various settings on a tank.
    // It works with the GameManager class to control how the tanks behave
    // and whether or not players have control of their tank in the 
    // different phases of the game.

    public Color m_PlayerColor;                             // This is the color this tank will be tinted.
    public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
    [HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.
    [HideInInspector] public int m_scene;            // This specifies which player this the manager for.
    [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
    [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.
    [HideInInspector] public Transform goal;
    [HideInInspector] public GameObject[] waypoints;

    public TankMovement m_Movement; // Reemplaza "TankMovement" por el tipo correcto
                                    // Reference to tank's movement script, used to disable and enable control.
    private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>()?.gameObject;

        if (m_Movement == null)
        {
            Debug.LogError($"TankMovement no encontrado en {m_Instance.name}");
        }

        if (m_Shooting == null)
        {
            Debug.LogError($"TankShooting no encontrado en {m_Instance.name}");
        }

        if (m_CanvasGameObject == null)
        {
            Debug.LogError($"Canvas no encontrado en {m_Instance.name}");
        }

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_Movement.goal = goal;
        m_Shooting.goal = goal;

        m_Movement.m_scene = m_scene;
        m_Shooting.m_scene = m_scene;
        m_Movement.waypoints = waypoints;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.material.color = m_PlayerColor;
        }
    }



    // Used during the phases of the game where the player shouldn't be able to control their tank.
    public void DisableControl ()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive (false);
    }


    // Used during the phases of the game where the player should be able to control their tank.
    public void EnableControl()
    {
        if (m_Movement == null || m_Shooting == null || m_CanvasGameObject == null)
        {
            Debug.LogError($"EnableControl falló: Componentes no inicializados en {m_Instance.name}");
            return;
        }

        Debug.Log($"EnableControl llamado por: {System.Environment.StackTrace}");

        m_Movement.enabled = true;
        m_Shooting.enabled = true;
        m_CanvasGameObject.SetActive(true);
    }




    // Used at the start of each round to put the tank into it's default state.
    public void Reset ()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive (false);
        m_Instance.SetActive (true);
    }
}