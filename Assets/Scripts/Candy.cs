using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public Color colorSelected;
    public int id;
    private static Candy oldCandySelected;

    [SerializeReference] bool isSelected = false;
    SpriteRenderer spriteRenderer;

    Vector2[] UpDownLeftRight = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void SelectCandy()
    {
        isSelected = true;
        spriteRenderer.color = colorSelected;
        oldCandySelected = gameObject.GetComponent<Candy>();
    }
    private void DeselectCandy()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        oldCandySelected = null;
    }
    private void OnMouseDown()
    {
        if (spriteRenderer.sprite == null || ManagerCandies.instance.isShifting)
        {
            return;
        }
        if (isSelected)
        {
            DeselectCandy();
        }
        else
        {
            if (oldCandySelected == null)
            {
                SelectCandy();
            }
            else
            {
                oldCandySelected.DeselectCandy();
            }
        }

    }

}
