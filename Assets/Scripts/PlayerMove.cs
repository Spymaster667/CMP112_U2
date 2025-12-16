using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
// ReSharper disable All

public class PlayerMove : MonoBehaviour
{
	// ----- Public
	public SplineContainer rail;
	public float accelerationTime = 1f;
	public float maxSpeed = .01f;
	
	// Audio
	[Header("Audio")]

	public AudioClip carriageAccel;
	public AudioClip carriageDrone;
	public AudioClip carriageDecel;
	public AudioClip carriageStop;

	public float droneToDecelFade = 0.1f;
	
	// ----- Private
	// References
	private Rigidbody rb;
	private AudioSource droneSource;
	private AudioSource decelSource;
	private AudioSource accelSource;
	
	// Varyings
	private float2 moveInput = float2.zero;
	private float velocity = 0;

	private float3 railPosition = float3.zero;
	
	// Audio Varyings
	private enum DroneState
	{
		STOP,
		ACCEL,
		MOVE,
		DECEL
	};
	private DroneState droneState = DroneState.STOP;
	
	// ----- Functions
	
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		
		droneSource = gameObject.AddComponent<AudioSource>();
		droneSource.clip = carriageDrone;
		droneSource.loop = true;
		decelSource = gameObject.AddComponent<AudioSource>();
		decelSource.clip = carriageDecel;
		accelSource = gameObject.AddComponent<AudioSource>();
		accelSource.clip = carriageAccel;
	}

	void Start()
	{
		AttachToRail(rail);
	}
	
	void Update()
	{
		// Calculate velocity
		float accelDelta = maxSpeed * Time.deltaTime * accelerationTime;
		if (Mathf.Abs(moveInput.x) > float.Epsilon || Mathf.Abs(moveInput.y) > float.Epsilon)
		{
			velocity = Mathf.MoveTowards(velocity, maxSpeed * moveInput.y, accelDelta);
		}
		else
		{
			velocity = Mathf.MoveTowards(velocity, 0, accelDelta);
		}
		
		velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);
		
		// Get rail info
		SplineUtility.GetNearestPoint(rail.Spline, transform.position, out float3 nearestPos, out float nearestT);
		float3 railDirection = Vector3.Normalize(rail.Spline.EvaluateTangent(nearestT));
		nearestPos += railPosition;
		
		// Apply movement
		rb.MovePosition(nearestPos + railDirection * velocity);

		// Drone
		SetDroneState();
		
		switch (droneState)
		{
		case DroneState.STOP:
			break;
		case DroneState.ACCEL:
			decelSource.Stop();
			if (!accelSource.isPlaying)
			{
				accelSource.Play();
			}
			break;
		case DroneState.MOVE:
			accelSource.Stop();
			if (!droneSource.isPlaying)
			{
				droneSource.Play();
			}
			break;
		case DroneState.DECEL:
			droneSource.Stop();
			accelSource.Stop();
			if (!decelSource.isPlaying)
			{
				decelSource.PlayOneShot(decelSource.clip);
			}
			break;
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}
	
	// ReSharper disable once MemberCanBePrivate.Global
	public void AttachToRail(SplineContainer container)
	{
		rail = container;
		railPosition = container.transform.position;
	}

	private void SetDroneState()
	{
		if (Mathf.Abs(velocity) < float.Epsilon)
		{
			droneState = DroneState.STOP;
			return;
		}

		if (Mathf.Abs(velocity) >= maxSpeed - float.Epsilon)
		{
			droneState = DroneState.MOVE;
			return;
		}
		
		if ((int)moveInput.y == (int)Mathf.Sign(velocity))
		{
			droneState = DroneState.ACCEL;
			return;
		}
		
		if (Mathf.Abs(moveInput.y) < float.Epsilon)
		{
			droneState = DroneState.DECEL;
			return;
		}
	}
}