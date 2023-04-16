using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTicketManager : MonoBehaviour
{
    [Header("Audio")]
    private AudioSource audio;

    [SerializeField]
    private List<AudioClip> sounds;

    [SerializeField]
    private List<int> soundTickets;

    //get the audio source
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    //iterates through the sounds after picking a number (0-99 inclusive)
    //choice is made if the value of ticket < the current sound's highest ticket number
    public void playSound() {
        int ticket = Random.Range(0, 100);

        for (int i = 0; i < sounds.Count; i++) {
            if (ticket < soundTickets[i]) {
                audio.PlayOneShot(sounds[i]);
                break;
            }
        }
    }
}
