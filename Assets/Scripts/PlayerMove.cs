using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

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
	private AudioSource[] sources;
	
	// Varyings
	private float2 moveInput = float2.zero;
	private float velocity = 0;

	private float3 railPosition = float3.zero;

	private bool playAccelerating = true;

	private void Start()
	{
		AttachToRail(rail);
		rb = GetComponent<Rigidbody>();
		
		sources = GetComponents<AudioSource>();
	}
	
	private void Update()
	{
		// Calculate velocity
		float accelDelta = maxSpeed * Time.deltaTime * accelerationTime;
		if (math.abs(moveInput.x) > Mathf.Epsilon || math.abs(moveInput.y) > Mathf.Epsilon)
		{
			velocity = Mathf.MoveTowards(velocity, maxSpeed * moveInput.y, accelDelta);
			PlayCarriageStart();
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
		rb.MovePosition(nearestPos);
		rb.MovePosition(new float3(rb.position) + railDirection * velocity);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}
	
	public void AttachToRail(SplineContainer container)
	{
		rail = container;
		railPosition = container.transform.position;
	}

	private void PlayCarriageStart()
	{
		// reset volume and clip on low volume
		if (sources[0].volume < 1 - float.Epsilon)
		{
			sources[0].Stop();
			sources[0].volume = 1;
			playAccelerating = true;
		}
		
		// don't change if playing
		if (sources[0].isPlaying)
		{
			return;
		}

		// play accelerating or drone
		if (playAccelerating)
		{
			sources[0].PlayOneShot(carriageAccel);
			playAccelerating = false;
		}
		else
		{
			sources[0].loop = true;
			sources[0].clip = carriageDrone;
			sources[0].Play();
		}

		return;
	}

	private void PlayCarriageStop()
	{
		sources[0].volume = Mathf.MoveTowards(sources[0].volume, 0, droneToDecelFade * Time.deltaTime);

		if (sources[1].isPlaying)
		{
			return;
		}
		
		sources[1].PlayOneShot(carriageDecel);
	}
}