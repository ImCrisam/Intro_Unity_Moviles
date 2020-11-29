using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGIU : MonoBehaviour
{
    public static ManagerGIU instance;

    public Canvas inplay;
    public Canvas play;
    public Canvas inGameOver;
    [SerializeField] private Text movesText;
    private int moves;
    private int movesMax;
    public int MovesMax { get { return movesMax; } set { movesMax = value > 10 ? 9 : value; } }

    public int Moves
    {
        get { return moves; }
        set
        {
            moves = value;
            if (moves >= 0)
            {
                movesText.text = "moves" + moves + "/" + movesMax;

            }
            else
            {
                StartCoroutine(GameOver());
            }

        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitUntil(() => !ManagerCandies.instance.isShifting);
        yield return new WaitForSeconds(0.5f);
    }

    [SerializeField] private Text ScoreText;
    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreText.text = "Score:" + score;
        }
    }



    void Start()
    {
        initCanvas();
        movesText.text = "Moves: " + moves + "/" + movesMax;
        if (movesMax == 0)
        {
            movesMax = 9;

        }
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void initCanvas()
    {
        inplay.enabled = true;
        inGameOver.enabled = false;
        play.enabled = false;

    }


}
