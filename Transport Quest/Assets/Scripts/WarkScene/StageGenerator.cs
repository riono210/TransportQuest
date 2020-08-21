using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageGenerator : MonoBehaviour {

    [SerializeField] private GameObject[] stagePrefabs; // 生成するプレファブ
    private float stageSpeed; // ステージの進む速度

    [SerializeField] private List<GameObject> stageList; // ステージインスタンスのリスト
    [SerializeField] private GameObject stageParent; // ステージの速度、生成時の初速

    // Start is called before the first frame update
    // void Start () {

    // }

    // Update is called once per frame
    void Update () {
        Generate ();
    }

    // ステージ生成
    private void Generate () {
        if (stageList.Last ().transform.position.x >= -30) { // 最後に生成されたステージが-30を切ったら新しいものを生成
            int selectStage = Random.Range (0, stagePrefabs.Length);
            //Debug.Log ("rand:" + selectStage);

            // 生成
            GameObject instanceObj = Instantiate (stagePrefabs[selectStage], new Vector3 (0, 10, 0), Quaternion.identity, stageParent.transform);
            Vector3 lastPos = stageList.Last ().transform.localPosition;
            lastPos.x -= 10;

            if (selectStage <= 3) {
                lastPos.z = -1;
            } else if (selectStage >= 9) {
                lastPos.z = 1;
            } else {
                lastPos.z = 0;
            }
            instanceObj.transform.localPosition = lastPos;

            // 速度設定など
            StageControler ctl = instanceObj.GetComponent<StageControler> ();
            ctl.SetSpeed (stageSpeed);
            stageList.Add (instanceObj);
        }
    }

    // 現在存在しているステージの速度を変える
    public void ChangeStageSpeed () {
        StageControler stageCtl;
        foreach (var value in stageList) {
            stageCtl = value.GetComponent<StageControler> ();
            stageCtl.SetSpeed (stageSpeed);
        }
    }

    // 同じInstanceIDだった場合削除する
    public void DeleteAtStageList (int instanceID) {
        stageList.RemoveAll (stage => stage.GetInstanceID () == instanceID);
    }

    // スピードの設定
    public void SetSpeed (float moveSpeed) {
        this.stageSpeed = moveSpeed;
    }
}