﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGIU : MonoBehaviour
{
    public static ManagerGIU instance;

    public Canvas inGame;
    public Canvas play;
    public Canvas inGameOver;
    public Slider sliderMath;
    public AudioSource music;
    public AudioSource toMatch;
    [SerializeField] private Text movesText;
    private int moves;


    public bool inPlay = false;
    public int Moves
    {
        get { return moves; }
        set
        {

            moves = value;
            if (moves >= 0)
            {
                movesText.text = moves + "";

            }
            else
            {
                StartCoroutine(GameOver());
            }

        }
    }

    private IEnumerator GameOver()
    {
        inPlay = false;
        yield return new WaitUntil(() => !ManagerCandies.instance.isShifting);
        yield return new WaitForSeconds(2.1f);
        inGameOver.GetComponentInChildren<Text>().text = "Score: " + score;
        inGameOver.enabled = true;
        inGame.enabled = false;
        play.enabled = false;

    }

    [SerializeField] private Text ScoreText;
    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreText.text = "" + score;
        }
    }

    void Start()
    {
        initCanvas();
        movesText.text = "" + moves;

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
        inGame.enabled = false;
        inGameOver.enabled = false;
        play.enabled = true;
        inPlay = false;
        Moves = 9;
        Score = 0;

    }
    public void easy()
    {
        newGame(3);
    }
    public void normal()
    {
        newGame(4);

    }
    public void hard()
    {
        newGame(5);

    }

    private void newGame(int minMach)
    {
        ManagerCandies.minToMach = minMach;
        reset();
    }

    public void reset()
    {
        ManagerCandies.instance.newGame();
        inGameOver.enabled = false;
        inGame.enabled = true;
        play.enabled = false;
        inPlay = true;
        Moves = 9;
        Score = 0;
    }

    public void backGame()
    {
        ManagerCandies.instance.destroyGame();
        inGameOver.enabled = false;
        inGame.enabled = false;
        play.enabled = true;
        inPlay = false;
        Moves = 9;
        Score = 0;
    }

    public void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void buttonMusica()
    {
        if (music.volume > 0.3f)
        {
            music.volume = 0.3f;
            toMatch.volume = 0.4f;
        }
        else if (music.volume <= 0.1f)
        {
            music.volume = 0.8f;
            toMatch.volume = 0.9f;
        }
        else
        {
            music.volume = 0;
            toMatch.volume = 0.1f;
        }
    }

}