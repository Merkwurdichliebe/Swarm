using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorController : MonoBehaviour {

	public bool isOn = true;

	private Renderer rend;
	public Light theLight;
	private bool switchingStates = false;

	Color offColor = new Color (0.3f, 0.3f, 0.3f, 3f);
	Color onColor = new Color (1f, 1f, 0.5f, 1f);

	void Start() {
		GameObject sphere = GameObject.Find ("Attractor/Sphere");
		rend = sphere.GetComponent<Renderer> ();
	}
	
	void Update () {
		Vector3 pos = transform.position;
		pos.x += 50 * Input.GetAxis("Horizontal") * Time.deltaTime;
		pos.z += 50 * Input.GetAxis("Vertical") * Time.deltaTime;
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
				theLight.intensity = isOn ? 1 - step : step;
				step += rate * Time.deltaTime;
				switchingStates = true;
				yield return null;
			}
			isOn = !isOn;
			switchingStates = false;
		}
	}
}
