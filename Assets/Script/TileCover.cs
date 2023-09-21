using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveArrow
{
    none,
    upper,
    lower,
    lu,
    ru,
    ld,
    rd
}
public class TileCover : MonoBehaviour
{

    public SpriteRenderer sprite;
    public Sprite[] sprites;
    private MoveArrow moveArrow;
    public MoveArrow _MoveArrow
    {
        get => moveArrow;
        set
        {
            moveArrow = value;
            sprite.sprite = sprites[(int)value];
        }
    }
}
