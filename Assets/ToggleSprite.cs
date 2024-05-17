using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToggleSprite : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;


    public void SpriteChangeMusic()
    {
        if (AudioManager.Instance.musicSource.mute)
        {
            buttonImage.GetComponent<Image>().sprite = soundOff;
        }
        else
        {
            buttonImage.GetComponent<Image>().sprite = soundOn;
        }
    }

    public void SpriteChangeSFX()
    {
        if (AudioManager.Instance.sfxSource.mute)
        {
            buttonImage.GetComponent<Image>().sprite = soundOff;
        }
        else
        {
            buttonImage.GetComponent<Image>().sprite = soundOn;
        }
    }

}
