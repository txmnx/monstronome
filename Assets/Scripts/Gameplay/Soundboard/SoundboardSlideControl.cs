using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SoundboardSlideControl : MonoBehaviour
{

    public GameObject positionAt0;

    public GameObject positionAt1;

    public float sliderValue;

    private Vector3 pos;
    
    // Start is called before the first frame update
    void Start()
    {
        sliderValue = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        sliderValue = Mathf.InverseLerp(positionAt0.transform.position.x, positionAt1.transform.position.x, pos.x);
        Mathf.Clamp(sliderValue, 0, 1);
    }
}
