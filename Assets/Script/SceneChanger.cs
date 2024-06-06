using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //SceneManager

public class SceneChanger : MonoBehaviour
{
   
    public void MainSceneChange()
        // acredev1SceneChange 이름의 함수 선언
    {
            SceneManager.LoadScene("MainScene");
            //SceneManager 메서드의 LoadScene 함수를 통해 acredev1.scene으로 씬 전환
    }
    public void GameSceneChange()
    {
        // acredev2SceneChanger
        SceneManager.LoadScene("GameScene");
        //SceneManager 메서드의 LoadScene 함수를 통해 acredev2.scene으로 씬 전환
    }
    public void MapSceneChange()
    {
        SceneManager.LoadScene("MapScene");
    }
    public void Stage1SceneChange()
    {
        SceneManager.LoadScene("Stage1Scene");
    }
    public void Stage2SceneChange()
    {
        SceneManager.LoadScene("Stage2Scene");
    }
    public void Stage3SceneChange()
    {
        SceneManager.LoadScene("Stage3Scene");
    }
    public void ShopSceneChange()
    {
        SceneManager.LoadScene("ShopScene");
    }
    public void TutorialsSceneChange()
    {
        SceneManager.LoadScene("TutorialsScene");
    }
}
