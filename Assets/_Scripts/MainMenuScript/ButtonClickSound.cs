using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonClickSound : MonoBehaviour
{
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }





    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            //Debug.Log("Button Click Sound");
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
