using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour {

	// Static counting variables
	public static int bugCount = 0;
	public static int countActive = 0;
	public static int countEncounters = 0;
	public static int countEncountersWithLight = 0;
	public static int countDeaths = 0;

	// Bug parameters
	private float timeToNextTurn;			// Time before next turn
	private float timeAtTurn;				// Time at turn
	private float speedMult;				// Bug speed multiplier
	private float birthTime;				// Time when enabled
	private float lifespan;					// Time before death
	private Vector3 dir;					// Direction bug is heading
	private Vector3 colliderStartSize;		// Size of the collider at creation, for scaling later

	// Object references
	private AttractorController attractor;
	private Manager manager;
	private BoxCollider boxCollider;

	public enum Gender {Male, Female};
	public Gender gender;

	private Color colorMale = new Color(0.5f, 0.5f, 1.0f, 1.0f);
	private Color colorFemale = new Color(1.0f, 0.5f, 0.5f, 1.0f);

	void Awake() {
		manager = GameObject.Find ("Manager").GetComponent<Manager>();
		boxCollider = GetComponent<BoxCollider> ();
		colliderStartSize = boxCollider.size;
		gameObject.tag = "Bug";
		lifespan = manager.averageLifespan + Random.Range (-3f, 4f);
		bugCount++;
	}
		
	void Start () {
		if (gender == Gender.Male) {
			gameObject.GetComponent<Renderer> ().material.color = colorMale;
		}
		else {
			gameObject.GetComponent<Renderer> ().material.color = colorFemale;
		}
	}

	void OnEnable() {
		birthTime = Time.time;
		countActive++;
	}

	void OnDisable() {
		countActive--;
	}

	void OnDestroy() {
		bugCount--;
	}

	void Update () {
		// Check lifespan
		if (Time.time - birthTime > lifespan) {
			countDeaths++;
			manager.Death (gameObject);
		}

		// Check if object should turn
		if (Time.time >= (timeAtTurn + timeToNextTurn)) {
			timeToNextTurn = Random.Range (manager.turnMin, manager.turnMax);
			speedMult = Random.Range (manager.speedMin, manager.speedMax);
			timeAtTurn = Time.time;

			if (attractor.isOn) {
				// If the object is too far from the attractor, give it a chance to get closer to it
				if ((transform.position - attractor.gameObject.transform.position).sqrMagnitude > (manager.distanceMax * manager.distanceMax) 
					&& Random.Range (0, 100) < manager.attractorThreshold) {
					dir = attractor.gameObject.transform.position - transform.position + Utilities.randomVectorInRange (manager.attractorVolume);
				} else {
					dir = Utilities.randomVectorInRange(1);
				}
			} else {
				dir = Utilities.randomVectorInRange(1);
			}
		}

		// Set new rotation and move the bug
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 10);
		transform.Translate (Vector3.forward * speedMult * Time.deltaTime);

		// TODO This should go into a method when paranoia slider is moved, not needed in every frame
		boxCollider.size = colliderStartSize * manager.colliderScale;
	}

	// Add an attractor that the bug will attract to
	// We only need the AttractorController script
	public void AddAttractor(GameObject att) {
		attractor = att.GetComponent<AttractorController>();
	}

	void OnTriggerEnter(Collider other) {
		timeAtTurn = Time.time;
		timeToNextTurn = 0;
		manager.Encounter (gameObject, other.gameObject);
		if (other.gameObject.tag == "Bug") {
			if (other.gameObject.GetComponent<BugController>().gender != gender) {
				gameObject.GetComponent<Renderer> ().material.color = Color.white;
			}
		}
	}
}
