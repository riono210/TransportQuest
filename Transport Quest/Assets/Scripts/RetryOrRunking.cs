using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryOrRunking : MonoBehaviour {

    [SerializeField] private ScoreControler scoreCtl;

    // ランキングを出す
    public void ShowRunking () {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (scoreCtl.GetScore ());
    }

    // リスタート
    public void RestartGame () {
        SceneManager.LoadScene (0);
    }
}