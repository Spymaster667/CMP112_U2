using UnityEngine;
using Unity.Mathematics;

public class RailGenerator : MonoBehaviour
{
	// ----- Public
	public GameObject[] sections;
	public GameObject player;

	[Header("Limits")]
	
	public uint maxSections;
	public float maxDistance;

	// ----- Private
	// References

	// Varyings
	private float3 endPosition = float3.zero;
	private float3 endDirection = Vector3.forward;

	private uint sectionCount = 0;
	
	void Update()
	{
		if (ShouldGenerate())
		{
			sectionCount++;
			// choose section
			GameObject selectedSection = sections[0];
			
			// instance section
			RailSection currentSection = Instantiate(selectedSection, endPosition, Quaternion.LookRotation(endDirection)).GetComponent<RailSection>();
			
			// offset section
			//currentSection.GetPathStart(out float3 startPosition, out float3 startDirection);
			
			
			
			
			//currentSection.GetPathEnd(out endPosition, out endDirection);
			print("Count " + sectionCount);
		}
	}

	private bool ShouldGenerate()
	{
		if (sectionCount >= maxSections)
		{
			return false;
		}
		if (math.abs(endPosition.y - player.transform.position.y) > maxDistance)
		{
			return false;
		}
		
		
		return true;
	}
}
