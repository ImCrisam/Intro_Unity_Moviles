using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCandies : MonoBehaviour
{
    public static ManagerCandies instance;
    public List<GameObject> listPreFabs = new List<GameObject>();
    public GameObject currentCandy;
    public int col, row;
    public const int minToMach = 3;
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

    }
    public void newGame()
    {

        if (this.transform.childCount > 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {

                Destroy(this.transform.GetChild(i).gameObject);
            }
        }
        Vector2 offSet = currentCandy.GetComponent<BoxCollider2D>().size;
        init(offSet);
    }
    private void destroyGame()
    {

    }

    private void init(Vector2 offset)
    {
        candies = new GameObject[col, row];
        GameObject temporalCandy;
        float intix = this.transform.position.x;
        float intiy = this.transform.position.y;
        GameObject prefab;

        for (int x = 0; col > x; x++)
        {
            for (int y = 0; row > y; y++)
            {

                do
                {
                    prefab = listPreFabs[Random.Range(0, listPreFabs.Count)];

                } while ((x > 0 && prefab.GetComponent<Candy>().id == candies[x - 1, y].GetComponent<Candy>().id) ||
                    (y > 0 && prefab.GetComponent<Candy>().id == candies[x, y - 1].GetComponent<Candy>().id));
                temporalCandy = Instantiate(
                    prefab,
                    new Vector3(intix + (offset.x * x),
                        intiy + (offset.y * y),
                        0),
                    currentCandy.transform.rotation);
                temporalCandy.name = string.Format("Candy[{0}][{1}]", x, y);
                temporalCandy.GetComponent<Candy>().id = prefab.GetComponent<Candy>().id;

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
                if (candies[x, y].GetComponent<Animator>().GetBool("destroy"))
                {
                    yield return StartCoroutine(MakeCandisFall(x, y));
                    break;
                }

            }
        }
        for (int x = 0; x < col; x++)
        {
            for (int y = 0; y < row; y++)
            {
                candies[x, y].GetComponent<Candy>().FindallMatche();
            }
        }

    }

    private IEnumerator MakeCandisFall(int i,
        int j,
        float delay = 0.08f)
    {
        isShifting = true;
        List<Candy> candiesinFall = new List<Candy>();
        int nullCandis = 0;
        for (int y = j; y < row; y++)
        {

            GameObject candyObject = candies[i, y];
            if (candyObject.GetComponent<Animator>().GetBool("destroy"))
            {
                nullCandis++;
            }
            candiesinFall.Add(candyObject.GetComponent<Candy>());

        }

        for (int k = nullCandis; k > 0; k--)
        {
            ManagerGIU.instance.Score = ManagerGIU.instance.Score + 10;

            yield return new WaitForSeconds(delay);
            if (candiesinFall.Count > 1)
            {
                for (int l = 0; l < candiesinFall.Count - 1; l++)
                {
                    candiesinFall[l].animator.runtimeAnimatorController = candiesinFall[l + 1].animator.runtimeAnimatorController;
                    candiesinFall[l].spriteRenderer.sprite = candiesinFall[l + 1].spriteRenderer.sprite;
                    candiesinFall[l].id = candiesinFall[l + 1].id;


                    if (l == candiesinFall.Count - 2)
                    {


                        GameObject newObje = GetNewSprite(i, candiesinFall.Count - k + j);

                        candiesinFall[l + 1].animator.runtimeAnimatorController = newObje.GetComponent<Animator>().runtimeAnimatorController;
                        candiesinFall[l + 1].spriteRenderer.sprite = newObje.GetComponent<SpriteRenderer>().sprite;
                        candiesinFall[l + 1].id = newObje.GetComponent<Candy>().id;
                    }

                }
            }
            else
            {

                GameObject newObje = GetNewSprite(i, j);

                candiesinFall[0].animator.runtimeAnimatorController = newObje.GetComponent<Animator>().runtimeAnimatorController;
                candiesinFall[0].spriteRenderer.sprite = newObje.GetComponent<SpriteRenderer>().sprite;
                candiesinFall[0].id = newObje.GetComponent<Candy>().id;
            }
        }
        isShifting = false;
    }

    private GameObject GetNewSprite(int x, int y)
    {

        List<GameObject> possibleCandies = new List<GameObject>(listPreFabs);
        if (x > 0)
        {
            /* possibleCandies.Remove(candies[x - 1, y]); */
            possibleCandies.RemoveAll(candy => candy.GetComponent<Candy>().id == candies[x - 1, y].GetComponent<Candy>().id);

        }
        if (x < col - 1)
        {
            /*  possibleCandies.Remove(candies[x + 1, y]); */
            possibleCandies.RemoveAll(candy => candy.GetComponent<Candy>().id == candies[x + 1, y].GetComponent<Candy>().id);

        }
        if (y > 0)
        {
            /*  possibleCandies.Remove(candies[x, y - 1]); */
            possibleCandies.RemoveAll(candy => candy.GetComponent<Candy>().id == candies[x, y - 1].GetComponent<Candy>().id);

        }


        return possibleCandies[Random.Range(0, possibleCandies.Count)];

    }
}