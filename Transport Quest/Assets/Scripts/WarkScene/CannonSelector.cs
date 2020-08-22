using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CannonSelector : MonoBehaviour {

    [SerializeField] private List<Transform> selectList; // レーンのリスト
    [SerializeField] private Transform cannonTranseform; // 大砲オブジェクト
    [SerializeField] private GameObject[] bulletPrefabs; // 弾のプレファブリスト
    [SerializeField] private Transform bulletParent;
    private int selectNumber;
    private Vector3 updatePos; // Updateで更新される大砲のポジション

    private Transform nowSelectRane; // 現在選択しているTransform
    private int prevSelectInstanceID; // 変更前のTransformのindex
    private float currentVelocity; // 移動時の加速度
    private bool isChangeRane; // レーンを変更したかどうか
    private bool isShot; // 発射したかどうか

    private float stageSpeed; // 選択するスピード
    private float shotSpeed; // 発射速度

    private int tokenNum; // もキュの数
    [SerializeField] private TextMeshProUGUI tokenNumText; // 表示テキスト

    [SerializeField] private AudioClip clip; //SE
    [SerializeField] private AudioSource audioSource;

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
        if (selectList.Count > 0) { // エラー回避リストが1以上の時移動可能
            CannonMove ();
        }
        if (Input.GetKey (KeyCode.Space)) {
            StartCoroutine (ShotBullet ());
        }
        ShowTokenNumber ();
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
        selectNumber -= 1;
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
            Transform colParent = nowSelectRane.parent.transform;
            // 親と子供のx座標を足したものが大砲のx座標系と同じになる  StabeChip + Rane + コライダー  
            float xAxis = nowSelectRane.localPosition.x + colParent.localPosition.x + colParent.transform.parent.localPosition.x;
            updatePos.x = xAxis;
        }
        cannonTranseform.localPosition = updatePos;
        prevSelectInstanceID = nowSelectRane.gameObject.GetInstanceID ();
    }

    // スライド移動　終了後にselectNumが変化しない方の移動方式に変える　変更スピードも可変できるといい
    private void SmoothMove () {
        //updatePos.x = Mathf.SmoothDamp (updatePos.x, nowSelectRane.position.x, ref currentVelocity, 0.3f);

        if (Mathf.Abs (updatePos.x - nowSelectRane.position.x) <= 0.1f) { // スムーズ移動を止める判定(距離)
            isChangeRane = false;
            return;
        }

        updatePos.x = Mathf.MoveTowards (updatePos.x, nowSelectRane.position.x, stageSpeed * 0.3f);
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
        if (!isShot && !isChangeRane && tokenNum > 0) { // 移動中は打てない 残弾が残っていたら
            tokenNum -= 1;
            isShot = true;
            int rundomSelect = Random.Range (0, 5); // ランダムな弾を生成
            GameObject newBullet = Instantiate (bulletPrefabs[rundomSelect], nowSelectRane.position, Quaternion.identity, bulletParent);
            BulletMove bullet = newBullet.GetComponent<BulletMove> ();
            bullet.SetSpeed (stageSpeed);
            audioSource.PlayOneShot (clip); // SEを流す

            shotSpeed = stageSpeed;
            if (stageSpeed <= 1) { // 1だと遅すぎるため
                shotSpeed = 2;
            }
            yield return new WaitForSeconds (0.8f / (shotSpeed)); // ステージ速度によって連射速度を早くする
            isShot = false;
        }
    }

    // もキュの数表示
    private void ShowTokenNumber () {
        //Debug.Log ("token: " + tokenNumber);
        tokenNumText.text = $"{tokenNum} たい";
    }

    public void SetTokenNum (int tokenNumber) {
        Debug.Log ("token:" + tokenNumber);
        this.tokenNum = tokenNumber;
    }

    public void SetSpeed (float speed) {
        this.stageSpeed = speed;
    }

    // 生成されている全ての弾丸の速度変更
    public void SetAllBulletSpeed () {
        for (int i = 0; i < bulletParent.childCount; i++) {
            BulletMove bullet = bulletParent.GetChild (i).GetComponent<BulletMove> ();
            bullet.SetSpeed (stageSpeed);
        }
    }

    // 大砲SEの設定
    public void SetSEVol (float value) {
        audioSource.volume = value;
    }
}