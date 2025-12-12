using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;

public class ScreenSetup : MonoBehaviour
{
	// ----- Public
	public float2 aspectRatio = new float2(1.2f, 1f);
	public int pixelSize = 2;
	public GameObject screenObject;

	public bool useTestCard = false;

	// ----- Private
	// References
	private RawImage image;
	private RectTransform rect;
	private Camera cam;

	// Varyings
	private RenderTexture texture;
	
	void Awake()
	{
		cam = GetComponent<Camera>();
		
		image = screenObject.GetComponent<RawImage>();
		rect = screenObject.GetComponent<RectTransform>();
	}

	void Start()
	{
		RefreshScreen();
		
		// Assign Texture
		cam.depthTextureMode = DepthTextureMode.Depth;
		
		cam.targetTexture = texture;
		if (!useTestCard)
		{
			image.texture = texture;
		}
	}
	
	void RefreshScreen()
	{
		// Resize aspectRatio
		int screenMin = Mathf.Min(Screen.width, Screen.height);
		float2 scaledRatio = aspectRatio/Mathf.Min(aspectRatio.x, aspectRatio.y); // aspectRatio scaled such that the min component must be 1
		int2 textureResolution = new int2(screenMin * scaledRatio / pixelSize);
		
		// Apply Scale
		texture = new RenderTexture(textureResolution.x, textureResolution.y, 24)
		{
			filterMode = FilterMode.Point
		};
		
		
		rect.sizeDelta = math.floor(screenMin * scaledRatio);
	}
}