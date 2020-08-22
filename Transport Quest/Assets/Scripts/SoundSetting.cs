using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour {

    [SerializeField] private GameObject settingUI; // 音量セッティングUI
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    [SerializeField] private GameControler controler;

    [SerializeField] private AudioSource audioSource; // se音量確認用

    // Start is called before the first frame update
    void Start () {
        settingUI.SetActive (false);
    }

    // Update is called once per frame
    // void Update () {

    // }

    // ウィンドウのon/off
    public void SetActiveUI (bool flag) {
        settingUI.SetActive (flag);
    }

    // BGMの大きさを変える
    public void ChangeBGMVolume () {
        controler.SetSoundVolume (bgmSlider.value);　
    }

    // SEの大きさを変える
    public void ChnageSEVolume () {
        controler.SetSEVolume (seSlider.value);
        audioSource.volume = seSlider.value;
        audioSource.Play ();
    }
}