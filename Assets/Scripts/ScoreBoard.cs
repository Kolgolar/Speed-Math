using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

public class ScoreBoard : MonoBehaviour
{
    const string PUBLISH_KEY = "40e0c02aacb5bb42a67ebb941fe73f04ce99f219";
    const int IDENT = 50;
    const int CAPACITY = 10;
    const string GET_URL = "https://kosterr.pythonanywhere.com/get_scores";
    const string POST_URL = "https://kosterr.pythonanywhere.com/publish_score";
    const int START_HEIGHT = 238;
    string nickname = "";
    int highscore = 0;

    [SerializeField]
    private GameObject rowsContainer;

    [SerializeField]
    private GameObject mainCircuit;
    
    [SerializeField]
    private GameObject row;
    
    [SerializeField]
    private GameObject inputField;

    
    public void SetValues(int score, string nick)
    {
        highscore = score;
        nickname = nick;
        inputField.GetComponent<TMP_InputField>().text = nickname;
        Debug.Log(nickname);
    }
    
    public void SyncWithServer()
    {
        StartCoroutine(getRequest(GET_URL));
        Debug.Log("Trying to get scores");
    }

    public void PublishScore()
    {
        nickname = inputField.GetComponent<TMP_InputField>().text;
        if (nickname == "")
            nickname = "Player " + Random.RandomRange(1, 99999).ToString();
        // highscore = mainCircuit.GetComponent<MainCircuit>().highscore;
        var publishData = new Dictionary<string, string>() {
            {"publish_key", PUBLISH_KEY},
            {"nickname", nickname},
            {"score", highscore.ToString()},
            {"game_ver", "0.0.1"},
        };
        string covertedData = JsonConvert.SerializeObject(publishData);
        StartCoroutine(postRequest(POST_URL, covertedData));
        // Debug.Log(JsonConvert.SerializeObject(publishData));
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    IEnumerator getRequest(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            string data = uwr.downloadHandler.text;
            string jsonString = fixJson(data);
            ScoreData[] recievedScores = JsonHelper.FromJson<ScoreData>(jsonString);
            FillTable(recievedScores);
        }
    }

     IEnumerator postRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            mainCircuit.GetComponent<MainCircuit>().nickname = nickname;
            mainCircuit.GetComponent<MainCircuit>().publishedHighscore = highscore;
            SaveSystem.SaveData(mainCircuit.GetComponent<MainCircuit>());
            SyncWithServer();
            // Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    void FillTable(ScoreData[] recievedScores)
    {
        int currIdent = START_HEIGHT;
        ClearTable();
        for (int i = 0; i < recievedScores.Length; i++)
        {
            GameObject newRow = Instantiate<GameObject>(row, rowsContainer.transform.position, Quaternion.EulerAngles(0, 0, 0), rowsContainer.transform);
            newRow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("{0}) {1}", i + 1, recievedScores[i].nickname);
            newRow.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = recievedScores[i].score;
            newRow.transform.localEulerAngles = new Vector3(0, 0, 0);
            newRow.transform.localPosition = new Vector3(0, currIdent, 0);
            currIdent -= IDENT;
        }
    }

    void ClearTable()
    {
        for (int i = 0; i < rowsContainer.transform.childCount; i++)
        {
            Destroy(rowsContainer.transform.GetChild(i).gameObject);
        }
    }

}

[System.Serializable]
class ScoreData
{
    public string nickname = "";
    public string score = "";
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}