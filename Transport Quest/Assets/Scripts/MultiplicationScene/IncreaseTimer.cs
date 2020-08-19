using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseTimer : MonoBehaviour {

    [SerializeField] private Slider timer;
    [SerializeField] private GameObject hider;
    private float nowTime; // 残り時間

    public bool isTimeUp; // 終了判定

    // Start is called before the first frame update
    void Start () {
        isTimeUp = false;
        nowTime = 30;
    }

    void Update () {
        CountTimer ();
    }

    // 30秒カウントしてスライダーの表示を変える
    public void CountTimer () {
        if (nowTime > 0) {
            nowTime -= Time.deltaTime;
            float timerVal = Mathf.InverseLerp (0, 30, nowTime);
            timer.value = timerVal;
        } else { // カウント終了後
            nowTime = 0;
            hider.SetActive (true);
            isTimeUp = true;
        }
    }
}