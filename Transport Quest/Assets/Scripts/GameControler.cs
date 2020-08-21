using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class GameControler : MonoBehaviour {

    [SerializeField] private GameObject[] multipulRoomObjs;
    [SerializeField] private GameObject[] walkingStageObjs;
    [SerializeField] private GameObject[] demoStageObjs;

    [SerializeField] private Behaviour[] multipulComponent; // 増殖シーンのスクリプト
    [SerializeField] private GameObject multipulStartText; // スタートテキスト

    [SerializeField] private Behaviour[] walkComponent; // 散歩シーンのスクリプト
    [SerializeField] private GameObject walkStartText; // スタートテキスト

    [SerializeField] private CinemachineVirtualCamera[] cameras; // バーチャルカメラ
    [SerializeField] private Animation cameraAim;
    [SerializeField] private AnimationClip[] animations;

    private int tokenNum = 0; // もキュの数
    private bool isGameOver; // ゲーム終了判定

    // Start is called before the first frame update
    void Start () {
        SetUp ();
        StartCoroutine (WaitCamMove ());
    }

    // Update is called once per frame
    void Update () {

    }

    // ゲームスタートボタンおした時
    private void StartMultipul () {
        StartCoroutine (WaitCamMove ());
    }

    // セットアップ
    private void SetUp () {
        isGameOver = false;
        foreach (var value in walkingStageObjs) { // 散歩ステージの非表示
            value.SetActive (false);
        }
        demoStageObjs[0].SetActive (true); // デモステージを表示
        // デモのUIを表示 追加しろ
        multipulRoomObjs[0].SetActive (true); // 増殖ステージを表示

        multipulRoomObjs[1].SetActive (false); // UI非表示
        cameras[0].Priority = 20; // カメラ設定

        // 増殖ステージのコンポーネントのoff
        foreach (var value in multipulComponent) {
            value.enabled = false;
        }

        // 散歩ステージのコンポーネントのoff

        multipulStartText.SetActive (false); // 増殖ステージのスタートテキストoff
    }

    // ゲーム開始のカメラ移動
    private IEnumerator WaitCamMove () {
        // カメラ移動まで待機
        yield return StartCoroutine (ChangeCamStart ());
        // 完全に終わるまで待つ
        yield return new WaitForSeconds (2f);

        // demoシーンOFF
        demoStageObjs[0].SetActive (false);

        // UI表示
        multipulRoomObjs[1].SetActive (true);
        yield return new WaitForSeconds (1f);
        // スタートテキスト
        multipulStartText.GetComponent<TextMeshProUGUI> ().text = "すたーと!";
        multipulStartText.SetActive (true);
        // ピピーSE

        yield return new WaitForSeconds (2f);

        multipulStartText.SetActive (false);
        // 回転開始
        foreach (var value in multipulComponent) {
            value.enabled = true;
        }
    }

    // タイマー側で呼び出す  増加シーン終了
    public IEnumerator StartWalk () {
        // テキスト表示
        multipulStartText.GetComponent<TextMeshProUGUI> ().text = "しゅうーりょう!";
        multipulStartText.SetActive (true);
        // ピピーSE

        yield return new WaitForSeconds (2f);
        // 終了テキスト非表示
        multipulStartText.SetActive (false);
        // 増殖ステージUIを非表示
        multipulRoomObjs[1].SetActive (false);

        // walkSceneをONにする
        walkingStageObjs[0].SetActive (true);

        // カメラ移動
        yield return StartCoroutine (ChangeCamWalk ());
        // 完全に終わるまで待つ
        yield return new WaitForSeconds (2f);
        // 増殖シーンoff
        multipulRoomObjs[0].SetActive (false);

        // UIの表示
        walkingStageObjs[1].SetActive (true);
        // スタート表示
        walkStartText.GetComponent<TextMeshProUGUI> ().text = "すたーと!";
        walkStartText.SetActive (true);

        // 散歩ステージスクリプト開始

    }

    // スタート時のカメラ移動
    private IEnumerator ChangeCamStart () {
        yield return new WaitForSeconds (1f);

        cameras[1].Priority = 21;
        cameras[0].Priority = 10;
        cameraAim.clip = animations[0];
        cameraAim.Play ();
        yield return new WaitForSeconds (2f);

        cameras[2].Priority = 22;
        cameras[1].Priority = 10;
        // 優先度の調整
        cameras[2].Priority = 15;
    }

    // お散歩シーン移動時
    private IEnumerator ChangeCamWalk () {
        yield return new WaitForSeconds (1f);

        cameras[1].Priority = 21;
        cameras[2].Priority = 10;
        cameraAim.clip = animations[1];
        cameraAim.Play ();
        yield return new WaitForSeconds (2f);

        cameras[0].Priority = 22;
        cameras[1].Priority = 11;

        cameras[0].Priority = 15;
    }

    // もキュの数セット
    private void SetTokenNum (int num) {
        this.tokenNum = num;
    }

    private void SetIsGameOver (bool flag) {
        this.isGameOver = flag;
    }

}