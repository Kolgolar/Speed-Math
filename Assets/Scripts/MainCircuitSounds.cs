using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCircuitSounds : MonoBehaviour
{
    public AudioClip extraLive; 
    public AudioClip gameOver; 


    public void GameOver()
    {
        GetComponent<AudioSource>().PlayOneShot(gameOver);
    }

    public void ExtraLive()
    {
        GetComponent<AudioSource>().PlayOneShot(extraLive);
    }
}
