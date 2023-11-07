using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
	public Text fpsText;
	private float deltaTime = 0.0f;
    private int frameCount = 0;

	// Update is called once per frame
	void Update()
	{
		deltaTime += Time.deltaTime;
		frameCount++;
		if (deltaTime >= 1.0f)
		{
			float fps = (float)frameCount / deltaTime;
			fpsText.text = "FPS: " + Mathf.RoundToInt(fps);
			deltaTime = 0.0f;
			frameCount = 0;
		}
	}
}