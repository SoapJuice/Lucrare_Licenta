using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : MonoBehaviour
{
    [SerializeField]
    private Color primaryColor, secondaryColor;
    [SerializeField]
    private SpriteRenderer renderer;

    public void PatternColor(bool pattern)
    {
        renderer.color = pattern ? primaryColor : secondaryColor;
    }
    public void PatternColor(bool pattern, Color primaryColor, Color secondaryColor)
    {
        renderer.color = pattern ? primaryColor : secondaryColor;
    }

    public void SingleColor(Color color)
    {
        renderer.color = color;
    }
}
