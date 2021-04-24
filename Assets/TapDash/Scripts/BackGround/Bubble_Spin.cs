using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble_Spin : MonoBehaviour
{
    public static float WIDTH = 200f;
    public static float HEIGHT = 380f;

    private float speed = 0.2f;

    //Image img;

    //public float targetBrightness = 0.7f;

    Vector3 startPos;
    Vector3 targetPos;
    //Color targetColor;

    //float currentHue;

    private void Start()
    {
        //currentHue = Random.Range(0f, 1f);

        //img = GetComponent<Image>();
        //img.GetComponent<RectTransform>().anchoredPosition = new Vector3(Random.Range(-WIDTH / 2, WIDTH), Random.Range(-HEIGHT / 2, HEIGHT));
        //img.GetComponent<CanvasGroup>().alpha = 1;
        //img.color = Color.HSVToRGB(Random.Range(0, 1f), 0.7f, 0.7f);
        //targetColor = img.color;
        //startPos = img.rectTransform.anchoredPosition;
        //startPos = transform.position;

        //StartCoroutine(ChangeStarPosition());
    }

    IEnumerator ChangeStarPosition()
    {
        while (true)
        {
            //Vector3 camPos = new Vector3(Random.Range(0, WIDTH), Random.Range(0, HEIGHT));
            //targetPos = Camera.main.ScreenToWorldPoint(camPos);
            //targetPos.z = 0;
            targetPos = transform.position + new Vector3(5f, 2f);


            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    private void Update()
    {
        //Color.RGBToHSV(img.color, out float hue, out float saturation, out float value);
        //img.rectTransform.anchoredPosition += ((Vector2)targetPos - img.rectTransform.anchoredPosition).normalized * Time.deltaTime * speed;
        //transform.position -= ((Vector3)targetPos - transform.position).normalized * Time.deltaTime * speed;
        transform.localPosition += new Vector3(5f, 2f) * Time.deltaTime * speed;

        if (transform.localPosition.x > 15f || transform.localPosition.y > -6f) {
            transform.localPosition = new Vector3(Random.Range(-15f, 1f), Random.Range(-15f, -11f));
        }

        //float r = (img.color.r > targetColor.r) ? img.color.r - Time.deltaTime : targetColor.r;
        //float g = (img.color.g > targetColor.g) ? img.color.g - Time.deltaTime : targetColor.g;
        //float b = (img.color.b > targetColor.b) ? img.color.b - Time.deltaTime : targetColor.b;

        //img.color = new Color(r, g, b);
    }

    //public void DoNoteEffect()
    //{
    //    Color.RGBToHSV(img.color, out float hue, out float saturation, out float value);
    //    img.color = new Color(0.7f, 0.7f, 0.7f);
    //}
}
