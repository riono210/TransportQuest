using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChekerDistance : MonoBehaviour {

    [SerializeField] private Transform needleMark; // 針側の判定オブジェクト
    [SerializeField] private Transform chekerMark; // 土台側の判定オブジェクト

    [SerializeField] private RotationIncrease speedMater; // スピード操作のため

    [SerializeField] private GameObject[] evaluatoinObj; // 評価のTMP配列
    [SerializeField] private Transform evalParent; // TMPの親

    [SerializeField] private Material[] chekerMats; // エンターキーを押した時のマテリアル変化
    [SerializeField] private GameObject chekerObj; // チェッカーオブジェクト
    private Renderer chekerMat;

    // Start is called before the first frame update
    void Start () {
        chekerMat = chekerObj.GetComponent<Renderer> ();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            DistanceComparison ();
            StartCoroutine (PushButtonColorChange ());
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
        float goodThresholdValue = 1f;　 // good判定の距離の差
        float perfectThresholdValue = 0.4f; // perfect判定の距離の差

        if (distance <= perfectThresholdValue) {
            Debug.Log ("PREFECT!!!");
            GenerateEvalText (0);
            // 増やす

            // スピードの調整
            speedMater.SpeedAdjust (0.8f);
        } else if (distance <= goodThresholdValue) {
            Debug.Log ("good!");
            GenerateEvalText (1);
            speedMater.SpeedAdjust (0.3f);
        } else {
            Debug.Log ("bad...");
            GenerateEvalText (2);
            // スピードを一段階落とす
            speedMater.SpeedAdjust (-1f);
        }
    }

    // 評価TMPを生成する
    private void GenerateEvalText (int index) {
        // インスタンス生成
        GameObject instance = Instantiate (evaluatoinObj[index], evalParent.position, Quaternion.identity, evalParent);
        StartCoroutine (EvalAnimation (instance));
    }

    // 評価TMPのアニメーション
    private IEnumerator EvalAnimation (GameObject evalObj) {
        RectTransform objectRect = evalObj.GetComponent<RectTransform> ();
        Vector2 objectPos = objectRect.position;
        // 上昇させる
        for (int i = 0; i < 8; i++) {
            objectPos.y += 3f;
            objectRect.position = objectPos;
            yield return new WaitForSeconds (0.05f);
        }

        yield return new WaitForSeconds (1f);

        Destroy (evalObj);
    }

    // ボタンを押した時のchekerの色変化
    private IEnumerator PushButtonColorChange () {
        // 色変化
        chekerMat.sharedMaterial = chekerMats[1];
        Debug.Log ("color change");
        yield return new WaitForSeconds (0.3f);

        // 元の色に戻す
        chekerMat.sharedMaterial = chekerMats[0];
    }
}