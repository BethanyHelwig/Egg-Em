using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSFXMuteSprite : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    private void Start()
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

    private void Update()
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
