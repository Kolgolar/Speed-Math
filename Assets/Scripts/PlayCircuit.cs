using UnityEngine;
using TMPro;

public class PlayCircuit : MonoBehaviour
{
    public GameObject ExampleText;
    public GameObject RightAnswText;
    public GameObject WrongAnswText;
    
    private int RightAnswQ, WrongAnswQ = 0;
    private bool IsRightAnswerShowed;
    private struct ExampleData
    {
        public int first;
        public int second;
        public int answ;
        public ExampleData(int f, int s, int a)
        {
            this.first = f;
            this.second = s;
            this.answ = a;
            
        }
    }
    
    void Start()
    {
        ShowExample();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPressed(string ButtonName)
    {
        var TargetButtonName = "FalseButton";
        if (IsRightAnswerShowed)
            TargetButtonName = "TrueButton";
        
        if (ButtonName == TargetButtonName)
        {
            RightAnswQ++;
            RightAnswText.GetComponent<TextMeshProUGUI>().text = RightAnswQ.ToString();
            Debug.Log("Верно!");
        }
        else
        {
            WrongAnswQ++;
            WrongAnswText.GetComponent<TextMeshProUGUI>().text = WrongAnswQ.ToString();
            Debug.Log("Неверно!");
        }
        ShowExample();
    }

    private void ShowExample()
    {
        var Data = CreateExample();
        var AnswToShow = Data.answ;
        if (Random.Range(0, 2) > 0)
        {
            AnswToShow = Random.Range(0, 100);
            IsRightAnswerShowed = false;
        }
        else
            IsRightAnswerShowed = true;
        var str = string.Format("{0} + {1} = {2}", Data.first, Data.second, AnswToShow);
         ExampleText.GetComponent<TextMeshProUGUI>().text = str;
    }

    private ExampleData CreateExample()
    {
        var Data = CreateSum();
        return Data;
    }

    private ExampleData CreateSum()
    {
        var s1 = Random.Range(0, 100);
        var s2 = Random.Range(0, 100);
        var answ = s1 + s2;
        var Data = new ExampleData(s1, s2, answ);
        return Data;
    }
}
