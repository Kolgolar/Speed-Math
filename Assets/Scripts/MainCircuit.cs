using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class MainCircuit : MonoBehaviour
{
    public GameObject StatsDisplayer;
    public GameObject AudioPlayer;
    private MainCircuitSounds sounds;
    public float nextQuestionDelay = 1f;
    public GameObject truenessIndicator;
    public GameObject ExampleText;
    public GameObject scoreBoard;
    public GameObject loadingDisplay;
    private float timeToAnswer;
    public float maxTimeToAnswer = 7f;
    public float minTimeToAnswer = 2f;
    public float timeToAnswerDelta = 0.2f;
    public static Action<int, int> onStatsChanged;
    public static Action<bool> onAnswerRecieved;
    public static Action<int, int> onGameOver;
    public string nickname = "";
    public int highscore = 0;
    private int addLifeCost = 5;
    private int rightCombo = 0;
    private string loadingString = ".........................................";
    private int loadingStringLength;
    private bool isQuestionShowed = false;
    public int publishedHighscore = 0;    
    private float timeToAnswerLeft;
    private int Score = 0;
    private int maxLives = 3;
    private int Lives = 0;
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
        ShowDefaultScreen();
        sounds = AudioPlayer.GetComponent<MainCircuitSounds>();
        SetStartStats();
        truenessIndicator.GetComponent<TruenessIndicator>().fullBrightnessTime = nextQuestionDelay * 0.8f;
        GameData data = SaveSystem.LoadData();
        if (data is null == false)
        {
            highscore = data.highscore;
            nickname = data.nickname;
            scoreBoard.GetComponent<ScoreBoard>().SetValues(highscore, nickname);
        }
        // ShowExample();
    }


    void Update()
    {
        // print(isQuestionShowed);
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
        // print(visibleCharacters);
        string strToShow = loadingString.Substring(loadingStringLength - visibleCharacters);
        loadingDisplay.GetComponent<TextMeshProUGUI>().text = strToShow;
    }

    private void SetStartStats()
    {
        Lives = maxLives;
        Score = 0;
        loadingStringLength = loadingString.Length;
        MCButton.canBePressed = true;
        onStatsChanged?.Invoke(Score, Lives);
        isGameOver = false;
        timeToAnswer = maxTimeToAnswer + timeToAnswerDelta;
    }

    private void Restart()
    {
        ShowDefaultScreen();
        StartCoroutine(Timer(1f, () => 
                    {
                        SetStartStats();
                        ShowExample();
                    }));
    }

    private void ShowDefaultScreen()
    {
        ExampleText.GetComponent<TextMeshProUGUI>().text = "Rebooting...";
        StatsDisplayer.GetComponent<StatsDisplayer>().Clear();
        loadingDisplay.GetComponent<TextMeshProUGUI>().text = "";
        timeToAnswerLeft = 0;
        isGameOver = true;
    }

    private void AnswerRecieved(bool choice)
    {
        bool addLife = false;
        isQuestionShowed = false;
        timeToAnswerLeft = 0;
        UpdateTimeIndicator();
        MCButton.canBePressed = false;
        if (!isGameOver)
        {
            if (choice == IsRightExampleShowed)
            {
                Score += SuccessPoints;
                if (Lives < maxLives)
                    rightCombo += 1;
                    if (rightCombo == addLifeCost)
                    {
                        sounds.ExtraLive();
                        Lives += 1;
                        addLife = true;
                        rightCombo = 0;
                    }
                // Debug.Log("Верно!");
            }
            else
            {
                //WrongAnswQ++;
                rightCombo = 0;
                Score -= ForfeitPoints;
                if (Score < 0)
                    Score = 0;
                Lives -= 1;
                // Debug.Log("Неверно!");
            }
            onAnswerRecieved?.Invoke(choice == IsRightExampleShowed);
            onStatsChanged?.Invoke(Score, Lives);
            if (Lives == 0)
                GameOver();
            else
                {
                    // ShowLenny(choice == IsRightExampleShowed);
                    ShowResult(choice == IsRightExampleShowed, addLife);
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


    void ShowResult(bool isRight, bool addLife)
    {
        string textToShow = string.Format("+{0} pts!", SuccessPoints);
        if (!isRight)
            textToShow = string.Format("-{0} pts.", ForfeitPoints);
        else if (addLife)
            textToShow += "\n +1 life!";
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
        if (!isGameOver)
        {
            sounds.GameOver();
            isQuestionShowed = false;
            if (Score > highscore)
                highscore = Score;
            onGameOver?.Invoke(Score, highscore);
            ExampleText.GetComponent<TextMeshProUGUI>().text = "Game Over!";
            
            isGameOver = true;
            SaveSystem.SaveData(this);
            scoreBoard.GetComponent<ScoreBoard>().SetValues(highscore, nickname);
        }
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
        var rnd1 = Random.Range(min, exception);
        var rnd2 = Random.Range(exception + 1, max);
        int rnd = rnd1;
        if (Random.Range(0, 2) > 0)
            rnd = rnd2;
        return rnd;
    }   

    private ExampleData CreateSum()
    {
        var max = 100;
        var s1 = Random.Range(0, max + 1);
        var s2 = Random.Range(0, max + 1);
        var rightAnsw = s1 + s2;
        var fakeAnsw = GenRandWithException(0, max * 2, rightAnsw);
        var Data = new ExampleData(s1, s2, "+", rightAnsw, fakeAnsw);
        return Data;
    }

    private ExampleData CreateDiff()
    {
        var max = 100;
        var s1 = Random.Range(-max, max + 1);
        var s2 = Random.Range(0, max + 1);
        var rightAnsw = s1 - s2;
        var fakeAnsw = GenRandWithException(-max, max, rightAnsw);
        var Data = new ExampleData(s1, s2, "-", rightAnsw, fakeAnsw);
        return Data;
    }

    private ExampleData CreateMult()
    {
        var max = 30;
        var s1 = Random.Range(0, max + 1);
        var s2 = Random.Range(0, max + 1);
        var rightAnsw = s1 * s2;
        var fakeAnsw = GenRandWithException(0, max * max, rightAnsw);
        var Data = new ExampleData(s1, s2, "*", rightAnsw, fakeAnsw);
        return Data;
    }

    private ExampleData CreateDiv()
    {
        var max = 30;
        var s2 = Random.Range(1, max + 1);
        var rightAnsw = Random.Range(1, max + 1);
        var s1 = s2 * rightAnsw;
        var fakeAnsw = GenRandWithException(0, max * max, rightAnsw);
        var Data = new ExampleData(s1, s2, "/", rightAnsw, fakeAnsw);
        return Data;
    }
}
