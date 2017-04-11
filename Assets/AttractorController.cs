using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorController : MonoBehaviour {
	
	void Update () {
		Vector3 pos = transform.position;
		pos.x += 30 * Input.GetAxis("Horizontal") * Time.deltaTime;
		pos.z += 30 * Input.GetAxis("Vertical") * Time.deltaTime;
		transform.position = pos;	
	}
}
