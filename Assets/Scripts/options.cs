using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class options : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_InputField volumeInput;
    [SerializeField] private Slider volumeSliderMus;
    [SerializeField] private TMP_InputField volumeInputMus;
    [SerializeField] private GameObject mus;
    private AudioSource music;
    void Start()
    {
        AudioListener.volume = 0.5f;
        music = mus.GetComponent<AudioSource>();
        music.volume = 0.25f;

    }

    public void changeVolume(float vol)
    {
        AudioListener.volume = vol/100;
        volumeSlider.value = Mathf.Round(vol);
        volumeInput.text = Mathf.Round(vol).ToString();
    }
    public void changeVolume(string volString)
    {
        float vol = float.Parse(volString);        
        if (vol > 100 || vol < 0)
        {
            volumeInput.text = Mathf.Round(AudioListener.volume*100).ToString();;
        }
        else
        {
            AudioListener.volume = vol/100;
            volumeSlider.value = Mathf.Round(vol);
            volumeInput.text = Mathf.Round(vol).ToString();
        }
    }
    public void changeMusicVol(float vol)
    {
        music.volume = vol/100;
        volumeSliderMus.value = Mathf.Round(vol);
        volumeInputMus.text = Mathf.Round(vol).ToString();
    }
    public void changeMusicVolume(string volString)
    {
        float vol = float.Parse(volString);        
        if (vol > 100 || vol < 0)
        {
            volumeInputMus.text = Mathf.Round(AudioListener.volume*100).ToString();;
        }
        else
        {
            AudioListener.volume = vol/100;
            volumeSliderMus.value = Mathf.Round(vol);
            volumeInputMus.text = Mathf.Round(vol).ToString();
        }
    }
}
