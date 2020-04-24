using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedFamilyHighlighter : MonoBehaviour
{
    public InstrumentFamilySelector selector;

    private void Start()
    {
        selector.OnSelectFamily += OnSelectFamily;
        selector.OnDeselectFamily += OnDeselectFamily;
    }

    private void OnSelectFamily(InstrumentFamily family)
    {
        Debug.Log("SELECT " + family.name);
    }

    private void OnDeselectFamily(InstrumentFamily family)
    {
        Debug.Log("DESELECT " + family.name);
    }
}
