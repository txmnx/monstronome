using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Deals with the delay generation of each instrument family
 * 
 * TODO : we currently are looking for another way than delay to induce "cacophonie"
 *
 */
public class InstrumentsDelayer : MonoBehaviour
{
    //TODO : maybe later we can store these references in a more global component
    public WoodsFamily woodsFamily;
    public BrassFamily brassFamily;
    public PercussionsFamily percussionsFamily;
    public StringsFamily stringsFamily;

    private InstrumentFamily[] m_Families;

    [Header("DEBUG")]
    //For the delay generation we increment the maxDelay of a family at each secondsBetweenEachDelay seconds
    public float secondsBetweenEachDelay = 0.5f;
    public float delayToAdd = 0.01f;
    private float timeSinceLastDelay = 0;

    private void Start()
    {
        m_Families = new InstrumentFamily[4] { woodsFamily, brassFamily, percussionsFamily, stringsFamily };
    }

    private void Update()
    {
        if (timeSinceLastDelay > secondsBetweenEachDelay) {
            DelayFamily();
            timeSinceLastDelay = timeSinceLastDelay % secondsBetweenEachDelay;
        }

        timeSinceLastDelay += Time.deltaTime;
    }

    private void DelayFamily()
    {
        m_Families[Random.Range(0, m_Families.Length)].AddToMaxDelay(delayToAdd);
    }
}
