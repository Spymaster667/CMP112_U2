using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Splines;
using System.Collections.Generic;

public class RailGenerator : MonoBehaviour
{
	// ----- Public
	public GameObject[] sections = {};
	public GameObject player;

	[Header("Limits")]

	public uint maxSections = 0;
	public float maxDistance = 100;
	
	// ----- Private
	// References

	// Varyings
	private float3 endPosition = float3.zero;
	private Quaternion endRotation = Quaternion.identity;
	
	private List<RailSection> instances = new List<RailSection>();

	void Start()
	{
		RailSection first = GenerateSection(sections[0]);
		player.GetComponentInChildren<PlayerMove>().AttachToRail(first);
	}
	
	void Update()
	{
		if (ShouldGenerate())
		{
			GenerateSection(sections[0]);
		}
	}

	private bool ShouldGenerate()
	{
		if (maxSections >= 0)
		{
			if (instances.Count >= maxSections)
			{
				return false;
			}
		}

		if (maxDistance >= 0)
		{
			if (Vector2.Distance(endPosition.xy,float2.zero) > maxDistance)
			{
				return false;
			}
		}

		return true;
	}

	
	RailSection GenerateSection(GameObject selectedSection)
	{
		// instance section
		RailSection currentSection =
			Instantiate(selectedSection, float3.zero, Quaternion.identity)
			.GetComponent<RailSection>();

		// offset section
		currentSection.GetPathStart(out float3 startPosition, out Quaternion startRotation);
		
		currentSection.transform.position = endPosition - startPosition;
		
		currentSection.transform.rotation = endRotation * startRotation;
		
		// Assign next and previous
		if (instances.Count > 0)
		{
			currentSection.previous = instances[^1];
			instances[^1].next = currentSection;
		}
		
		// update end of the path and instances[]
		currentSection.GetPathEnd(out endPosition, out endRotation);
		instances.Add(currentSection);
		
		return currentSection;
	}
}