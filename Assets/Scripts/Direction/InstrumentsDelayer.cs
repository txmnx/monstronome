using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Deals with the delay generation of each instrument family
 */
public class InstrumentsDelayer : MonoBehaviour
{
    //TODO : maybe later we can store these references in a more global component
    public WoodsFamily woodsFamily;
    public BrassFamily brassFamily;
    public PercussionsFamily percussionsFamily;
    public StringsFamily stringsFamily;

    private InstrumentFamily[] families;

    private void Start()
    {
        families = new InstrumentFamily[4] { woodsFamily, brassFamily, percussionsFamily, stringsFamily };
    }

    private void DelayAFamily()
    {

    }
}
