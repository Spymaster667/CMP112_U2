using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines;

public class PlayerMove : MonoBehaviour
{
	// ----- Public
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

	public RailSection rail;
	private float3 railPosition = float3.zero;
	
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		sources = GetComponents<AudioSource>();
	}

	public bool tester = true;
	
	void Update()
	{
		// Calculate velocwity
		float accelDelta = maxSpeed * Time.deltaTime * accelerationTime;
		if (math.abs(moveInput.x) > Mathf.Epsilon || math.abs(moveInput.y) > Mathf.Epsilon)
		{
			velocity = Mathf.MoveTowards(velocity, maxSpeed * moveInput.y, accelDelta);
		}
		else
		{
			velocity = Mathf.MoveTowards(velocity, 0, accelDelta);
		}
		
		velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);
		
		// Get rail info
		SplineUtility.GetNearestPoint(rail.container.Spline, transform.position, out float3 nearestPos, out float nearestT);
		float3 railDirection = Vector3.Normalize(rail.container.Spline.EvaluateTangent(nearestT));
		nearestPos += railPosition;
		print("nearestT: " + nearestT + " | railPosition: " + railPosition + " | nearestPos: " + nearestPos);
		
		// Apply movement
		rb.MovePosition(nearestPos + (railDirection * velocity));
		
		// Switch rails
		if (nearestT >= .95 - float.Epsilon)
		{
			print("through");
			if (rail.next)
			{
				print("it happened " + rail.next.transform.position);
				if (tester)
				{
					print("tester passed");
					AttachToRail(rail.next);
					tester = false;
				}
			}
		}
		else if (nearestT < float.Epsilon)
		{
			if (rail.previous)
			{
				print("bounce");
				AttachToRail(rail.previous);
			}
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}
	
	public void AttachToRail(RailSection section)
	{
		print("Attaching!!");
		rail = section;
		railPosition = section.transform.position;
		print("Attached to: " + rail + " | at: " + railPosition);
	}
}