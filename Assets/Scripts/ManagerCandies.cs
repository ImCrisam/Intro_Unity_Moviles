using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCandies : MonoBehaviour
{
    public static ManagerCandies instance;
    public List<Sprite> listPreFabs = new List<Sprite>();
    public GameObject currentCandy;
    public int col, row;

    private GameObject[,] candies;
    public bool isShifting { get; set; }
    // Start is called before the first frame update
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


    // Update is called once per frame
    void Update()
    {

    }
}
