using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

	private Vector3 direction;

	private float timeToNextTurn;			// Time before next turn
	private float timeAtTurn;				// Time at turn
	private float movementSpeed;			// Object speed multiplier
	public float distToAttractor;			// Object's distance to Attractor object
	private Vector3 dirVariation;			// Random variation vector added to Attractor object

	public GameObject trail;

	private GameObject attractor;

	public float vectorRange = 1f;			// To be remove, just for direction magnitude, not really used
	public float minInterval = 0f;			// Shortest time before turn
	public float maxInterval = 5f;			// Longest time before turn
	public int speedMin = 1;				// Minimum object speed
	public int speedMax = 50;				// Maximum object speed

	public float maxDistToAttractor = 10;	// Further away objects start approaching the Attractor
	public int attVar = 10;					// Variation for Attractor direction
	public float attThreshold = 10;			// Percentage of chance for object too far away to approach the Attractor

	void Awake() {
		attractor = GameObject.Find ("Attractor");
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame 
	void Update () {
		if (Time.time >= (timeAtTurn + timeToNextTurn)) {
			timeToNextTurn = Random.Range (minInterval, maxInterval);
			movementSpeed = Random.Range (speedMin, speedMax);
			timeAtTurn = Time.time;
			distToAttractor = (transform.position - attractor.transform.position).sqrMagnitude;
			if (distToAttractor > maxDistToAttractor * maxDistToAttractor && Random.Range(0, 100) > attThreshold) {
				dirVariation = Utilities.randomVectorInRange (attVar);
				direction = attractor.transform.position - transform.position + dirVariation;
			} else {
				direction = Utilities.randomVectorInRange(vectorRange);
			}
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * 10);
		transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
	}

	void Trail() {
		Instantiate (trail, transform.position, Quaternion.identity);
	}
}
