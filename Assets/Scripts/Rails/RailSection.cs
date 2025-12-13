using System;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class RailSection : MonoBehaviour
{
	// ----- Public
	
	// ----- Private
	// References
	[NonSerialized] public SplineContainer container;
	

	void OnDrawGizmos()
	{
		container = GetComponent<SplineContainer>();
		
		// Start
		float3 knotPos = transform.TransformPoint(container.Spline[0].Position);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(knotPos, 0.1f);
		Gizmos.DrawLine(
			knotPos, 
			knotPos - new float3(transform.rotation * container.Spline.EvaluateTangent(0f))
		);
		
		// End
		knotPos = transform.TransformPoint(container.Spline[1].Position);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(knotPos, 0.1f);
		Gizmos.DrawLine(
			knotPos, 
			knotPos + new float3(transform.rotation * container.Spline.EvaluateTangent(1f))
		);
	}
	
	void Awake()
	{
		container = GetComponent<SplineContainer>();
	}

	// GetPath...
	public void GetPathStart(out float3 startPosition, out Quaternion startRotation)
	{
		startPosition = container.Spline[0].Position;
		startRotation = container.Spline[0].Rotation;
	}

	public void GetPathEnd(out float3 endPosition, out Quaternion endRotation)
	{
		endPosition = transform.TransformPoint(container.Spline[^1].Position);
		endRotation = transform.rotation * container.Spline[^1].Rotation;
	}
}