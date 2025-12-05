using UnityEngine;
using Unity.Mathematics;

public class RailGenerator : MonoBehaviour
{
	// ----- Public
	public RailSection[] sections;
	public GameObject player;

	// ----- Private
	// References

	// Varyings
	private float3 endPosition = float3.zero;
	private float3 endDirection = Vector3.forward;
	
	void Update()
	{
		if (ShouldGenerate())
		{
			RailSection currentSection = sections[0];
			currentSection.GetPathStart(out float3 startPosition, out float3 startDirection);
			
			Instantiate(currentSection, endPosition, Quaternion.LookRotation(endDirection));

			currentSection.GetPathEnd(out endPosition, out endDirection);
		}
	}

	private bool ShouldGenerate()
	{
		if (math.abs(endPosition.y - player.transform.position.y) < 100)
		{
			return true;
		}
		return false;
	}
}
