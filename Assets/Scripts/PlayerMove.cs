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
		float3 localPos = rail.transform.InverseTransformPoint(transform.position);
		SplineUtility.GetNearestPoint(rail.container.Spline, localPos, out float3 nearestPos, out float nearestT);
		print("localPos: " + localPos + " | nearestPos: " + nearestPos);
		nearestPos = transform.TransformPoint(nearestPos);
		print("nearestPos TransformPoint: " + nearestPos);
		float3 railDirection = Vector3.Normalize(rail.container.Spline.EvaluateTangent(nearestT));
		
		// Switch rails
		if (nearestT >= .97 - float.Epsilon)
		{
			//print("through");
			if (rail.next)
			{
				//print("it happened " + rail.next.transform.position);
				if (tester)
				{
					//print("tester passed");
					AttachToRail(rail.next);
					tester = false;
				}
			}
		}
		else if (nearestT < float.Epsilon)
		{
			if (rail.previous)
			{
				//print("bounce");
				AttachToRail(rail.previous);
			}
		}
		
		// Apply movement
		rb.MovePosition(nearestPos + (railDirection * velocity));
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}
	
	public void AttachToRail(RailSection section)
	{
		print("Attaching!!");
		rail = section;
		//print("Attached==to: " + rail + " | at: " + railPosition);
		//.GetNearestPoint(rail.container.Spline, transform.position, out float3 nearestPos, out float nearestT);
		//print("Attached==NearestPos: " + nearestPos + " | nearestT: " + nearestT);
	}
}