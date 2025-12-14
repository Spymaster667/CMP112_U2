using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class checkr : MonoBehaviour
{
	public GameObject partner;
	public SplineContainer me;

	private void OnDrawGizmos()
	{
		float3 partnerPos = partner.transform.position - transform.position;
		
		
		SplineUtility.GetNearestPoint(me.Spline, partnerPos, out float3 pointPosition, out float pointT, 4, 4);
		pointPosition += new float3(transform.position);
		
		Gizmos.color =  Color.red;
		Gizmos.DrawSphere(pointPosition, 0.2f);
		Gizmos.DrawLine(pointPosition, partner.transform.position);

		float3 evalPosition = me.Spline.EvaluatePosition(pointT);
		evalPosition += new float3(transform.position);
		
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(pointPosition, 0.1f);
		Gizmos.DrawLine(evalPosition, partner.transform.position);
	}
}