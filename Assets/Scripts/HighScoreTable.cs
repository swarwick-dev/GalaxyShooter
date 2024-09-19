using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    private float _score_offset = 20f;
    private List<HighScoreEntry> highScoreEntryList;
    private List<Transform> highScoreEntryTransformList;
    
    private void Awake() {
        entryTemplate.gameObject.SetActive(false);
        highScoreEntryTransformList = new List<Transform>();
        ReloadHighScores();
    }

    private void ReloadHighScores() {
        HighScores highScores;
        string jsonString = PlayerPrefs.GetString("highscoretable");

        if ( jsonString.Length == 0 ){
            highScores = new HighScores();
            highScores.highScoreEntryList = new List<HighScoreEntry>
            {
                new HighScoreEntry { score = 99999, name = "SJW" }
            };
        }
        else 
            highScores = JsonUtility.FromJson<HighScores>(jsonString);
        
        highScoreEntryList = highScores.highScoreEntryList;

        highScoreEntryList.Sort((x,y) => x.score.CompareTo(y.score));
        highScoreEntryList.Reverse();

        if ( highScoreEntryList.Count > 10 )
            highScoreEntryList.RemoveRange(9, highScoreEntryList.Count - 9);

        foreach(Transform t in highScoreEntryTransformList)
            Destroy(t.gameObject);
        
        highScoreEntryTransformList.Clear();

        foreach ( HighScoreEntry score in highScoreEntryList) 
            CreateHighScoreEntry(score, entryContainer, highScoreEntryTransformList);
    }

    private void CreateHighScoreEntry(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList) {
        string rankString = "";

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0,-_score_offset * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count+1;
        switch(rank) {
            default: rankString = rank + "TH"; break;
            case 1 : rankString = rank + "ST"; break;
            case 2 : rankString = rank + "ND"; break;
            case 3 : rankString = rank + "RD"; break;
        }
        entryTransform.Find("Position").GetComponent<Text>().text = rankString;
        entryTransform.Find("Score").GetComponent<Text>().text = highScoreEntry.score.ToString();
        entryTransform.Find("Name").GetComponent<Text>().text = highScoreEntry.name;

        transformList.Add(entryTransform);
    }

    public bool IsNewHighScore(long score) {
        if ( highScoreEntryList.Count < 10 )
            return true;
        else {
            for ( int i = highScoreEntryList.Count; i > 0; i-- ) {
                if ( highScoreEntryList[i-1].score <= score ){
                    return true;
                }
            }
        }

        return false;
    }
    public void AddHighScoreEntry(long score, string name) {
        HighScoreEntry newScore = new HighScoreEntry{ score = score, name = name };
        string jsonString = PlayerPrefs.GetString("highscoretable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);
        
        if ( highScores == null ) {
            highScores = new HighScores();
            highScores.highScoreEntryList = new List<HighScoreEntry>();
        }

        highScores.highScoreEntryList.Add(newScore);
        jsonString = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highscoretable", jsonString);
        PlayerPrefs.Save();

        ReloadHighScores();

    }

    private class HighScores { public List<HighScoreEntry> highScoreEntryList; }
    [System.Serializable] public class HighScoreEntry {
        public string name;
        public long score;
    }
}
