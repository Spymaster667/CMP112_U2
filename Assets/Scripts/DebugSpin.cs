using UnityEngine;

public class DebugSpin : MonoBehaviour
{
	// ----- Public
	public float speed = 1;
	public Vector3 axis = Vector3.up;
	
	void Update()
	{
		transform.Rotate(axis, speed * Time.deltaTime);
	}
}