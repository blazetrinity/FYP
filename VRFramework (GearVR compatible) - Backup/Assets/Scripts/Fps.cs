using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Fps : MonoBehaviour {

	float deltaTime = 0;

	public Text fpstext;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		fpstext.text = (1.0f / deltaTime).ToString();
	}
}
