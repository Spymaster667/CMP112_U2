using UnityEngine;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScreenSetup : MonoBehaviour
{
	// ----- Public
	public float2 aspectRatio = new float2(1.2f, 1f);
	public int pixelSize = 2;

	public Material screenShader;
	public RawImage image;

	// ----- Private
	// References
	private Camera cam;

	// Varyings
	private RenderTexture texture;
	
	void Awake()
	{
		cam = GetComponent<Camera>();
		if (!image)
		{
			image = GetComponentInChildren<RawImage>();
		}
		if (!image)
		{
			throw new UnityException("No RawImage attached");
		}
	}

	void Start()
	{
		RefreshScreen();
		
		// Assign Texture
		cam.depthTextureMode |= DepthTextureMode.Depth;
		cam.targetTexture = texture;
		
		image.texture = texture;
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
			filterMode = FilterMode.Point,
			depthStencilFormat = GraphicsFormat.D32_SFloat_S8_UInt
		};
		texture.Create();
		
		image.rectTransform.sizeDelta = new float2(textureResolution * pixelSize);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, screenShader);
	}
}