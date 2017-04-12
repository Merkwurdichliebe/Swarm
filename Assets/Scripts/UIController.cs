using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private MainController settings;
	public Text fpsText;
	public float deltaTime;

	void Awake() {
		settings = GameObject.Find ("MainController").GetComponent<MainController>();
	}

	void Update() { // TODO
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		fpsText.text = Mathf.Ceil (fps).ToString ();
	}

	public void SliderMinMaxInterval(float newValue) {
		settings.minInterval = Utilities.map01((1 - newValue), 0f, 2f);
		settings.maxInterval = Utilities.map01((1 - newValue), 0f, 10f);
	}

	public void SliderMinMaxSpeed(float newValue) {
		settings.speedMin = Utilities.map01(newValue, 1f, 20f);
		settings.speedMax = Utilities.map01(newValue, 3f, 60f);
	}

	public void SliderMaxDistToAttractor(float newValue) {
		settings.maxDistToAttractor = (1 - newValue) * (1 - newValue) * 100;
		settings.attractorVolume = ((1 - newValue) * 20);
	}
}
