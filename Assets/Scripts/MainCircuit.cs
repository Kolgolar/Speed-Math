using UnityEngine;
using TMPro;
using System;

public class MainCircuit : MonoBehaviour
{
    public GameObject ExampleText;
    public static Action<int, int> onStatsChanged;
    public static Action<bool> onAnswerRecieved;
    public static Action<int, int> onGameOver;
    public int highscore = 0;    
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
    private void OnEnable() 
    {
        MCButton.onClicked += AnswerRecieved;
        PCRestartButton.onClicked += Restart;
    }

    private void OnDisable() 
    {
        MCButton.onClicked -= AnswerRecieved;
        PCRestartButton.onClicked -= Restart;
    }
    
    void Start()
    {
        GameData data = SaveSystem.LoadData();
        if (data is null == false)
        {
            highscore = data.highscore;
        }
        ShowExample();
        onStatsChanged?.Invoke(Score, Lives);
    }


    void Update()
    {
        
    }

    private void Restart()
    {
        isGameOver = false;
        Lives = 3;
        Score = 0;
        ShowExample();
        onStatsChanged?.Invoke(Score, Lives);
    }


    private void AnswerRecieved(bool choice)
    {
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
                ShowExample();
        }
    }

    private void GameOver()
    {
        if (Score > highscore)
            highscore = Score;
        onGameOver?.Invoke(Score, highscore);
        ExampleText.GetComponent<TextMeshProUGUI>().text = "Game Over!";
        
        isGameOver = true;
        SaveSystem.SaveData(this);
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
    }

    private ExampleData CreateExample()
    {
        ExampleData data;
        var type = UnityEngine.Random.Range(0, 4);
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
