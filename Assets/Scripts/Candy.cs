using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public Color colorSelected;
    public int id;
    public static Candy oldCandySelected;

    [SerializeReference] bool isSelected = false;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    Vector2[] UpDownLeftRight = new Vector2[] {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        animator.SetBool("destroy", false);
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
        if (ManagerGIU.instance.inPlay)
        {
            if (animator.GetBool("destroy") || ManagerCandies.instance.isShifting)
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
                        ManagerGIU.instance.Moves--;
                        SwapSprinte(oldCandySelected);
                        oldCandySelected.FindallMatche();
                        FindallMatche();
                        oldCandySelected.DeselectCandy();

                    }
                    else
                    {
                        oldCandySelected.DeselectCandy();
                        SelectCandy();
                    }

                }
            }
        }

    }

    private void SwapSprinte(Candy newCandy)
    {
        /*  if(spriteRenderer.sprite == 
            newCandy.GetComponent<SpriteRenderer>().sprite) {
            return;
        } */

        if (spriteRenderer.sprite != newCandy.spriteRenderer.sprite)
        {
            Sprite tempoSprite = newCandy.spriteRenderer.sprite;
            RuntimeAnimatorController animatorController = newCandy.animator.runtimeAnimatorController;
            int id = newCandy.id;

            newCandy.spriteRenderer.sprite = this.spriteRenderer.sprite;
            newCandy.animator.runtimeAnimatorController = this.animator.runtimeAnimatorController;
            newCandy.id = this.id;

            this.spriteRenderer.sprite = tempoSprite;
            this.animator.runtimeAnimatorController = animatorController;
            this.id = id;

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

    private List<GameObject> FindMatch(Vector2 direction, Transform position)
    {
        List<GameObject> result = new List<GameObject>();

        RaycastHit2D rayHit = Physics2D.Raycast(position.position, direction);
        while (rayHit.collider != null &&
            rayHit.collider.GetComponent<Candy>().id ==
            id)
        {
            result.Add(rayHit.collider.gameObject);
            rayHit = Physics2D.Raycast(rayHit.collider.transform.position, direction);

        }

        return result;
    }

    private bool ClearMatch(Vector2[] directions)
    {
        HashSet<GameObject> matchCandies = new HashSet<GameObject>();
        List<GameObject> temporalMatchCandies = new List<GameObject>();
        List<GameObject> temporal = new List<GameObject>();

        foreach (Vector2 direction in directions)
        {
            temporalMatchCandies.AddRange(FindMatch(direction, this.GetComponent<Transform>()));
        }
        matchCandies.UnionWith(temporalMatchCandies);

        foreach (GameObject gameObject in temporalMatchCandies)
        {
            foreach (Vector2 direction in directions)
            {
                temporal.AddRange(FindMatch(direction, gameObject.GetComponent<Transform>()));
            }
        }
        temporalMatchCandies = temporal;
        temporal = new List<GameObject>();

        matchCandies.Add(this.gameObject);
        matchCandies.UnionWith(temporalMatchCandies);

        if (matchCandies.Count >= ManagerCandies.minToMach)
        {

            /* this.GetComponent<Animator> ().SetBool ("destroy", true); */
            foreach (GameObject candy in matchCandies)
            {
                candy.GetComponent<Animator>().SetBool("destroy", true);
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

        if (this.animator.GetBool("destroy"))
        {
            return;

        }
        bool isMatch = ClearMatch(new Vector2[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left });

        if (isMatch)
        {

            ManagerGIU.instance.Moves++;
            /* this.GetComponent<Animator>().SetBool("destroy", true); */
            Invoke("starFindNullCandies", 2f);

        }
    }

    public void starFindNullCandies()
    {
        StopCoroutine(ManagerCandies.instance.FindNullCandi());
        StartCoroutine(ManagerCandies.instance.FindNullCandi());
    }

}