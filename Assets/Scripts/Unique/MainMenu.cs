using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	// ----- Public
	public string playLevel = "SampleScene";
	
	// ----- Private
	// References

	public void OnPlay()
	{
		print("PLAY");
		SceneManager.LoadScene(playLevel);
	}

	public void OnQuit()
	{
		print("Hello");
		Application.Quit();
	}
}