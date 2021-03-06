﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreControler : MonoBehaviour {

    [SerializeField] private CannonSelector cannonSelector; // 速度調整のため
    [SerializeField] private StageGenerator stageGenerator; // 速度調整のため
    [SerializeField] private CharactorControler charactor; // アニメーションの速度
    [SerializeField] private float stageSpeed; // ステージの速度
    private float prevSpeed; // 変更前の速度

    private bool isStop; // 終了判定
    private int scorePoint; // お散歩した距離
    [SerializeField] private TextMeshProUGUI scoreText; // スコアのテキスト
    [SerializeField] private GameObject speedUpObj; // スピードアップオブジェクト
    private TextMeshProUGUI speedUpText; // スピードアップの表示テキスト

    // Start is called before the first frame update
    void Start () {
        isStop = false;
        scorePoint = 0;
        speedUpText = speedUpObj.GetComponent<TextMeshProUGUI> ();

        // スピードの初期値を設定
        stageSpeed = 1f;
        AllSpeedSet (); // スピードセット
        StartCoroutine (AddScore ()); // スコア加点
        StartCoroutine (SpeedUp ()); // スピードアップ
        //Debug.Log ("teststart");
    }

    // スピードの変更があったことを通知
    private void AllSpeedSet () {
        // 大砲側の速度設定
        cannonSelector.SetSpeed (stageSpeed);
        cannonSelector.SetAllBulletSpeed ();
        // ステージ側の速度設定
        stageGenerator.SetSpeed (stageSpeed);
        stageGenerator.ChangeStageSpeed ();
        prevSpeed = stageSpeed;

        // アニメーションの速度
        if (stageSpeed != 0) {
            charactor.AnimationSpeedChange (stageSpeed);
        }
    }

    // スコアを足す
    private IEnumerator AddScore () {
        while (!isStop) {
            scorePoint += 1;
            scoreText.text = $"{scorePoint} m";

            //Debug.Log ("test: " + stageSpeed);
            yield return new WaitForSeconds (1 / stageSpeed);
        }
    }

    private IEnumerator SpeedUp () {
        while (!isStop) {
            yield return new WaitForSeconds (30f);

            stageSpeed += 0.5f;
            AllSpeedSet ();

            if (stageSpeed == 4f) {
                Debug.Log ("Max Speed");
                speedUpText.text = "Max Speed!";
                StartCoroutine (FlashText (true));
                yield break;
            }
            StartCoroutine (FlashText (false));
        }
    }

    // テキストを点滅させる
    private IEnumerator FlashText (bool isLast) {
        // ピッ
        speedUpObj.SetActive (true);
        yield return new WaitForSeconds (0.3f);

        speedUpObj.SetActive (false);
        yield return new WaitForSeconds (0.1f);

        // ピッ
        speedUpObj.SetActive (true);
        yield return new WaitForSeconds (0.3f);

        speedUpObj.SetActive (false);
        yield return new WaitForSeconds (0.1f);

        // ピッ
        speedUpObj.SetActive (true);
        yield return new WaitForSeconds (1f);

        if (isLast) {
            speedUpObj.SetActive (true);
        } else {
            speedUpObj.SetActive (false);
        }
    }

    // スピード変更
    public void SetStageSpeed (float speed) {
        this.stageSpeed = speed;
        AllSpeedSet ();
    }

    // スコアを渡す
    public int GetScore () {
        return scorePoint;
    }

    public void SetIsStop (bool flag) {
        this.isStop = flag;
    }
}