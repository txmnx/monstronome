using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILanguageSwitch : XRGrabbable
{
    public enum Language
    {
        English,
        French
    }

    public SceneTransitionsManager sceneTransitionsManager;
    
    public GameObject englishButton;
    public GameObject frenchButton;

    public Renderer emissionEnglishButtonRend;
    public Renderer emissionFrenchButtonRend;

    public Material enPushedMaterial;
    public Material frPushedMaterial;
    
    public Material enUnpushedMaterial;
    public Material frUnpushedMaterial;
    
    public float pushDistance;

    public AK.Wwise.Event SFXOnSwitch;

    private Language m_CurrentLanguage;
    

    public override void OnEnterGrab(XRGrabber xrGrabber)
    {
        if (m_CurrentLanguage == Language.English)
        {
            m_CurrentLanguage = Language.French;
            
            frenchButton.transform.localPosition += new Vector3(0, 0, -pushDistance);
            englishButton.transform.localPosition += new Vector3(0, 0, pushDistance);

            SetMaterial(emissionFrenchButtonRend, frPushedMaterial);
            SetMaterial(emissionEnglishButtonRend, enUnpushedMaterial);
        }
        else if (m_CurrentLanguage == Language.French)
        {
            m_CurrentLanguage = Language.English;
            
            englishButton.transform.localPosition += new Vector3(0, 0, -pushDistance);
            frenchButton.transform.localPosition += new Vector3(0, 0, pushDistance);
            
            SetMaterial(emissionFrenchButtonRend, frUnpushedMaterial);
            SetMaterial(emissionEnglishButtonRend, enPushedMaterial);
        }

        SFXOnSwitch.Post(gameObject);
        
        AkSoundEngine.UnloadBank("SNB_Voice", IntPtr.Zero);
        AkSoundEngine.SetCurrentLanguage((m_CurrentLanguage == Language.English) ? "English(US)" : "Francais");
        AkSoundEngine.LoadBank("SNB_Voice", out uint id);
    }

    private void SetMaterial(Renderer rend, Material mat)
    {
        Material[] matArray = rend.materials;
        matArray[1] = mat;
        rend.materials = matArray;
    }
}
