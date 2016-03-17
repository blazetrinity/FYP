using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoints : MonoBehaviour {

	private Transform CameraTransform;

	private Vector3 Direction, OldDirection;

	private List<Transform> waypoints;

	public Transform waypoint;

	public int waypointIndex;

	private GameObject lastCollidedGameObject;

	private Quaternion CameraRotation, ObjectRotation;

	float rotationDamping = 6.0f;

	private bool updateWaypoint = true;

	float accelaration = 50.0f;
	float speedLimit = 1000.0f;
	float currentSpeed = 0.0f;

	// Use this for initialization
	void Start () {
		waypointIndex = 0;
		GetWayPoint ();
		lastCollidedGameObject = null;
		CameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateRotate ();
		Move ();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject != lastCollidedGameObject) {
			ChangeNextWaypoint ();
			lastCollidedGameObject = other.gameObject;
		}
	}
		
	void UpdateRotate()
	{
		if (waypoint) {

			if (updateWaypoint) {
				transform.rotation = Quaternion.Lerp (transform.rotation, ObjectRotation, Time.deltaTime * rotationDamping);
				CameraTransform.rotation = Quaternion.Lerp (CameraTransform.rotation, CameraRotation, Time.deltaTime * rotationDamping);

				if (Quaternion.Angle (transform.rotation, ObjectRotation) < 2.5f) {
					transform.rotation = ObjectRotation;
					CameraTransform.rotation = CameraRotation;
					updateWaypoint = false;
				}
			}
		}
	}

	void Move()
	{
		currentSpeed = currentSpeed + accelaration * accelaration;

		transform.position = Vector3.MoveTowards(transform.position, waypoint.position, Time.deltaTime * currentSpeed);

		if (currentSpeed >= speedLimit) {
			currentSpeed = speedLimit;
		}
	}

	void ChangeNextWaypoint()
	{
		OldDirection = Direction;

		++waypointIndex;

		if (waypointIndex >= waypoints.Count) {
			waypointIndex = 0;
		}

		waypoint = waypoints [waypointIndex];

		Direction = waypoint.position - transform.position;
		Direction.Normalize();

		updateWaypoint = true;

		ObjectRotation = Quaternion.LookRotation (Direction);

		CalculateCameraRotation ();
	}

	void CalculateCameraRotation()
	{
		float angleA = Mathf.Atan2 (OldDirection.x, OldDirection.z) * Mathf.Rad2Deg;
		float angleB = Mathf.Atan2 (Direction.x, Direction.z) * Mathf.Rad2Deg;

		float angleDifference = Mathf.DeltaAngle (angleA, angleB);

		//Debug.Log ("Angle difference" + angleDifference);
		//Debug.Log ("Transform angle" + transform.rotation.eulerAngles.y);
		//Debug.Log ("Camera angle" + CameraTransform.rotation.eulerAngles.y);


		CameraRotation = Quaternion.AngleAxis (CameraTransform.rotation.eulerAngles.y + angleDifference, Vector3.up);

		//Debug.Log ("Camera supposed angle" + CameraRotation.eulerAngles.y);
	}

	void GetWayPoint()
	{
		Transform getwaypoints = waypoint.GetComponentInChildren<Transform>();
		waypoints = new List<Transform>();

		foreach (Transform findwaypoints in getwaypoints)
		{
			waypoints.Add(findwaypoints);
		}

		waypoint = waypoints [waypointIndex];
		Direction = waypoint.position - transform.position;
		Direction.Normalize();

		ObjectRotation = Quaternion.LookRotation (Direction);
	}
}
