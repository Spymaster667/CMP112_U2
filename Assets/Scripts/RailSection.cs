using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEditor.Splines;

public class RailSection : MonoBehaviour
{
	// ----- Public
	
	// ----- Private
	// References
	private SplineContainer path;

	void OnDrawGizmos()
	{
		// Start
		float3 knotPos = path.Spline[0].Position + new float3(transform.position);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(knotPos, 0.1f);
		Gizmos.DrawLine(knotPos, knotPos - path.Spline.EvaluateTangent(0.001f));
		
		// End
		knotPos = path.Spline[1].Position + new float3(transform.position);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(knotPos, 0.1f);
		Gizmos.DrawLine(knotPos, knotPos + path.Spline.EvaluateTangent(.999f));
	}
	
	void Start()
	{
		path = GetComponent<SplineContainer>();
	}

	public void GetPathEnd(out float3 endPosition, out float3 endDirection)
	{
		endPosition = path.Spline[^1].Position;
		endDirection = path.Spline[^1].TangentOut;
		print(endDirection);
	}

	public void GetPathStart(out float3 startPosition, out float3 startDirection)
	{
		startPosition = path.Spline[0].Position;
		startDirection = path.Spline[0].TangentIn;
	}
}