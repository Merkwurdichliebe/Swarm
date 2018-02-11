/* 
 * The Bug Prefab needs to use a Material with the Rendering Mode set to Fade
 * in order to make the fade-out effect work (in the Update method switch)
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BugGender {Male, Female};
public enum Status {Born, Adult, Dying};

public class Bug : MonoBehaviour {

	// EVENT SYSTEM
	// Declare delegate and events for notifications
	// These are subscribed to by the Manager class
	public delegate void BugEventHandler(Bug sender);
	public static event BugEventHandler BugDeath;
	public static event BugEventHandler BugEncounter;

	// Static counting variables
	public static int BugCount = 0;
	public static int CountEncounters = 0;
	public static int CountEncountersWithLight = 0;
	public static int CountDeaths = 0;

	// A 2-element array for counting male and female bugs
	// With a property for returning the total count
	public static int[] count = new int[2];
	public static int CountActive
	{
		get
		{
			return count [(int)BugGender.Male] + count[(int)BugGender.Female];
		}
	}

	// Bug parameters
	private float timeToNextTurn;				// Time before next turn
	private float timeAtTurn;					// Time at turn
	private float speedMult;					// Bug speed multiplier
	private float birthTime;					// Time when enabled
	private float deathTime;					// Time at the end of lifespan, used for death animation
	private float lifespan;					// Time before death
	private Vector3 dir;						// Direction bug is heading
	private Vector3 colliderStartSize;		// Size of the collider at creation, for scaling later

	// Object references
	private AttractorController attractor;
	private Setup setup;
	private BoxCollider boxCollider;

	// Gender is a Property so that setting it also changes the bug's color
	private BugGender gender;

	public BugGender Gender
	{
		get
		{
			return gender;
		}
		set
		{
			gender = value;
			material.color = (gender == BugGender.Male ? colorMale : colorFemale);
		}
	}

	// Bug status
	public Status status;

	// Renderer
	private Material material;
	private Color colorMale = new Color(0.5f, 0.5f, 1.0f, 1.0f);
	private Color colorFemale = new Color(1.0f, 0.5f, 0.5f, 1.0f);

	//Collisions
	public Collider lastCollider;

	void Awake() 
	{
		boxCollider = gameObject.GetComponent<BoxCollider> ();
		material = gameObject.GetComponent<Renderer> ().material;
		colliderStartSize = boxCollider.size;
		gameObject.tag = "Bug";
		lifespan = Random.Range (Setup.averageLifespan / 2f, Setup.averageLifespan * 1.5f);
		status = Status.Adult;
		BugCount++;
	}

	/// <summary>
	/// Initialize a new bug with the specified Gender.
	/// </summary>
	/// <param name="g">The bug's Gender.</param>
	public void Initialize(BugGender g)
	{
		gameObject.SetActive (true);
		Gender = g;
		Debug.Log (Gender);
		status = Status.Adult;
		birthTime = Time.time;
		transform.position = Utilities.randomVectorInRange (50);
		transform.rotation = Quaternion.LookRotation (Utilities.randomVectorInRange (1));
		count [(int)Gender]++;
		StartCoroutine(CheckIfDead());
		Debug.Log (string.Format ("FROM BUG : {0} has been enabled, Active count now {1}", this.name, CountActive));		
	}

	void OnDestroy() 
	{
		BugCount--;
	}

	void Update () 
	{
			switch (status)
			{
				case Status.Adult:
					Rotate ();
					Move ();
					break;
				case Status.Dying:
					// Do the death animation for 3.0 seconds, then die
					if (Time.time > deathTime + 3.0)
					{
						Die ();
					}
					else
					{
						PrepareToDie ();
						Move ();
					}
					break;	
			}
	}
		
	// Coroutine to perform lifespan check every half a second
	// We don't need to check this every frame
	IEnumerator CheckIfDead()
	{
		while (Time.time - birthTime < lifespan)
		{
			yield return new WaitForSeconds (0.5f);	
		}
		status = Status.Dying;
		deathTime = Time.time;
		// TODO Make this work: StartCoroutine (DoDeathAnimation ());
	}

	private void PrepareToDie()
	{
		// Slow down the bug to 20 percent of speed over time and fade it out
		speedMult = Mathf.Lerp (speedMult, 0, 3 * Time.deltaTime);
		material.color = new Color(material.color.r, material.color.g, material.color.b, Mathf.Lerp(material.color.a, 0, 2f * Time.deltaTime));
	}

	// TODO This doesn't work for now
	IEnumerator DoDeathAnimation()
	{
		float step = 0;
		while (step < 1) {
			material.color = new Color(material.color.r, material.color.g, material.color.b, Mathf.Lerp(255, 0, step));
			step += Time.deltaTime;
		}
		yield return null;
		Die ();
	}

	private void Rotate()
	{
		// Check if object should turn; don't turn if Dying
		if (Time.time >= (timeAtTurn + timeToNextTurn) && status != Status.Dying) 
		{
			timeToNextTurn = Random.Range (Setup.turnMin, Setup.turnMax);
			speedMult = Random.Range (Setup.speedMin, Setup.speedMax);
			timeAtTurn = Time.time;

			if (attractor.isOn) 
			{
				// If the object is too far from the attractor, give it a chance to get closer to it
				if ((transform.position - attractor.gameObject.transform.position).sqrMagnitude > (Setup.distanceMax * Setup.distanceMax) 
					&& Random.Range (0, 100) < Setup.attractorThreshold) 
				{
					dir = attractor.gameObject.transform.position - transform.position + Utilities.randomVectorInRange (Setup.attractorVolume);
				} 
				else 
				{
					dir = Utilities.randomVectorInRange(1);
				}
			} 
			else 
			{
				dir = Utilities.randomVectorInRange(1);
			}
		}
	}
		
	private void Move()
	{
		// Set new rotation and move the bug
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 10);
		transform.Translate (Vector3.forward * speedMult * Time.deltaTime);

		// TODO This should go into a method when paranoia slider is moved, not needed in every frame
		boxCollider.size = colliderStartSize * Setup.colliderScale;
	}

	// Add an attractor that the bug will attract to
	// We only need the AttractorController script
	public void AddAttractor(GameObject att)
	{
		attractor = att.GetComponent<AttractorController>();
	}

	void Die()
	{
		gameObject.SetActive (false);
		count [(int)gender]--;
		CountDeaths++;
		if (BugDeath != null)
		{
			BugDeath (this);
		}
		Debug.Log (string.Format ("FROM BUG : {0} has been disabled, Active count now {1}", this.name, CountActive));

	}

	/// <summary>
	/// Handle collision with another object
	/// </summary>
	/// <param name="other">The object with which this object has collided.</param>

	public void OnTriggerEnter(Collider other)
	{
		timeAtTurn = Time.time;
		timeToNextTurn = 0;
		lastCollider = other;
		if (BugEncounter != null)
		{
			CountEncounters++;
			BugEncounter (this);
		}
		if (other.gameObject.tag == "Bug" && status == Status.Adult)
		{
			if (other.gameObject.GetComponent<Bug>().gender != gender)
			{
				material.color = Color.yellow;
			}
		}
	}
		
}
