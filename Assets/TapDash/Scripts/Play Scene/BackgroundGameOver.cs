using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundGameOver : MonoBehaviour
{
	//for game over , the background is shown
    public Image background;

    private Color color;

    void Update()
    {
        background.color = Color.Lerp(background.color, color, Time.time);
    }

    void OnEnable()
    {
        background.color = Camera.main.GetComponent<Camera>().backgroundColor;
        color = new Color(background.color.r, background.color.g, background.color.b, 190.0f / 255.0f);
    }
}
