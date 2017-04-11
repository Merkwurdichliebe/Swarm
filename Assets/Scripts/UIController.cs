using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private MainController settings;

	void Awake() {
		settings = (MainController)FindObjectOfType(typeof(MainController));
	}

	public void SliderMinMaxInterval(float newValue) {
		settings.minInterval = Utilities.map01(newValue, 0f, 2f);
		settings.maxInterval = Utilities.map01(newValue, 0f, 10f);
		print (settings.minInterval + " / " + settings.maxInterval);
	}

	public void SliderMaxDistToAttractor(float newValue) {
		settings.maxDistToAttractor = (1 - newValue) * 50;
	}
}
