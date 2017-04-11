using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {

	public static Vector3 randomVectorInRange(float range) {
		return new Vector3 (Random.Range (-range, range), Random.Range (-range, range), Random.Range (-range, range));
	}
}
