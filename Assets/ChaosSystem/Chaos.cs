using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chaos : MonoBehaviour
{

    [Tooltip("Clip for the Announcer Voiceline")]
    [SerializeField] private AudioSource announcerClip;
    
    //Trigger The Chaos Event
    public abstract void Trigger();

    //plays the audio clip
    protected void PlayAnnouncerClip()
    {
        announcerClip.Play();
    }
}
