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
                if (CanSwipe(oldCandySelected))
                {
                    SwapSprinte(oldCandySelected);
                    oldCandySelected.FindallMatche();
                    oldCandySelected.DeselectCandy();
                    FindallMatche();
                }
                else
                {
                    oldCandySelected.DeselectCandy();
                    SelectCandy();
                }

            }
        }

    }

    private void SwapSprinte(Candy newCandy)
    {

        if (spriteRenderer.sprite != newCandy.spriteRenderer.sprite)
        {
            Sprite tempoSprite = newCandy.spriteRenderer.sprite;
            newCandy.spriteRenderer.sprite = this.spriteRenderer.sprite;
            this.spriteRenderer.sprite = tempoSprite;
            int tempoId = newCandy.id;
            newCandy.id = this.id;
            this.id = tempoId;
        }

    }

    private GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D rayHit = Physics2D.Raycast(this.transform.position, direction);
        return rayHit.collider != null ? rayHit.collider.gameObject : null;
    }

    private List<GameObject> GetAllNeighbor()
    {
        List<GameObject> result = new List<GameObject>();
        foreach (Vector2 v in UpDownLeftRight)
        {
            result.Add(GetNeighbor(v));
        }
        return result;
    }

    private bool CanSwipe(Candy candy)
    {
        return GetAllNeighbor().Contains(candy.gameObject);
    }


    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> result = new List<GameObject>();

        RaycastHit2D rayHit = Physics2D.Raycast(this.transform.position, direction);
        while (rayHit.collider != null &&
             rayHit.collider.GetComponent<SpriteRenderer>().sprite
                    == spriteRenderer.sprite)
        {
            result.Add(rayHit.collider.gameObject);
            rayHit = Physics2D.Raycast(rayHit.collider.transform.position, direction);

        }

        return result;
    }

    private bool ClearMatch(Vector2[] directions)
    {
        List<GameObject> matchCandies = new List<GameObject>();
        foreach (Vector2 direction in directions)
        {
            matchCandies.AddRange(FindMatch(direction));
        }

        if (matchCandies.Count >= ManagerCandies.minToMach)
        {
            foreach (GameObject candy in matchCandies)
            {
                candy.GetComponent<SpriteRenderer>().sprite = null;
            }

            return true;

        }
        else
        {
            return false;

        }

    }

    public void FindallMatche()
    {

        if (spriteRenderer.sprite == null)
        {
            return;

        }
        bool verticalMatch = ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        bool horizontalMatch = ClearMatch(new Vector2[2] { Vector2.right, Vector2.left });

        if (verticalMatch || horizontalMatch)
        {
            spriteRenderer.sprite = null;

        }
    }
}
