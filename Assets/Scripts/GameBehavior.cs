using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// This class controls the main gameplay mechanics. 
/// </summary>
public class GameBehavior : MonoBehaviour
{
    public Egg eggPrefab;
    public Transform spawnPoint;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI levelTitle;
    [SerializeField] private TextMeshProUGUI levelDescription;
    [SerializeField] private TextMeshProUGUI playerSpeedBonusText;
    [SerializeField] private TextMeshProUGUI eggSpawnBonusText;
    [SerializeField] private GameObject dragonMinusText;
    [SerializeField] private GameObject playerSpeedBonusObject;
    [SerializeField] private GameObject eggSpawnBonusObject;
    [SerializeField] private GameObject levelCompleteDialogue;
    [SerializeField] private GameObject levelStartDialogue;
    [SerializeField] private GameObject levelFailedDialogue;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject levelNormal;
    [SerializeField] private GameObject levelRotten;
    [SerializeField] private GameObject levelSpecial;
    [SerializeField] private GameObject levelDragon;
    [SerializeField] private EggBar eggBar;

    //public EggBar eggBar;
    public int eggsNeeded = 8;
    public float respawnTimer = 1.0f;

    public float timeLeft;
    public bool timerOn = false;

    private EggSpawner eggSpawner;

    private string levelDescriptionNormal = "Use the arrow keys (or A and D) to move the frying pan to catch the egg yolks. Fill the pot before the timer runs out!";
    private string levelDescriptionRotten = "Oh, no! The kids have started throwing rotten eggs too! Be sure to avoid them or you'll lose some eggs cleaning them out of the pot!";
    private string levelDescriptionSpecial = "These kids are throwing the strangest eggs. You never know what might pop out of them! Hopefully something tasty!";
    private string levelDescriptionDragon = "Where on earth did they get this egg?? What could possibly be inside? It looks dangerous...";

    /// <summary>
    /// Start is called before the first frame update. It pauses the game, sets the level information, and enables the level opening dialogue.
    /// </summary>
    void Start()
    {
        Time.timeScale = 0;
        SetLevelInformation();
        if (GameManager.instance.level < 2)
        {
            levelNormal.SetActive(true);
        }
        else if (GameManager.instance.level < 4)
        {
            levelRotten.SetActive(true);
        }
        else if (GameManager.instance.level < 6)
        {
            levelSpecial.SetActive(true);
        }
        else
        {
            levelDragon.SetActive(true);
        }
    }

    /// <summary>
    /// This method is activated when the player presses the play button at the start of a level. It disables the start of level dialogue,
    /// unfreezes the time, starts the level countdown timer, gets the egg spawner and starts the eggs spawning.
    /// </summary>
    public void StartLevel()
    {      
        levelStartDialogue.SetActive(false);
        Time.timeScale = 1;
        eggSpawner = GetComponent<EggSpawner>();
        StartCoroutine(EggWave());
        eggBar.SetMaxEggsNeeded(eggsNeeded);
        //timeLeft = 3;
        timerOn = true;
    }

    /// <summary>
    /// The fixed update counts down the timer. Depending on if time is up or if the appropriate score is reached,
    /// it will also pause the game and enable the appropriate game over or level complete dialogues.
    /// </summary>
    private void FixedUpdate()
    {
        if (timerOn)
        {
            if(GameManager.instance.score >= GameManager.instance.scoreToReach)
            {
                LevelCompleted();
            }
            else if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                float seconds = Mathf.FloorToInt(timeLeft);
                time.text = string.Format("{00}", seconds);
            }
            else
            {
                Debug.Log("Time is up!");
                timeLeft = 0;
                timerOn = false;
                if(GameManager.instance.score < GameManager.instance.scoreToReach)
                {
                    LevelFailed();
                    AudioManager.Instance.PlaySFX("GrandmaGameOver");
                }
                else
                {
                    LevelCompleted();
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSounds[18].name);
                }
            }
        }

        /// <remarks>
        /// Enables the Player Speed Bonus text when the bonus is activated
        /// </remarks>
        if (GameManager.instance.playerSpeedTimerOn)
        {
            playerSpeedBonusObject.SetActive(true);
            float speedSeconds = Mathf.FloorToInt(GameManager.instance.playerSpeedTimer);
            playerSpeedBonusText.text = string.Format("{00}", speedSeconds);
        }
        else
        {
            playerSpeedBonusObject.SetActive(false);
        }

        /// <remarks>
        /// Enables the egg spawn rate text when the bonus is activated
        /// </remarks>
        if (GameManager.instance.eggSpawnIncreaseTimerOn)
        {
            eggSpawnBonusObject.SetActive(true);
            float speedSeconds = Mathf.FloorToInt(GameManager.instance.eggSpawnTimer);
            eggSpawnBonusText.text = string.Format("{00}", speedSeconds);
        }
        else
        {
            eggSpawnBonusObject.SetActive(false);
        }

        /// <remarks>
        /// Enables the dragon minus eggs text when the debuff is activated
        /// </remarks>
        if (GameManager.instance.dragonMinusTimerOn)
        {
            dragonMinusText.SetActive(true);
        }
        else
        {
            dragonMinusText.SetActive(false);
        }
    }

    /// <summary>
    /// Update is called once per frame and keeps the score and score meter up to date.
    /// </summary>
    void Update()
    {
        score.SetText(System.Convert.ToString(GameManager.instance.scoreToReach - GameManager.instance.score));
        eggBar.SetEgg(GameManager.instance.score);
    }

    /// <summary>
    /// This method gets the eggs spawning and pulled from their object pool.
    /// </summary>
    /// <returns></returns>
    IEnumerator EggWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.instance.eggSpawnSpeed);
            eggSpawner._pool.Get();
        }
    }

    /// <summary>
    /// Method pauses the game and enables the level failed dialogue in the GUI
    /// </summary>
    public void LevelFailed()
    {
        Time.timeScale = 0;
        levelFailedDialogue.SetActive(true);
    }

    /// <summary>
    /// Method pauses the game and enables the level completed dialogue in the GUI
    /// </summary>
    public void LevelCompleted()
    {
        Time.timeScale = 0;
        levelCompleteDialogue.SetActive(true);
    }


    /// <summary>
    /// Method unfreezes the game, resets the score, reloads the level that was just played
    /// </summary>
    public void SameLevelReset()
    {
        GameManager.instance.score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// Method unfreezes the game, increases the level, resets the score, and reloads the scene.
    /// </summary>
    public void NextLevelReset()
    {
        GameManager.instance.level += 1;
        GameManager.instance.score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// Method unfreezes the game, resets the level to the start, resets the score, and loads the main menu scene
    /// </summary>
    public void QuitGame()
    {
        GameManager.instance.level = 1;
        GameManager.instance.score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Method freezes the game and enables the options menu in the GUI
    /// </summary>
    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Method unfreezes the game and disables the options menu in the GUI
    /// </summary>
    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Method gets the level title and description in the GUI, sets the level timer, and updates the scoreToReach
    /// held in the Game Manager.
    /// </summary>
    public void SetLevelInformation()
    {
        levelTitle.text = "Level  " + GameManager.instance.level;
        Debug.Log(levelTitle);
        Debug.Log(GameManager.instance.level);

        if (GameManager.instance.level < 2)
        {
            levelDescription.text = levelDescriptionNormal;
            eggsNeeded = 8;
            timeLeft = 15;
            GameManager.instance.scoreToReach = 8;
        }
        else if (GameManager.instance.level < 4)
        {
            levelDescription.text = levelDescriptionRotten;
            eggsNeeded = 12;
            timeLeft = 20;
            GameManager.instance.scoreToReach = 12;
        }
        else if (GameManager.instance.level < 6)
        {
            levelDescription.text = levelDescriptionSpecial;
            eggsNeeded = 14;
            timeLeft = 20;
            GameManager.instance.scoreToReach = 14;
        }
        else
        {
            levelDescription.text = levelDescriptionDragon;
            eggsNeeded = 16;
            timeLeft = 25;
            GameManager.instance.scoreToReach = 16;
        }
    }
}
