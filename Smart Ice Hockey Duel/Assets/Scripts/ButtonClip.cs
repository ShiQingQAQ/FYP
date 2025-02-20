using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClip : MonoBehaviour
{
    // ����������������Unity�༭��������AI��Puck����Ϸ����
    public GameObject AI;
    public GameObject Puck;

    public void StartGame()
    {
        // ������Ϊ"AirHockey2D"�ĳ���
        SceneManager.LoadScene("AirHockey2D");
    }

    // ������ϷΪ���Ѷȵķ���
    public void StartGameEazy()
    {
        // ��ȡAI��Ϸ�����ϵ�AiMovement�����������������ƶ��ٶ�Ϊ10
        AI.GetComponent<AiMovement>().maxMovementSpeed = 10;
        // ��ȡPuck��Ϸ�����ϵ�PuckScript�����������������ٶ�Ϊ15
        Puck.GetComponent<PuckScript>().maxSpeed = 15;
    }

    // ������ϷΪ�����Ѷȵķ���
    public void StartGameHard()
    {
        // ����AI������ƶ��ٶ�Ϊ25
        AI.GetComponent<AiMovement>().maxMovementSpeed = 25;
        // ����Puck������ٶ�Ϊ20
        Puck.GetComponent<PuckScript>().maxSpeed = 20;
    }

    // �����Ӯ����Ϸʱ���õķ���������ʤ������
    public void Vectory()
    {
        // ������Ϊ"Vectory"�ĳ���
        SceneManager.LoadScene("Vectory");
    }

    // ����������Ϸʱ���õķ���������ʧ�ܳ���
    public void Lose()
    {
        // ������Ϊ"Lose"�ĳ���
        SceneManager.LoadScene("Lose");
    }

    // �����ѡ���˳���Ϸʱ���õķ���
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
