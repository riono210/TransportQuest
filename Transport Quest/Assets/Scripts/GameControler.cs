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

    [SerializeField] private CannonSelector cannon; // もキュの数渡し
    [SerializeField] private CharactorControler character; // 終了判定とアニメーション

    [SerializeField] private GameObject gameOverUI; // ゲームオーバー時のUI

    [SerializeField] private AudioClip[] bgms; //BGM
    private AudioSource audioSource;
    private float seVolume = 1; // seの大きさ

    private int tokenNum = 0; // もキュの数
    //private bool isGameOver; // ゲーム終了判定
    private bool isWalk; // 散歩シーンか

    private bool isGameOverOnce; // ゲームオーバー出現フラグ

    // Start is called before the first frame update
    void Start () {
        SetUp ();
        //StartCoroutine (WaitCamMove ());
    }

    // Update is called once per frame
    void Update () {
        if (isWalk) {
            Gameover ();
        }
    }

    // ゲームスタートボタンおした時
    public void StartMultipul () {
        StartCoroutine (WaitCamMove ());
        demoStageObjs[1].SetActive (false);
    }

    // セットアップ
    private void SetUp () {
        audioSource = GetComponent<AudioSource> ();
        isGameOverOnce = false;
        isWalk = false;
        foreach (var value in walkingStageObjs) { // 散歩ステージの非表示
            value.SetActive (false);
        }

        foreach (var value in demoStageObjs) { // でもステージの表示
            value.SetActive (true);
        }
        multipulRoomObjs[0].SetActive (true); // 増殖ステージを表示

        multipulRoomObjs[1].SetActive (false); // UI非表示
        cameras[0].Priority = 20; // カメラ設定

        // 増殖ステージのコンポーネントのoff
        foreach (var value in multipulComponent) {
            value.enabled = false;
        }

        // 散歩ステージのコンポーネントのoff
        foreach (var value in walkComponent) {
            value.enabled = false;
        }

        // ゲームオーバーUIを非表示
        gameOverUI.SetActive (false);

        multipulStartText.SetActive (false); // 増殖ステージのスタートテキストoff
        // タイトルBGM
        BGMSet (0);
    }

    // BGMのセットと再生
    private void BGMSet (int index) {
        audioSource.Stop ();
        audioSource.clip = bgms[index];
        audioSource.Play ();

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

        // ちょっと待つ？

        // 増殖BGM
        BGMSet (1);

        yield return new WaitForSeconds (2f);

        multipulStartText.SetActive (false);
        // 回転開始
        foreach (var value in multipulComponent) {
            value.enabled = true;
            if (value.GetType () == typeof (ChekerDistance)) { // SE音量を設定
                value.GetComponent<ChekerDistance> ().SetSEVol (seVolume);
            }
        }
    }

    //  増加シーン終了 移動
    private IEnumerator MoveWalkStage () {
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

        // もキュ渡し
        cannon.SetTokenNum (tokenNum);
        // UIの表示
        walkingStageObjs[1].SetActive (true);

        // 散歩ステージスタート表示
        walkStartText.GetComponent<TextMeshProUGUI> ().text = "すたーと!";
        walkStartText.SetActive (true);

        // 散歩ステージBGM
        BGMSet (2);

        yield return new WaitForSeconds (2f);
        // スタート非表示
        walkStartText.SetActive (false);

        // 散歩ステージスクリプト開始
        foreach (var value in walkComponent) {
            value.enabled = true;
            if (value.GetType () == typeof (CannonSelector)) { // SE音量を設定
                value.GetComponent<CannonSelector> ().SetSEVol (seVolume * 0.5f);
            }
        }

        // 判定開始
        isWalk = true;

    }

    // タイマー側で呼び出す 
    public void StartWalk () {
        StartCoroutine (MoveWalkStage ());
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

    // Unityちゃんがmissを踏んだ場合
    public void Gameover () {
        if (character.GetIsGamoOver () && !isGameOverOnce) {
            isGameOverOnce = true;
            // ゲームオーバーBGM
            BGMSet (3);

            // unitychannアニメーション
            character.GameoverAnimation ();
            // 散歩ステージスクリプト終了
            walkComponent[1].GetComponent<ScoreControler> ().SetIsStop (true);
            walkComponent[1].GetComponent<ScoreControler> ().SetStageSpeed (0f);

            // ゲームオーバーUIを表示
            gameOverUI.SetActive (true);
            Debug.Log ("gameover");
        }
    }

    // もキュの数セット
    public void SetTokenNum (int num) {
        this.tokenNum = num;
    }

    // BGMの音量変更
    public void SetSoundVolume (float value) {
        audioSource.volume = value;
    }

    // SEの音量セット
    public void SetSEVolume (float value) {
        this.seVolume = value;
    }
}