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
        if (stageList.Last ().transform.position.x >= -30) {
            // 生成
            GameObject instanceObj = Instantiate (stagePrefabs[0], new Vector3 (0, 10, 0), Quaternion.identity, stageParent.transform);
            instanceObj.transform.localPosition = new Vector3 (-40, 7, 0);

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