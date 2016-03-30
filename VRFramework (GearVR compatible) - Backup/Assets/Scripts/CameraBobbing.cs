using UnityEngine;
using System.Collections;

public class CameraBobbing : MonoBehaviour {

	public float BobbingIntervals = 1.0f;
	public float MaxBobRange;
	public float MinBobRange;
	public float BobSpeed;

	float initialY;
	float initialRange;
	public float timeElapsed;
	float bobValue;
	float amountBobbed;

	bool positiveBob;

	Waypoints waypointscript;

	// Use this for initialization
	void Start () {
		waypointscript = gameObject.GetComponent<Waypoints> ();
		initialY = transform.position.y;
		initialRange = MaxBobRange - MinBobRange;
		timeElapsed = 0;
		amountBobbed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;

		//FactorSpeed ();

		if (timeElapsed >= BobbingIntervals) {
			RandomBobbing ();
			timeElapsed = 0;
		}

		Bob ();
	}

	void Bob()
	{
		if (amountBobbed != bobValue) {

			amountBobbed += BobSpeed * Time.deltaTime;

			if (positiveBob) {
				transform.Translate(0, BobSpeed * Time.deltaTime, 0);
			}
			else {
				transform.Translate(0, -BobSpeed * Time.deltaTime, 0);
			}

			//Debug.Log ("Bob per Frame" + BobSpeed * Time.deltaTime);
			//Debug.Log ("Bob amount" + amountBobbed);
			if (Mathf.Abs(bobValue - amountBobbed) < 2.5) {
				amountBobbed = bobValue;
				Debug.Log("Bob reached");
			}
		} 
		else {
			if(Mathf.Abs(initialY - transform.position.y) > 2.5f)
			{
				if (positiveBob) {
					transform.Translate (0, -BobSpeed * Time.deltaTime, 0);
				}
				else {
					transform.Translate (0, BobSpeed * Time.deltaTime, 0);
				}

				if (Mathf.Abs (initialY - transform.position.y) < 2.5f) {
					Vector3 position = transform.position;
					position.y = initialY;
					transform.position = position;
				}
			}
		}
	}

	void RandomBobbing()
	{
		do {
			bobValue = Random.Range (MinBobRange, MaxBobRange);
			int rng = Random.Range (0, 2);

			Debug.Log ("RNG Value " + rng);

			if (rng == 1) {
				positiveBob = true;
			} else {
				positiveBob = false;
			}
		} 
		while(bobValue == 0);

		//Debug.Log ("Bob Value" + bobValue);
		Debug.Log ("Positive Bob " + positiveBob);

		amountBobbed = 0;
	}

	void FactorSpeed()
	{
		float currentspeed = waypointscript.currentSpeed;

		if(currentspeed != 0)
		{
			BobbingIntervals = 1 *(currentspeed / 1500);

			MaxBobRange = -(initialRange * (currentspeed / 150));
			MinBobRange = MaxBobRange - initialRange;
		}
	}
}
