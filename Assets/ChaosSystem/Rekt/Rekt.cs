using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rekt : Chaos
{
    Chaos[] ch;
    /*
     * On the ChoasSystem object order choas from wanted (higher on list) to unwanted (lower)
     */
    [Range(1, 20)]
    [SerializeField] private int numUnwantedChoas = 1;
    public override void Stop()
    {
        for (int i = 0; i < ch.Length; i += 1)
        {
            ch[i].Stop();
        }
    }

    public override void Trigger()
    {
        Component[] comp = this.GetComponents(typeof(Chaos));
        ch = new Chaos[comp.Length-numUnwantedChoas];
        for (int i = 0; i < comp.Length-numUnwantedChoas; i += 1)
        {
            ch[i] = (Chaos)comp[i];
            ch[i].Trigger();

        }
        
        
        //throw new System.NotImplementedException();
    }

}
