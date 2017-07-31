using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameObject attractor;
	private Vector3 cameraPos;

	void Start() {
		attractor = GameObject.Find ("Attractor");
	}

	void Update () {
		cameraPos = attractor.transform.position;
		cameraPos.y += 60;
		cameraPos.z -= 20;
		transform.position = Vector3.Lerp (transform.position, cameraPos, Time.deltaTime * 1f);

	}
}
