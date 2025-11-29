using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
	// ----- Public
	public Vector2 sensitivity = Vector2.one * 10;
	public float maxAngle = 80;
	public float minAngle = -90;
	
	public void OnLook(InputAction.CallbackContext context)
	{
		Vector2 mouseMove = context.ReadValue<Vector2>() * sensitivity / 100;
		
		
		transform.Rotate(Vector3.up, mouseMove.x);
		transform.Rotate(transform.right, -mouseMove.y);
		
		
		print("Axis: " + transform.forward);
		print("Quat: " + transform.rotation);
		print("Rota: " + transform.eulerAngles);
	}
}
