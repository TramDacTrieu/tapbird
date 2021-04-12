using UnityEngine;
using System.Collections;

public class ShowUpArrow : MonoBehaviour
{
    public SpriteRenderer arrowActive;

    void Update()
    {
        arrowActive.color = Color.Lerp(arrowActive.color, Color.white, Time.time / 7.5f);
        if (arrowActive.color == Color.white)
        {
            this.enabled = false;
        }
    }
}
