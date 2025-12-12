using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
	// ----- Public
	public float2 sensitivity = new float2(10);
	public float maxAngle = 80;
	public float minAngle = -90;
	
	// ----- Private
	// References
	
	// Varyings
	private float2 lookAngle;
	
	public void OnLook(InputAction.CallbackContext context)
	{
		float2 mouseMove = new float2(context.ReadValue<Vector2>()) * sensitivity / 100;

		lookAngle += mouseMove;
		lookAngle.y = Mathf.Clamp(lookAngle.y, minAngle, maxAngle);
		
		// Apply rotation
		transform.rotation = Quaternion.identity;
		transform.Rotate(Vector3.up, lookAngle.x);
		transform.Rotate(Vector3.right, -lookAngle.y);
		
		Cursor.lockState = CursorLockMode.Locked;
	}
}
