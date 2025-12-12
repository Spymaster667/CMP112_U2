using System;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class RailSection : MonoBehaviour
{
	// ----- Public
	
	// ----- Private
	// References
	[NonSerialized] public SplineContainer path;

	void OnDrawGizmos()
	{
		path = GetComponent<SplineContainer>();
		
		// Start
		float3 knotPos = transform.TransformPoint(path.Spline[0].Position);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(knotPos, 0.1f);
		Gizmos.DrawLine(
			knotPos, 
			knotPos - new float3(transform.rotation * path.Spline.EvaluateTangent(0f))
		);
		
		// End
		knotPos = transform.TransformPoint(path.Spline[1].Position);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(knotPos, 0.1f);
		Gizmos.DrawLine(
			knotPos, 
			knotPos + new float3(transform.rotation * path.Spline.EvaluateTangent(1f))
		);
	}
	
	void Awake()
	{
		path = GetComponent<SplineContainer>();
	}

	// GetPath...
	public void GetPathStart(out float3 startPosition, out Quaternion startRotation)
	{
		startPosition = path.Spline[0].Position;
		startRotation = path.Spline[0].Rotation;
	}

	public void GetPathEnd(out float3 endPosition, out Quaternion endRotation)
	{
		endPosition = transform.TransformPoint(path.Spline[^1].Position);
		endRotation = transform.rotation * path.Spline[^1].Rotation;
	}
}