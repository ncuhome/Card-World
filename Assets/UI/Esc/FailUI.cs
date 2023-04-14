using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailUI : MonoBehaviour
{
    public void Quit()    //退出游戏
    {
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        //打包时不能使用
        //UnityEditor.EditorApplication.isPlaying = false;
        //测试时不能执行，打包后可以执行
        Application.Quit();
    }

    public void ReLoad() //重新加载场景
    {
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        Debug.Log("Relodad");
        GameObject.Find("FailPanel").transform.localScale = Vector3.zero;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
