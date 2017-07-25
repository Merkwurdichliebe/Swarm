using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour {

	private float timeToNextTurn;			// Time before next turn
	private float timeAtTurn;				// Time at turn
	private float speedMult;				// Bug speed multiplier
	private float distToAttractor;			// Bug distance to Attractor object
	private float maxDist;					// Maximum distance from the Attractor

	private Vector3 dir;					// Random new direction for bug
	private Vector3 dirVar;					// Random variation vector added to Attractor object

	private GameObject attractor;
	private AttractorController attractorController;
	private MainController settings;
	private BoxCollider boxCollider;
	private Vector3 startSize;

	void Awake() {
		settings = GameObject.Find ("MainController").GetComponent<MainController>();
		attractor = settings.attractor;
		attractorController = attractor.GetComponent<AttractorController>();
		boxCollider = GetComponent<BoxCollider> ();
		startSize = boxCollider.size;
	}
		
	void Start () {
	}

	void Update () {
		// Check if object should turn
		if (Time.time >= (timeAtTurn + timeToNextTurn)) {
			timeToNextTurn = Random.Range (settings.minInterval, settings.maxInterval);
			speedMult = Random.Range (settings.speedMin, settings.speedMax);
			timeAtTurn = Time.time;

			if (attractorController.isOn) {
				maxDist = settings.maxDistToAttractor * settings.maxDistToAttractor;
				distToAttractor = (transform.position - attractor.transform.position).sqrMagnitude;

				// If the object is too far from the attractor, give it a chance to get closer to it
				if (distToAttractor > maxDist && Random.Range(0, 100) < settings.attractorThreshold) {
					dirVar = Utilities.randomVectorInRange (settings.attractorVolume);
					dir = attractor.transform.position - transform.position + dirVar;
				} else {
					dir = Utilities.randomVectorInRange(1);
				}

			} else {
				dir = Utilities.randomVectorInRange(1);
			}
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 10);
		transform.Translate (Vector3.forward * speedMult * Time.deltaTime);
		boxCollider.size = startSize * settings.colliderScale;
	}

	void OnTriggerEnter(Collider other) {
		timeAtTurn = Time.time;
		timeToNextTurn = 0;
		settings.AddEncounter (gameObject, other.gameObject);
	}
}
