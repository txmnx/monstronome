using System;
using UnityEngine;
using UnityEngine.XR;

/**
 * Used to select a family
 */
public class InstrumentFamilySelector : MonoBehaviour
{
    public XRCustomController controller;
    public InstrumentFamilyLooker instrumentFamilyLooker;
    public ConductingEventsManager conductingEventsManager;

    [HideInInspector]
    public InstrumentFamily selectedFamily;

    [HideInInspector]
    public bool hasSelected = false;
    
    //Treshold at which we consider a click to be on a particular direction
    private float m_buttonTreshold = 0.5f;

    private void Update()
    {
        bool isClicking;
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out isClicking)) {
            if (isClicking) {
                Vector2 clickAxis;
                if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out clickAxis)) {
                    if (clickAxis.y > m_buttonTreshold) {
                        OnSelectButtonPressed();
                    }
                    if (clickAxis.y < -m_buttonTreshold) {
                        OnDeselectButtonPressed();
                    }
                }
            }
        }
    }

    private void OnSelectButtonPressed()
    {
        if (instrumentFamilyLooker.lookedFamily != null) {
            if (!hasSelected) {
                selectedFamily = instrumentFamilyLooker.lookedFamily;
                hasSelected = true;
                OnSelectFamily?.Invoke();
                conductingEventsManager.enableConducting = false;
            }
        }
    }

    private void OnDeselectButtonPressed()
    {
        if (hasSelected) {
            OnDeselectFamily?.Invoke();
            selectedFamily = null;
            hasSelected = false;
            conductingEventsManager.enableConducting = true;
        }
    }

    //Events
    public event Action OnSelectFamily;
    public event Action OnDeselectFamily;
}
