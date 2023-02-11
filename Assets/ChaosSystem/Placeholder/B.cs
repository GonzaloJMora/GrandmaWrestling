using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : Chaos
{
    public override void Trigger()
    {
        Debug.Log(chaosName + " Triggered");
    }

    public override void Stop()
    {
        Debug.Log(chaosName + " Stopped");
    }
}

