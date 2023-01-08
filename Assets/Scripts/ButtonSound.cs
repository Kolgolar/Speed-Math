using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private void OnEnable() 
    {
        MCButton.onClicked += PlayChoiceSound;
        PCRestartButton.onClicked += PlayStdSound;
        StopGameButton.onClicked += PlayStdSound;
        GMButton.onClicked += PlayGameModeSound;
        AimCamButton.onClicked += PlayCamSound;
        RestartGameButton.onClicked += PlayStdSound;
    }

    private void OnDisable() 
    {
        MCButton.onClicked -= PlayChoiceSound;
        PCRestartButton.onClicked -= PlayStdSound;
        StopGameButton.onClicked -= PlayStdSound;
        GMButton.onClicked -= PlayGameModeSound;
        AimCamButton.onClicked -= PlayCamSound;
        RestartGameButton.onClicked -= PlayStdSound;
    }

    void PlayStdSound()
    {
        GetComponent<AudioSource>().Play();
    }

    void PlayGameModeSound(int mode)
    {
        PlayStdSound();
    }

    void PlayChoiceSound(bool choice)
    {
        PlayStdSound();
    }

    void PlayCamSound(GameObject go)
    {
        PlayStdSound();
    }
}
