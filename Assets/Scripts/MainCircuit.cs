using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MainCircuit : MonoBehaviour
{
    public float nextQuestionDelay = 1f;
    public GameObject truenessIndicator;
    public GameObject ExampleText;
    public GameObject scoreBoard;
    public GameObject loadingDisplay;
    public float timeToAnswer = 7f;
    public float minTimeToAnswer = 3f;
    public float timeToAnswerDelta = 0.1f;
    public static Action<int, int> onStatsChanged;
    public static Action<bool> onAnswerRecieved;
    public static Action<int, int> onGameOver;
    public string nickname = "";
    public int highscore = 0;
    private string loadingString = ".........................................";
    private int loadingStringLength;
    private bool isQuestionShowed = false;
    public int publishedHighscore = 0;    
    private float timeToAnswerLeft;
    private int Score = 0;
    private int Lives = 3;
    private int ForfeitPoints = 50;
    private int SuccessPoints = 100;
    private bool isGameOver = false;
    private bool IsRightExampleShowed;
    private struct ExampleData
    {
        public int first;
        public int second;
        public string sign;
        public int rightAnsw;
        public int fakeAnsw;
        public ExampleData(int f, int s, string si, int ra, int fa)
        {
            this.first = f;
            this.second = s;
            this.sign = si;
            this.rightAnsw = ra;
            this.fakeAnsw = fa;
        }
    }

    private int gamemode = -1;
    public int Gamemode {
        set{gamemode = value; Debug.Log(gamemode); Restart();}
        get{return gamemode;}
    }


    private void OnEnable() 
    {
        MCButton.onClicked += AnswerRecieved;
        PCRestartButton.onClicked += Restart;
        StopGameButton.onClicked += GameOver;
    }

    private void OnDisable() 
    {
        MCButton.onClicked -= AnswerRecieved;
        PCRestartButton.onClicked -= Restart;
        StopGameButton.onClicked -= GameOver;
    }
    
    void Start()
    {
        timeToAnswer += timeToAnswerDelta;
        loadingStringLength = loadingString.Length;
        MCButton.canBePressed = true;
        truenessIndicator.GetComponent<TruenessIndicator>().fullBrightnessTime = nextQuestionDelay * 0.8f;
        GameData data = SaveSystem.LoadData();
        if (data is null == false)
        {
            highscore = data.highscore;
            nickname = data.nickname;
            scoreBoard.GetComponent<ScoreBoard>().SetValues(highscore, nickname);
        }
        // ShowExample();
        onStatsChanged?.Invoke(Score, Lives);
    }


    void Update()
    {
        print(isQuestionShowed);
        if (isQuestionShowed)
        {
            if (timeToAnswerLeft > 0)
            {
                UpdateTimeIndicator();
                timeToAnswerLeft -= Time.deltaTime;
            }
            else
            {
                if (!isGameOver)
                    AnswerRecieved(!IsRightExampleShowed);
                timeToAnswerLeft = 0;
            }
        }
    }

    void UpdateTimeIndicator()
    {
        float persentage = timeToAnswerLeft / timeToAnswer;
        int visibleCharacters = Convert.ToInt32(loadingStringLength * persentage);
        print(visibleCharacters);
        string strToShow = loadingString.Substring(loadingStringLength - visibleCharacters);
        loadingDisplay.GetComponent<TextMeshProUGUI>().text = strToShow;
    }

    private void Restart()
    {
        isGameOver = false;
        Lives = 3;
        Score = 0;
        ShowExample();
        onStatsChanged?.Invoke(Score, Lives);
        MCButton.canBePressed = true;
    }

    private void AnswerRecieved(bool choice)
    {
        isQuestionShowed = false;
        timeToAnswerLeft = 0;
        UpdateTimeIndicator();
        MCButton.canBePressed = false;
        if (!isGameOver)
        {
            if (choice == IsRightExampleShowed)
            {
                Score += SuccessPoints;
                Debug.Log("Верно!");
            }
            else
            {
                //WrongAnswQ++;
                Score -= ForfeitPoints;
                if (Score < 0)
                    Score = 0;
                Lives -= 1;
                Debug.Log("Неверно!");
            }
            onAnswerRecieved?.Invoke(choice == IsRightExampleShowed);
            onStatsChanged?.Invoke(Score, Lives);
            if (Lives == 0)
                GameOver();
            else
                {
                    // ShowLenny(choice == IsRightExampleShowed);
                    ShowResult(choice == IsRightExampleShowed);
                    StartCoroutine(Timer(nextQuestionDelay, () => 
                    {
                        ShowExample();
                    }));  
                }
        }
    }

    IEnumerator Timer(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }


    void ShowResult(bool isRight)
    {
        string textToShow = string.Format("+{0} pts!", SuccessPoints);
        if (!isRight)
        {
            textToShow = string.Format("-{0} pts.", ForfeitPoints);
        }
        ExampleText.GetComponent<TextMeshProUGUI>().text = textToShow;
    }
    
    /*void ShowLenny(bool isHappy)
    {
        string[] happyLennies = {"( ͡ᵔ ͜ʖ ͡ᵔ ", "( ͡~ ͜ʖ ͡°)", "(˵ ͡o ͜ʖ ͡o˵)", "( ͡° ͜ʖ ͡°)", "٩(^ᴗ^)۶"};
        string[] sadLenny = {"ಠ╭╮ಠ", "(ó﹏ò｡)", "¯\\_(ツ)_/¯", "( ͠° ͟ʖ ͡°)", "ཀ ʖ̯ ཀ"};
        string[] lennies;
        if (isHappy)
            lennies = happyLennies;
        else
            lennies = sadLenny;
        int idx = Random.Range(0, lennies.Length);
        ExampleText.GetComponent<TextMeshProUGUI>().text = lennies[idx];
    }
    */

    private void GameOver()
    {
        isQuestionShowed = false;
        if (Score > highscore)
            highscore = Score;
        onGameOver?.Invoke(Score, highscore);
        ExampleText.GetComponent<TextMeshProUGUI>().text = "Game Over!";
        
        isGameOver = true;
        SaveSystem.SaveData(this);
        scoreBoard.GetComponent<ScoreBoard>().SetValues(highscore, nickname);
    }


    private void ShowExample()
    {
        var Data = CreateExample();
        var AnswToShow = Data.rightAnsw;
        if (UnityEngine.Random.Range(0, 2) > 0)
        {
            AnswToShow = Data.fakeAnsw;
            IsRightExampleShowed = false;
        }
        else
            IsRightExampleShowed = true;
        var str = string.Format("{0}{1}{2}={3}", Data.first, Data.sign, Data.second, AnswToShow);
        ExampleText.GetComponent<TextMeshProUGUI>().text = str;
        MCButton.canBePressed = true;
        isQuestionShowed = true;
        if (timeToAnswer > minTimeToAnswer)
            timeToAnswer -= timeToAnswerDelta;
        timeToAnswerLeft = timeToAnswer;
    }

    private ExampleData CreateExample()
    {
        ExampleData data;
        int type;
        if (gamemode == -1)
            type = UnityEngine.Random.Range(0, 4);
        else
            type = gamemode;

        switch(type)
        {
            case 0:
                data = CreateSum();
                break;
            case 1:
                data = CreateDiff();
                break;
            case 2:
                data = CreateMult();
                break;
            case 3:
                data = CreateDiv();
                break;
            default:
                data = CreateSum();
                break;
        }
        return data;
    }

    private int GenRandWithException(int min, int max, int exception)
    {
        var rnd1 = UnityEngine.Random.Range(min, exception);
        var rnd2 = UnityEngine.Random.Range(exception + 1, max);
        int rnd = rnd1;
        if (UnityEngine.Random.Range(0, 2) > 0)
            rnd = rnd2;
        return rnd;
    }   

    private ExampleData CreateSum()
    {
        var max = 100;
        var s1 = UnityEngine.Random.Range(0, max + 1);
        var s2 = UnityEngine.Random.Range(0, max + 1);
        var rightAnsw = s1 + s2;
        var fakeAnsw = GenRandWithException(0, max * 2, rightAnsw);
        var Data = new ExampleData(s1, s2, "+", rightAnsw, fakeAnsw);
        return Data;
    }

    private ExampleData CreateDiff()
    {
        var max = 100;
        var s1 = UnityEngine.Random.Range(0, max + 1);
        var s2 = UnityEngine.Random.Range(0, max + 1);
        var rightAnsw = s1 - s2;
        var fakeAnsw = GenRandWithException(-max, max, rightAnsw);
        var Data = new ExampleData(s1, s2, "-", rightAnsw, fakeAnsw);
        return Data;
    }

    private ExampleData CreateMult()
    {
        var max = 20;
        var s1 = UnityEngine.Random.Range(0, max + 1);
        var s2 = UnityEngine.Random.Range(0, max + 1);
        var rightAnsw = s1 * s2;
        var fakeAnsw = GenRandWithException(-max * max, max * max, rightAnsw);
        var Data = new ExampleData(s1, s2, "*", rightAnsw, fakeAnsw);
        return Data;
    }

    private ExampleData CreateDiv()
    {
        var max = 20;
        var s2 = UnityEngine.Random.Range(0, max + 1);
        var rightAnsw = UnityEngine.Random.Range(0, max + 1);
        var s1 = s2 * rightAnsw;
        var fakeAnsw = GenRandWithException(-max * max, max * max, rightAnsw);
        var Data = new ExampleData(s1, s2, "/", rightAnsw, fakeAnsw);
        return Data;
    }
}
