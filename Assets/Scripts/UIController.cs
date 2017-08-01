using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private Manager settings;
	public Text fpsText;
	public Text encounterText;
	public Text encounterWithLightText;
	public float deltaTime;

	void Awake() {
		settings = GameObject.Find ("Manager").GetComponent<Manager>();
	}

	void Update() {
		DisplayFPS ();
		DisplayEncounters ();
	}

	public void SliderMinMaxInterval(float newValue) {
		settings.turnMin = Utilities.map01((1 - newValue), 0f, 2f);
		settings.turnMax = Utilities.map01((1 - newValue), 0f, 10f);
	}

	public void SliderMinMaxSpeed(float newValue) {
		settings.speedMin = Utilities.map01(newValue, 1f, 20f);
		settings.speedMax = Utilities.map01(newValue, 3f, 60f);
	}

	public void SliderMaxDistToAttractor(float newValue) {
		settings.distanceMax = (1 - newValue) * (1 - newValue) * 100;
		settings.attractorVolume = ((1 - newValue) * 20);
	}

	public void SliderColliderScale(float newValue) {
		settings.colliderScale = newValue;
	}

	private void DisplayFPS() {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		fpsText.text = Mathf.Ceil (fps).ToString ();
	}

	private void DisplayEncounters() {
		encounterText.text = BugController.countEncounters.ToString ();
		encounterWithLightText.text = BugController.countEncountersWithLight.ToString ();
	}
}
