using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationIncrease : MonoBehaviour {

    private Quaternion rotatorQuaternion;
    private Vector3 rotatorEuler;
    public float speed = 0.8f; // 角速度
    public float speedFactor = 1f; // 速度倍率

    // Start is called before the first frame update
    void Start () {
        rotatorQuaternion = this.transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate () {
        EulerRotation ();
    }

    // 回転させる
    private void EulerRotation () {
        // 現在のQuaternionをオイラー角に変換
        rotatorEuler = rotatorQuaternion.eulerAngles;

        rotatorEuler = new Vector3 (rotatorEuler.x, rotatorEuler.y + (speed * speedFactor), rotatorEuler.z);

        // vector3をquaternionに変換
        rotatorQuaternion = Quaternion.Euler (rotatorEuler);
        this.transform.localRotation = rotatorQuaternion;
    }

    // 判定を受けて速度調整
    public void SpeedAdjust (float speedDelta) {

        if (speedDelta <= 0) { // batの場合
            StartCoroutine (TemporarySlowdown (speedDelta));
        } else {
            speedFactor += speedDelta;
        }

        if (speedFactor <= 0.5f) {  // スピードの下限
            speedFactor = 0.5f;
        } else if (speedFactor >= 20f) {  // スピードの上限
            speedFactor = 20f;
        }
    }

    // badだった時に一瞬止める
    public IEnumerator TemporarySlowdown (float speedDelta) {
        float tempSpeedFactor = speedFactor;
        //Debug.Log ("factor:" + tempSpeedFactor);
        speedFactor = speedFactor * 0.1f;

        yield return new WaitForSeconds (0.01f);

        speedFactor = tempSpeedFactor + speedDelta;
        if (speedFactor <= 0.5f) {  // スピードの下限
            speedFactor = 0.5f;
        }
        //Debug.Log ("aftor: " + speedFactor);
    }
}