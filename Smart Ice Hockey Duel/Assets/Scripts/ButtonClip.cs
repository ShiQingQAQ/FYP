using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClip : MonoBehaviour
{
    // 公开变量，允许在Unity编辑器中设置AI和Puck的游戏对象
    public GameObject AI;
    public GameObject Puck;

    public void StartGame()
    {
        // 加载名为"AirHockey2D"的场景
        SceneManager.LoadScene("AirHockey2D");
    }

    // 设置游戏为简单难度的方法
    public void StartGameEazy()
    {
        // 获取AI游戏对象上的AiMovement组件，并设置其最大移动速度为10
        AI.GetComponent<AiMovement>().maxMovementSpeed = 10;
        // 获取Puck游戏对象上的PuckScript组件，并设置其最大速度为15
        Puck.GetComponent<PuckScript>().maxSpeed = 15;
    }

    // 设置游戏为困难难度的方法
    public void StartGameHard()
    {
        // 设置AI的最大移动速度为25
        AI.GetComponent<AiMovement>().maxMovementSpeed = 25;
        // 设置Puck的最大速度为20
        Puck.GetComponent<PuckScript>().maxSpeed = 20;
    }

    // 当玩家赢得游戏时调用的方法，加载胜利场景
    public void Vectory()
    {
        // 加载名为"Vectory"的场景
        SceneManager.LoadScene("Vectory");
    }

    // 当玩家输掉游戏时调用的方法，加载失败场景
    public void Lose()
    {
        // 加载名为"Lose"的场景
        SceneManager.LoadScene("Lose");
    }

    // 当玩家选择退出游戏时调用的方法
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
