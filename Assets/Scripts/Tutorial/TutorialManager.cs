using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Describe and check the steps of the tutorial 
 */
public class TutorialManager : MonoBehaviour
{
    [Header("Objects to show")] 
    public GameObject factory;
    public GameObject potions;
    public GameObject metronomicon;

    private TutorialSequence m_Sequence;

    [Header("Voice instructions")] 
    [SerializeField]
    private TutorialStep.Instruction[] m_Instructions;
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_Sequence = new TutorialSequence(new TutorialStep[] {
            //{ new TutorialStep(m_Sequence, ) }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
