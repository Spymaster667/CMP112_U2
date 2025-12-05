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
	
	void Start()
	{
		AttachToRail(rail);
		rb = GetComponent<Rigidbody>();
		
		sources = GetComponents<AudioSource>();
	}
	
	void Update()
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
		rb.MovePosition(nearestPos + railDirection * velocity);
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