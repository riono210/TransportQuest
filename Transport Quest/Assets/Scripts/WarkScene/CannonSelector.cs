﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSelector : MonoBehaviour {

    [SerializeField] private List<Transform> selectList; // レーンのリスト
    [SerializeField] private Transform cannonTranseform; // 大砲オブジェクト
    [SerializeField] private GameObject[] bulletPrefabs;
    private int selectNumber;
    private Vector3 updatePos; // Updateで更新される大砲のポジション

    private Transform nowSelectRane; // 現在選択しているTransform
    private int prevSelectInstanceID; // 変更前のTransformのindex
    private float currentVelocity; // 移動時の加速度
    private bool isChangeRane; // レーンを変更したかどうか
    private bool isShot; // 発射したかどうか

    private float stageSpeed; // 選択するスピード
    private float shotSpeed; // 発射速度

    // Start is called before the first frame update
    void Start () {
        selectNumber = 0;
        updatePos = cannonTranseform.localPosition;

        currentVelocity = 0;
        isChangeRane = false;
        isShot = false;
    }

    // Update is called once per frame
    void Update () {
        LaneSelect ();
        CannonMove ();
        if (Input.GetKey (KeyCode.Space)) {
            StartCoroutine (ShotBullet ());
        }
    }

    // 範囲内に入ったらリストに追加する
    private void OnTriggerEnter (Collider other) {
        string layer = LayerMask.LayerToName (other.gameObject.layer);
        if (layer == "Stage") {
            //Debug.Log ("name:" + other.gameObject.name);
            selectList.Add (other.gameObject.transform);
        }
    }

    // 範囲外に出たらリストから削除する・Destroyする前にリストから削除すること
    private void OnTriggerExit (Collider other) {
        // 同じInstanceIDだった場合削除する
        int otherID = other.gameObject.GetInstanceID ();
        selectList.RemoveAll (trans => trans.gameObject.GetInstanceID () == otherID);
    }

    // 大砲の移動
    private void CannonMove () {
        nowSelectRane = selectList[selectNumber];
        if (prevSelectInstanceID != nowSelectRane.gameObject.GetInstanceID () || isChangeRane) { // 選択しているレーンが変更されている場合
            //Debug.Log ("change");
            // 変数持たせて実行　途中でselectNumが変更されたらそのコルーチンを破棄させる
            isChangeRane = true;
            SmoothMove ();
        } else if (selectList.Count > 0 && !isChangeRane) { // selectNumが変わらなかった場合の動作
            // Debug.Log ("not change");
            // 親と子供のx座標を足したものが大砲のx座標系と同じになる
            float xAxis = nowSelectRane.localPosition.x + nowSelectRane.parent.transform.localPosition.x;
            updatePos.x = xAxis;
        }
        cannonTranseform.localPosition = updatePos;
        prevSelectInstanceID = nowSelectRane.gameObject.GetInstanceID ();
    }

    // スライド移動　終了後にselectNumが変化しない方の移動方式に変える　変更スピードも可変できるといい
    private void SmoothMove () {
        updatePos.x = Mathf.SmoothDamp (updatePos.x, nowSelectRane.position.x, ref currentVelocity, 0.3f);

        if (Mathf.Abs (updatePos.x - nowSelectRane.position.x) <= 0.3f) { // スムーズ移動を止める判定(距離)
            isChangeRane = false;
        }
    }

    // レーン番号の変更操作
    private void LaneSelect () {
        if (Input.GetKeyDown (KeyCode.LeftArrow)) { // 左矢印
            selectNumber += 1;
        } else if (Input.GetKeyDown (KeyCode.RightArrow)) { // 右矢印
            selectNumber -= 1;
        }

        if (selectNumber < 0) {
            selectNumber = 0;
        } else if (selectNumber >= selectList.Count) {
            selectNumber = selectList.Count - 1;
        }
    }

    // 弾丸を打つ
    private IEnumerator ShotBullet () {
        if (!isShot && !isChangeRane) { // 移動中は打てない
            isShot = true;
            GameObject newBullet = Instantiate (bulletPrefabs[0], nowSelectRane.position, Quaternion.identity, nowSelectRane.parent);
            BulletMove bullet = newBullet.GetComponent<BulletMove> ();
            bullet.SetSpeed (stageSpeed);

            shotSpeed = stageSpeed;
            if (stageSpeed == 1) {  // 1だと遅すぎるため
                shotSpeed = 2;
            }
            yield return new WaitForSeconds (0.8f / (shotSpeed * 0.8f)); // ステージ速度によって連射速度を早くする
            isShot = false;
        }
    }

    public void SetSpeed (float speed) {
        this.stageSpeed = speed;
    }
}