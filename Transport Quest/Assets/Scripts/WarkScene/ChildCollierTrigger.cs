using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollierTrigger : MonoBehaviour {

    [SerializeField] private StageGenerator stageGenerator; // ステージリスト管理用

    //　カメラ外になったら消す
    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Stage") {
            Debug.Log ("Delete");
            // hitするのは地面自体なのでその親を投げる
            SendInstanceID (other.transform.parent.gameObject);
            Destroy (other.transform.parent.gameObject);
        } else if (other.gameObject.tag == "Bullet") {
            Destroy (other.gameObject);
        }
    }

    // stageをリストから削除する
    private void SendInstanceID (GameObject destroyObj) {
        stageGenerator.DeleteAtStageList (destroyObj.GetInstanceID ());
    }
}