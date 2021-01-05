using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagner : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayMusic1()
    {
        audioSource.Play();
    }
}
