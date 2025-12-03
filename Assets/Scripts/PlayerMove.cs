using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerMove : MonoBehaviour
{
	// ----- Public
	public SplineContainer rail;
	public float acceleration = 5f;
	
	// ----- Private
	// 
	
	// Varyings
	private Vector2 moveInput;
	private float velocity;

	private void Update()
	{
		velocity += moveInput.y * acceleration;
		
		Vector3 nearestPos = Vector3.zero;
		
		SplineUtility.GetNearestPoint(rail, out nearestPos, );
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}
}