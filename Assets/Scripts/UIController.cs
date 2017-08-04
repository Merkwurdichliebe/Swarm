using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private Manager settings;
	public Text FPSText;
	public Text EncounterText;
	public Text EncounterWithLightText;
	public Text BlueText;
	public Text RedText;
	public float deltaTime;

	void Awake() {
		settings = GameObject.Find ("Manager").GetComponent<Manager>();
	}

	void Update() {
		DisplayFPS ();
		DisplayEncounters ();
		BlueText.text = Bug.count [(int)BugGender.Male].ToString ();
		RedText.text = Bug.count [(int)BugGender.Female].ToString ();
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
		FPSText.text = Mathf.Ceil (fps).ToString ();
	}

	private void DisplayEncounters() {
		EncounterText.text = Bug.CountEncounters.ToString ();
		EncounterWithLightText.text = Bug.CountEncountersWithLight.ToString ();
	}
}
