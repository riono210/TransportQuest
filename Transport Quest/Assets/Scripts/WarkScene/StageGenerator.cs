using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageGenerator : MonoBehaviour {

    [SerializeField] private GameObject[] stagePrefabs; // 生成するプレファブ
    private float moveSpeed; // ステージの進む速度

    [SerializeField] private List<GameObject> stageList; // ステージインスタンスのリスト
    [SerializeField] private GameObject stageParent;

    // Start is called before the first frame update
    void Start () {

    }

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
            stageList.Add (instanceObj);
        }
    }
}