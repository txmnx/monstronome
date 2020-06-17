using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TVChoice : MonoBehaviour
{
    public UnityEvent OnSelect;
    public GameObject defaultChoice;
    public GameObject highlightChoice;

    public void Highlight(bool highlight)
    {
        if (highlight) {
            defaultChoice.SetActive(false);
            highlightChoice.SetActive(true);
        }
        else {
            highlightChoice.SetActive(false);
            defaultChoice.SetActive(true);
        }
    }
}
