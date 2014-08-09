using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	void Awake () {
		Application.targetFrameRate = 60;
		Input.gyro.updateInterval = 0.01f;
	}
	
	void Update () {	
	}
}
