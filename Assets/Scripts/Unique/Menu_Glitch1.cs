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
	private RectTransform rect;
	private RawImage image;
	
	// Varyings
	private float timeOut = 0;

	void Awake()
	{
		rect = GetComponent<RectTransform>();
		image = GetComponent<RawImage>();
	}

	void Start()
	{
		rect.sizeDelta = new Vector2(Screen.width, Screen.height);
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