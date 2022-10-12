using UnityEngine;
using TMPro;
using System;

public class PlayCircuit : MonoBehaviour
{
    public GameObject ExampleText;
    public static Action<int, int> onStatsChanged;
    public static Action<bool> onAnswerRecieved;
    public static Action onGameOver;
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
        PC_Button.onClicked += AnswerRecieved;
        pcRestartButton.onClicked += Restart;
    }

    private void OnDisable() 
    {
        PC_Button.onClicked -= AnswerRecieved;
        pcRestartButton.onClicked -= Restart;
    }
    
    void Start()
    {
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
                //RightAnswQ++;
                Score += SuccessPoints;
                //RightAnswText.GetComponent<TextMeshProUGUI>().text = RightAnswQ.ToString();
                Debug.Log("Верно!");
            }
            else
            {
                //WrongAnswQ++;
                Score -= ForfeitPoints;
                if (Score < 0)
                    Score = 0;
                Lives -= 1;
                //WrongAnswText.GetComponent<TextMeshProUGUI>().text = WrongAnswQ.ToString();
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
        onGameOver?.Invoke();
        ExampleText.GetComponent<TextMeshProUGUI>().text = "Game Over!";
        isGameOver = true;
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
        var str = string.Format("{0} {1} {2} = {3}", Data.first, Data.sign, Data.second, AnswToShow);
        ExampleText.GetComponent<TextMeshProUGUI>().text = str;
    }

    private ExampleData CreateExample()
    {
        ExampleData data;
        var type = UnityEngine.Random.Range(0, 2);
        switch(type)
        {
            case 0:
                data = CreateSum();
                break;
            case 1:
                data = CreateDifference();
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

    private ExampleData CreateDifference()
    {
        var max = 100;
        var s1 = UnityEngine.Random.Range(0, max + 1);
        var s2 = UnityEngine.Random.Range(0, max + 1);
        var rightAnsw = s1 - s2;
        var fakeAnsw = GenRandWithException(-max, max, rightAnsw);
        var Data = new ExampleData(s1, s2, "-", rightAnsw, fakeAnsw);
        return Data;
    }
}
