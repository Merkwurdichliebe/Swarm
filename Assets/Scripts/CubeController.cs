using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

	private float timeToNextTurn;			// Time before next turn
	private float timeAtTurn;				// Time at turn
	private float speedMult;				// Object speed multiplier
	private float distToAttractor;			// Object's distance to Attractor object

	private Vector3 dir;					// Random new direction for object
	private Vector3 dirVar;					// Random variation vector added to Attractor object

	private float maxDist;					// Maximum distance from the Attractor

	private GameObject attractor;

	public float vectorRange = 1f;			// To be remove, just for direction magnitude, not really used

	private MainController settings;

	void Awake() {
		attractor = GameObject.Find ("Attractor");
		settings = GameObject.Find ("MainController").GetComponent<MainController>();
	}
		
	void Start () {
	}

	void Update () {
		if (Time.time >= (timeAtTurn + timeToNextTurn)) {
			timeToNextTurn = Random.Range (settings.minInterval, settings.maxInterval);
			speedMult = Random.Range (settings.speedMin, settings.speedMax);
			timeAtTurn = Time.time;
			maxDist = settings.maxDistToAttractor * settings.maxDistToAttractor;
			distToAttractor = (transform.position - attractor.transform.position).sqrMagnitude;
			if (distToAttractor > maxDist && Random.Range(0, 100) < settings.attractorThreshold) {
				dirVar = Utilities.randomVectorInRange (settings.attractorVolume);
				dir = attractor.transform.position - transform.position + dirVar;
			} else {
				dir = Utilities.randomVectorInRange(vectorRange);
			}
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 10);
		transform.Translate (Vector3.forward * speedMult * Time.deltaTime);
	}
}
