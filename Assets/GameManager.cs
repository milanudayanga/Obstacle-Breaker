using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { set; get; }

    public bool IsDead { set; get; }
    private bool isGameStarted = false;
    private PlayerMotor motor;

    public Animator gamecanvas;
    public Text ScoreText, coinText,modifierText;
    private float score, coinScore, modifierScore;
    private int lastScore;

    //Death Menu 
    public Animator deathMenuanim;
    public Text DeathScoreText, DeathCoinScore;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        ScoreText.text = ScoreText.text = score.ToString("0");


    }

    private void Update() 
    {
     if (MobileInput.Instance.Tap  && !isGameStarted)
        {
            isGameStarted =true;
            motor.StartRunning();
            FindObjectOfType<CameraMotor>().Ismoving = true;
            gamecanvas.SetTrigger("Show");
        }
        if (isGameStarted && !IsDead)
        {
            //Bump the score up
            lastScore = (int)score;

            score += (Time.deltaTime * modifierScore);

            if (lastScore ==(int) score)
            {

                ScoreText.text = score.ToString("0");

            }

        }
    }

    public void GetCoin()
    {
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        ScoreText.text = ScoreText.text = score.ToString("0");
    }



    public void updateModifier(float modifieramount)
    {
        modifierScore = 1.0f + modifieramount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameF");

    }

    public void OnDeath()
    {
        IsDead = true;
        DeathScoreText.text = score.ToString("0");
        DeathCoinScore.text = coinScore.ToString("0");

        deathMenuanim.SetTrigger("Dead");

    }

}
