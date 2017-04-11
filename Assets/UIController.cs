using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private MainController settings;

	void Awake() {
		settings = (MainController)FindObjectOfType(typeof(MainController));
	}

	public void SliderMinInterval(float newValue) {
		settings.minInterval = newValue * 10;
	}

	public void SliderMaxInterval(float newValue) {
		settings.maxInterval = newValue * 10;
	}

	public void SliderMaxDistToAttractor(float newValue) {
		settings.maxDistToAttractor = newValue * 50;
	}
}
