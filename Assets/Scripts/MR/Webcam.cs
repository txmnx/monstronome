using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Webcam : MonoBehaviour
{
	void Start()
	{
		WebCamTexture webCamTexture = new WebCamTexture();
		Renderer renderer = GetComponent<Renderer>();
		renderer.sharedMaterial.SetTexture("_BaseMap", webCamTexture);
		webCamTexture.Play();
	}
}
