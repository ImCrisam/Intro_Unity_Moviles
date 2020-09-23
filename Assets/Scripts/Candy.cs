using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public static Color ColorSelected;
    public int id;
    private static Candy OldCandySelected = null;

    bool isSelected = false;
    SpriteRenderer spriteRenderer;

    Vector2[] UpDownLeftRight = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
