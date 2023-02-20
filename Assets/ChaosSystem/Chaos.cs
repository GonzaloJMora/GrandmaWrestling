using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chaos : MonoBehaviour
{
    [SerializeField] protected string chaosName;

    [Tooltip("Clip for the Announcer Voiceline")]
    [SerializeField] private AudioSource announcerClip;
    [SerializeField] protected string announcerOneLiner;
    
    
    //Trigger The Chaos Event
    public abstract void Trigger();

    //Stop the Chaos Event
    public abstract void Stop();
    //
    //plays the audio clip
    public virtual void PlayAnnouncerClip()
    {
        announcerClip.Play();
    }

    public virtual string GetOneliner()
    {
        return announcerOneLiner;
    }
}
