using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoints : MonoBehaviour {

    public AudioClip Acceleration;
    public AudioClip AccelerationPeak;

    private AudioSource source;

	private Transform CameraTransform;

	private Vector3 Direction;

	private List<Transform> waypoints;

	public Transform waypoint;

	public int waypointIndex;

	private Quaternion ObjectRotation;

	float rotationDamping = 6.0f;

	private bool updateWaypoint = true;

    private bool playNext = false;

	float accelaration = 88.0f;
	float speedLimit = 88.0f;
	public float currentSpeed = 0.0f;

	// Use this for initialization
	void Start () {
		waypointIndex = 0;
		GetWayPoint ();
		CameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateRotate ();
		Move ();
        UpdateSound();
	}

    void UpdateSound()
    {
        if (!playNext)
        {
            playNext = true;
            source.PlayOneShot(Acceleration, 1f);
        }
        if (!source.isPlaying)
        {
            source.PlayOneShot(AccelerationPeak, 1f);
        }
    }

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Waypoint")) {
			ChangeNextWaypoint ();
		}
	}
		
	void UpdateRotate()
	{
		if (waypoint) {

			if (updateWaypoint) {
				transform.rotation = Quaternion.Lerp (transform.rotation, ObjectRotation, Time.deltaTime * rotationDamping);

				if (Quaternion.Angle (transform.rotation, ObjectRotation) < 2.5f) {
					transform.rotation = ObjectRotation;
					updateWaypoint = false;
				}
			}
		}
	}

	void Move()
	{
		currentSpeed = currentSpeed + accelaration * accelaration;

		Vector3 waypointPosition = waypoint.position;
		waypointPosition.y = transform.position.y;

		transform.position = Vector3.MoveTowards(transform.position, waypointPosition, Time.deltaTime * currentSpeed);

		if (currentSpeed >= speedLimit) {
			currentSpeed = speedLimit;
		}
	}

	void ChangeNextWaypoint()
	{
		waypointIndex++;

		if (waypointIndex >= waypoints.Count) {
			waypointIndex = 0;
		}

		waypoint = waypoints [waypointIndex];

		Direction = waypoint.position - transform.position;
		Direction.Normalize();

		updateWaypoint = true;

		ObjectRotation = Quaternion.LookRotation (Direction);
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
