using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerMove : MonoBehaviour
{
	// ----- Public
	public SplineContainer rail;
	public float accelerationTime = 1f;
	public float maxSpeed = .01f;
	
	// ----- Private
	// References
	private Rigidbody rb;
	private AudioSource as0;
	private AudioSource as1;
	
	// Varyings
	private float2 moveInput;
	private float velocity;

	private float3 railPosition = float3.zero;

	private bool asIdentifier;

	private void Start()
	{
		AttachToRail(rail);
		rb = GetComponent<Rigidbody>();
		
		as0 = GetComponent<AudioSource>();
		as1 = GetComponent<AudioSource>();
		
		print(as0);
		print(as1);
	}
	
	private void Update()
	{
		// Calculate velocity
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
}