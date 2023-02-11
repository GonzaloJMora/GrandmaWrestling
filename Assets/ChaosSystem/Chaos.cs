using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chaos : MonoBehaviour
{
    [SerializeField] protected string chaosName;

    [Tooltip("Clip for the Announcer Voiceline")]
    [SerializeField] private AudioSource announcerClip;
    [SerializeField] private string announcerOneLiner;
    
    
    //Trigger The Chaos Event
    public abstract void Trigger();

    //Stop the Chaos Event
    public abstract void Stop();
    //
    //plays the audio clip
    protected void PlayAnnouncerClip()
    {
        announcerClip.Play();
    }
}
