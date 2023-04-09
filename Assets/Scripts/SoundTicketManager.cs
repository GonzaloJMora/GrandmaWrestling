using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTicketManager : MonoBehaviour
{
    private AudioSource audio;

    [SerializeField]
    private List<AudioClip> sounds;

    [SerializeField]
    private List<int> soundTickets;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

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
