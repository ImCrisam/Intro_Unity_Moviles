using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCandies : MonoBehaviour
{
    public static ManagerCandies instance;
    public List<Sprite> listPreFabs = new List<Sprite>();
    public GameObject currentCandy;
    public int col, row;
    public const int minToMach = 2;
    private GameObject[,] candies;
    public bool isShifting { get; set; }

    private Candy candySelected;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Vector2 offSet = currentCandy.GetComponent<BoxCollider2D>().size;
        init(offSet);
    }

    private void init(Vector2 offset)
    {
        candies = new GameObject[col, row];
        GameObject temporalCandy;
        Sprite temporalSprite;
        float intix = this.transform.position.x;
        float intiy = this.transform.position.y;
        int idx = -1;
        for (int x = 0; col > x; x++)
        {
            for (int y = 0; row > y; y++)
            {
                temporalCandy = Instantiate(
                                currentCandy,
                                new Vector3(intix + (offset.x * x),
                                             intiy + (offset.y * y),
                                             0),
                                currentCandy.transform.rotation);
                temporalCandy.name = string.Format("Candy[{0}][{1}]", x, y);
                do
                {
                    idx = Random.Range(0, listPreFabs.Count);
                } while ((x > 0 && idx == candies[x - 1, y].GetComponent<Candy>().id) ||
                            (y > 0 && idx == candies[x, y - 1].GetComponent<Candy>().id));


                temporalSprite = listPreFabs[idx];
                temporalCandy.GetComponent<SpriteRenderer>().sprite = temporalSprite;
                temporalCandy.GetComponent<Candy>().id = idx;
                temporalCandy.transform.parent = this.transform;
                candies[x, y] = temporalCandy;
            }
        }
    }

    private List<GameObject> FindMach(Vector2 direction)
    {
        List<GameObject> result = new List<GameObject>();

        return result;
    }
    public IEnumerator FindNullCandi()
    {
        for (int x = 0; x < col; x++)
        {
            for (int y = 0; y < row; y++)
            {
                if (candies[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(MakeCandisFall(x, y));
                    break;
                }


            }
        }

    }

    private IEnumerator MakeCandisFall(int i,
                                  int j,
                                  float delay = 0.5f)
    {
        isShifting = true;
        List<SpriteRenderer> renderes = new List<SpriteRenderer>();
        int nullCandis = 0;
        for (int y = j; y < row; y++)
        {
            SpriteRenderer spriteRenderer = candies[i, y].GetComponent<SpriteRenderer>();
            if (spriteRenderer.sprite == null)
            {
                nullCandis++;
            }
            renderes.Add(spriteRenderer);

        }
        for (int k = 0; k < nullCandis; k++)
        {
            yield return new WaitForSeconds(delay);
            for (int l = 0; l < renderes.Count - 1; l++)
            {

                renderes[l].sprite = renderes[l + 1].sprite;

                renderes[l + 1].sprite = GetNewSprite(k, row - 1);
            }
        }

        isShifting = false;
    }

    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleCandies = new List<Sprite>(listPreFabs);

        if (x > 0)
        {
            possibleCandies.Remove(candies[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x > col - 1)
        {
            possibleCandies.Remove(candies[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleCandies.Remove(candies[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possibleCandies[Random.Range(0, possibleCandies.Count)];
    }
}
