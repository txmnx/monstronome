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
    public DirectionManager directionManager;

    [HideInInspector]
    public InstrumentFamily selectedFamily;

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
            if (selectedFamily == null) {
                selectedFamily = instrumentFamilyLooker.lookedFamily;
                OnSelectFamily?.Invoke(selectedFamily);
                directionManager.enableDirecting = false;
            }
        }
    }

    private void OnDeselectButtonPressed()
    {
        if (selectedFamily != null) {
            OnDeselectFamily?.Invoke(selectedFamily);
            selectedFamily = null;
            directionManager.enableDirecting = true;
        }
    }

    //Events
    public event Action<InstrumentFamily> OnSelectFamily;
    public event Action<InstrumentFamily> OnDeselectFamily;
}
