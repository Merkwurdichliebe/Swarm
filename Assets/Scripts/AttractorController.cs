using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorController : MonoBehaviour {

	public bool isOn = true;

	private Renderer rend;
	public Light light;
	private bool switchingStates = false;

	Color offColor = new Color (0.3f, 0.3f, 0.3f, 3f);
	Color onColor = new Color (1f, 1f, 0.5f, 1f);

	void Start() {
		GameObject sphere = GameObject.Find ("Attractor/Sphere");
		rend = sphere.GetComponent<Renderer> ();
	}
	
	void Update () {
		Vector3 pos = transform.position;
		pos.x += 30 * Input.GetAxis("Horizontal") * Time.deltaTime;
		pos.z += 30 * Input.GetAxis("Vertical") * Time.deltaTime;
		transform.position = pos;

		if (Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(ToggleLight(1));
		}
	}

	IEnumerator ToggleLight(float rate) {
		if (!switchingStates) {
			
			Color currentColor = rend.material.color;
			Color destinationColor = isOn ? offColor : onColor;
			float step = 0;
			while (step < 1) {
				Color c = Color.Lerp (currentColor, destinationColor, step);
				rend.material.color = c;
				light.intensity = isOn ? 1 - step : step;
				step += rate * Time.deltaTime;
				switchingStates = true;
				yield return null;
			}
			isOn = !isOn;
			switchingStates = false;
		}
	}
	/*
	IEnumerator LightSwitch() {
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = rend.material.color;
			c.r = f; c.g = f; c.b = f;
			rend.material.color = c;
			yield return null;
		}
			isOn = !isOn;
		print ("Ended");
	}
	*/
}
