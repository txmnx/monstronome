using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Deals with the delay generation of each instrument family
 */
public class InstrumentsDelayer : MonoBehaviour
{
    //TODO : maybe later we can store these references in a more global component
    public WoodwindFamily woodwindFamily;
    public BrassFamily brassFamily;
    public PercussionFamily percussionFamily;
    public StringedFamily stringedFamily;

    private InstrumentFamily[] families;

    private void Start()
    {
        families = new InstrumentFamily[4] { woodwindFamily, brassFamily, percussionFamily, stringedFamily };
    }

    private void DelayAFamily()
    {

    }
}
