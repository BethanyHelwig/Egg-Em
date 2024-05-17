using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds the persistent game data.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public int scoreToReach = 10;
    public int score = 0;
    public int level = 1;
    public int musicVolume;
    public int sfxVolume;
    public float playerSpeed = 10f;
    public float eggSpawnSpeed = 1.0f;

    public float playerSpeedTimer = 5f;
    public bool playerSpeedTimerOn = false;

    public float eggSpawnTimer = 5f;
    public bool eggSpawnIncreaseTimerOn = false;

    public float grandmaSFXTimer = 7f;
    public bool grandmaSFXTimerOn = false;

    public float dragonMinusTimer = 3f;
    public bool dragonMinusTimerOn = false;

    /// <summary>
    /// Load and persist the manager without creating a duplicate
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Fixed Update from the Game Manager handles the timers used by gameplay mechanics.
    /// This includes timers for player speed bonus, egg spawn bonus, and timer to limit
    /// the amount of grandma SFX. If a timer is triggered as as on, it gets counted down here.
    /// </summary>
    private void FixedUpdate()
    {
        // Player speed increase timer countdown
        if (playerSpeedTimerOn)
        {
            if (playerSpeedTimer > 0)
            {
                playerSpeedTimer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Player speed increase ends.");
                playerSpeedTimer = 0;
                playerSpeedTimerOn = false;
                playerSpeed = 10f;
            }
        }

        // Egg spawn increase timer countdown
        if (eggSpawnIncreaseTimerOn)
        {
            if (eggSpawnTimer > 0)
            {
                eggSpawnTimer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Player speed increase ends.");
                eggSpawnTimer = 0;
                eggSpawnIncreaseTimerOn = false;
                eggSpawnSpeed = 1f;
            }
        }

        // Timer to space out Grandma's SFX so there's not overlap
        if (grandmaSFXTimerOn)
        {
            if (grandmaSFXTimer > 0)
            {
                grandmaSFXTimer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Grandma can speak again.");
                grandmaSFXTimer = 7f;
                grandmaSFXTimerOn = false;
            }
        }

        // Timer to display minus eggs after dragon appears
        if (dragonMinusTimerOn)
        {
            if (dragonMinusTimer > 0)
            {
                dragonMinusTimer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Minus eggs display turns off.");
                dragonMinusTimer = 3f;
                dragonMinusTimerOn = false;
            }
        }
    }

    /// <summary>
    /// This method increases the player's speed, sets a timer, and turns the timer on.
    /// </summary>
    public void IncreasedPlayerSpeed()
    {
        Debug.Log("Player speed increase activated.");
        playerSpeedTimer = 5f;
        playerSpeed = 15f;
        playerSpeedTimerOn = true;
    }

    /// <summary>
    /// This method increases the egg spawn rate, sets a timer, and turns the timer on.
    /// </summary>
    public void IncreasedEggSpawnRate()
    {
        Debug.Log("Egg spawn rate increase activated.");
        eggSpawnSpeed = 0.7f;
        eggSpawnTimer = 5f;
        eggSpawnIncreaseTimerOn = true;
    }

    /// <summary>
    /// This method will play the indexed sfx audio after checking if the grandma sfx timer
    /// is already on or not. If the timer is not on, it will play the audio then set the timer
    /// time and turn the timer on.
    /// </summary>
    /// <param name="splatRandom">The index number of the sfx sounds array.</param>
    public void PlayGrandmaSFX(int splatRandom)
    {
        if (!grandmaSFXTimerOn)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSounds[splatRandom].name);
            grandmaSFXTimer = 7f;
            grandmaSFXTimerOn = true;
            Debug.Log("Grandma timer is now on!");
        }
    }

    /// <summary>
    /// This method will play the named audio clip after checking if the grandma sfx timer
    /// is already on or not. If the timer is not on, it will play the audio then set the timer
    /// time and turn the timer on.
    /// </summary>
    public void PlayGrandmaSFX(string splatName)
    {
        if (!grandmaSFXTimerOn)
        {
            AudioManager.Instance.PlaySFX(splatName);
            grandmaSFXTimerOn = true;
            Debug.Log("Grandma timer is now on!");
        }
    }

    /// <summary>
    /// Method indicates a dragon has spawned and turns on the timer to enable a graphic on screen
    /// displaying a decrease to the plays score. After the timer ends, the graphic is disabled.
    /// </summary>
    public void DragonTakesEggs()
    {
        dragonMinusTimerOn = true;
    }
}
