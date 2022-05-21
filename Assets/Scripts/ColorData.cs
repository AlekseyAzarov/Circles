using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Color Data", fileName = "Color Data")]
public class ColorData : ScriptableObject
{
    [SerializeField] private Color[] _color;

    public Color GetRandomColor()
    {
        int index = Random.Range(0, _color.Length);
        return _color[index];
    }
}
