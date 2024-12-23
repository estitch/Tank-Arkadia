﻿using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
    
    public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
    public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
    public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
    public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
    public TankManager[] m_Tanks;               // A collection of managers for enabling and disabling different aspects of the tanks.
    public int m_scene = 1;
    public GameObject[] waypoints;

    private int m_RoundNumber;                  // Which round the game is currently on.
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    private TankManager m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
    private TankManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.
    public GameObject m_SpecialTankPrefab;  // Prefab para el tanque especial
    public GameObject gameOverPanel;  // El panel de GameOver (cartel con mensaje y botón).
    public Camera gameOverCamera;  // Cámara estática para el Game Over

    private void Start()
    {
        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds (m_StartDelay);
        m_EndWait = new WaitForSeconds (m_EndDelay);
        // Asegúrate de que las referencias se restablezcan al iniciar la escena
        if (m_Tanks == null || m_Tanks.Length == 0)
        {
            m_Tanks = new TankManager[8]; // o el número correcto de tanques
        }

        SpawnAllTanks();
        SetCameraTargets();

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine (GameLoop ());

    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            GameObject tankToSpawn;

            // Si es el primer tanque, usa el prefab especial
            if (i == 0 && m_SpecialTankPrefab != null)
            {
                tankToSpawn = m_SpecialTankPrefab;
            }
            else
            {
                tankToSpawn = m_TankPrefab;
            }

            // Instanciar el tanque
            m_Tanks[i].m_Instance = Instantiate(
                tankToSpawn,
                m_Tanks[i].m_SpawnPoint.position,
                m_Tanks[i].m_SpawnPoint.rotation
            );

            if (m_Tanks[i].m_Instance == null)
            {
                Debug.LogError($"No se pudo instanciar el tanque en el índice {i}.");
                continue;
            }

            // Configurar el tanque
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].m_scene = m_scene;
            m_Tanks[i].waypoints = waypoints;

            if (m_scene == 1)
            {
                if (i > 1 && i < 5)
                {
                    m_Tanks[i].goal = m_Tanks[1].m_Instance.transform;
                }
                else if (i > 4 && i < 8)
                {
                    m_Tanks[i].goal = m_Tanks[0].m_Instance.transform;
                }
            }
            else if (m_scene >= 2 && m_scene != 4)
            {
                if (i > 0)
                {
                    m_Tanks[i].goal = m_Tanks[0].m_Instance.transform;
                }
            }

            // Si es el tanque especial, conectamos el HUD
            if (i == 0 && m_Tanks[i].m_Instance != null)
            {
                // Obtener el TankHUDManager desde el Canvas
                TankHUDManager tankHUDManager = FindObjectOfType<TankHUDManager>();

                if (tankHUDManager != null)
                {
                    // Obtener los componentes del tanque especial
                    TankHealth tankHealth = m_Tanks[i].m_Instance.GetComponent<TankHealth>();
                    TankMovement tankMovement = m_Tanks[i].m_Instance.GetComponent<TankMovement>();
                    TankShooting tankShooting = m_Tanks[i].m_Instance.GetComponent<TankShooting>();

                    // Asignar los componentes al TankHUDManager
                    tankHUDManager.tankHealth = tankHealth;
                    tankHUDManager.tankMovement = tankMovement;
                    tankHUDManager.tankShooting = tankShooting;

                    Debug.Log("Conectando el HUD al tanque especial");
                }
                else
                {
                    Debug.LogError("No se encontró un TankHUDManager en la escena.");
                }
            }

            // Configura el tanque
            m_Tanks[i].Setup();
        }
    }




    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[m_Tanks.Length];

        // For each of these transforms...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... set it to the appropriate tank transform.
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }


    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop ()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine (RoundStarting ());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine (RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine (RoundEnding());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (m_GameWinner != null)
        {
            // If there is a game winner, restart the level.
            Application.LoadLevel (Application.loadedLevel);
        }
        else
        {
            // If there isn't a winner yet, restart this coroutine so the loop continues.
            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine (GameLoop ());
        }
    }



    private IEnumerator RoundStarting ()
    {
        // As soon as the round starts reset the tanks and make sure they can't move.
        ResetAllTanks ();
        DisableTankControl ();

        // Snap the camera's zoom and position to something appropriate for the reset tanks.
        m_CameraControl.SetStartPositionAndSize ();

        // Increment the round number and display text showing the players what round it is.
        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying ()
    {
        // As soon as the round begins playing let the players control the tanks.
        EnableTankControl ();

        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        // While there is not one tank left...
        while (!OneTankLeft())
        {
            // ... return on the next frame.
            yield return null;
        }
    }


    private IEnumerator RoundEnding ()
    {
        // Stop tanks from moving.
        DisableTankControl ();

        // Clear the winner from the previous round.
        m_RoundWinner = null;

        // See if there is a winner now the round is over.
        m_RoundWinner = GetRoundWinner ();

        // If there is a winner, increment their score.
        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        // Now the winner's score has been incremented, see if someone has one the game.
        m_GameWinner = GetGameWinner ();

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage ();
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }


    // This is used to check if there is one or fewer tanks remaining and thus the round should end.
    private bool OneTankLeft()
    {
        // Start the count of tanks left at zero.
        int numTanksLeft = 0;

        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        // If there are one or fewer tanks remaining return true, otherwise return false.
        if (m_scene == 4)
        {
            return numTanksLeft < 1;
        }
        else {
            return numTanksLeft <= 1;
        }
        
    }


    // This function is to find out if there is a winner of the round.
    // This function is called with the assumption that 1 or fewer tanks are currently active.
    private TankManager GetRoundWinner()
    {
        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if one of them is active, it is the winner so return it.
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        // If none of the tanks are active it is a draw so return null.
        return null;
    }


    // This function is to find out if there is a winner of the game.
    private TankManager GetGameWinner()
    {
        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if one of them has enough rounds to win the game, return it.
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        // If no tanks have enough rounds to win, return null.
        return null;
    }


    // Returns a string message to display at the end of each round.
    private string EndMessage()
    {
        // By default when a round ends there are no winners so the default end message is a draw.
        string message = "DRAW!";

        // If there is a winner then change the message to reflect that.
        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        // Add some line breaks after the initial message.
        message += "\n\n\n\n";

        // Go through all the tanks and add each of their scores to the message.
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        // If there is a game winner, change the entire message to reflect that.
        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    // This function is used to turn all the tanks back on and reset their positions and properties.
    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
    private void Update()
    {
        if (m_Tanks != null && m_Tanks.Length > 0)
        {
            // Verifica si el tanque especial ha muerto
            if (IsSpecialTankDead())
            {
                // Detener el juego y mostrar el cartel
                StopGame();
            }
        }
    }

    // Verificar si el tanque especial está muerto
    private bool IsSpecialTankDead()
    {
        if (m_Tanks.Length > 0 && m_Tanks[0] != null && m_Tanks[0].m_Instance != null)
        {
            TankHealth specialTankHealth = m_Tanks[0].m_Instance.GetComponent<TankHealth>();
            if (specialTankHealth != null && specialTankHealth.IsDead())
            {
                return true;
            }
        }
        return false;
    }


    // Detener el juego y mostrar el panel de fin
    private void StopGame()
    {
        // Detener todos los componentes de juego (puedes ajustar esto según tu lógica)
        gameOverCamera.gameObject.SetActive(true);
        foreach (var tank in m_Tanks)
        {
            tank.m_Movement.enabled = false;
 
        }

        // Activar el panel de fin de juego
        gameOverPanel.SetActive(true);

   
    }
    // Reinicia la escena actual
  
    private void OnDisable()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i] != null && m_Tanks[i].m_Instance != null)
            {
                Destroy(m_Tanks[i].m_Instance);
                m_Tanks[i].m_Instance = null;
            }
        }
    }



}