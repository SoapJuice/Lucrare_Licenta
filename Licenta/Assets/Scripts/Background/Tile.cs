using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Color primaryColor, secondaryColor;
    [SerializeField]
    private SpriteRenderer customRenderer;

    public void PatternColor(bool pattern)
    {
        customRenderer.color = pattern ? primaryColor : secondaryColor;
    }
    public void PatternColor(bool pattern, Color primaryColor, Color secondaryColor)
    {
        customRenderer.color = pattern ? primaryColor : secondaryColor;
    }

    public void SingleColor(Color color)
    {
        customRenderer.color = color;
    }
}
