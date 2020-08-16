using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChekerDistance : MonoBehaviour {

    [SerializeField] private Transform needleMark;
    [SerializeField] private Transform chekerMark;

    [SerializeField] private RotationIncrease speedMater;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            DistanceComparison ();
        }
    }

    // 二つのオブジェクトの距離を比較
    public void DistanceComparison () {
        float distance = Vector3.Distance (chekerMark.position, needleMark.position);
        //Debug.Log ("distance: " + distance);
        Judgment (distance);
    }

    // 距離から判定を出す
    private void Judgment (float distance) {
        float goodThresholdValue = 1f;
        float excellentThresholdValue = 0.4f;

        if (distance <= excellentThresholdValue) {
            Debug.Log ("EXCELLENT!!!");
            // 増やす

            // スピードの調整
            speedMater.SpeedAdjust (0.8f);
        } else if (distance <= goodThresholdValue) {
            Debug.Log ("good!");
            speedMater.SpeedAdjust (0.3f);
        } else {
            Debug.Log ("bad...");
            // スピードを一段階落とす
            speedMater.SpeedAdjust (-1f);
        }
    }
}