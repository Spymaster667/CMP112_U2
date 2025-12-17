using UnityEngine;
using UnityEngine.UI;

public class Menu_Glitch1 : MonoBehaviour
{
	// ----- Public
	public float maxDelay = 20f;
	public float minDelay = .2f;

	public float maxFlash = 1.5f;
	public float minFlash = .1f;

	// ----- Private
	// References
	private RawImage image;
	
	// Varyings
	private float timeOut = 0;

	void Awake()
	{
		image = GetComponent<RawImage>();
	}

	void Update()
	{
		if (Time.time > timeOut)
		{
			if (image.enabled)
			{
				timeOut = Time.time + Random.Range(minDelay, maxDelay);
				image.enabled = false;
			}
			else
			{
				timeOut = Time.time + Random.Range(minFlash, maxFlash);
				image.enabled = true;
			}
		}
	}
}