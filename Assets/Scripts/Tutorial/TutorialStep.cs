using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A step in a TutorialSequence
 */
public abstract class TutorialStep
{
    public virtual void Launch()
    { }

    protected virtual void OnSuccess()
    { }
}
