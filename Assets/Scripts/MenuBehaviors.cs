using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehaviors : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;

    public void StartGame()
    {
        Debug.Log("Play button pressed.");
        SceneManager.LoadScene("Main");
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        // set music volume
        // set SFX volume
        optionsMenu.SetActive(false);
    }


    public void CreditsMenu()
    {
        creditsMenu.SetActive(true);
    }

    public void CloseCreditsMenu()
    {
        creditsMenu.SetActive(false);
    }

}
