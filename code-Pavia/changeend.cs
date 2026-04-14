using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeend : MonoBehaviour
{
    public string targetScene = "end"; // 目标场景名称
    public string playerTag = "Player"; // 玩家的标签

    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞的物体是否有玩家标签
        if (collision.gameObject.CompareTag(playerTag))
        {
            Debug.Log("玩家碰撞到物体，准备加载场景: " + targetScene);

            // 加载目标场景
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.Log("非玩家物体碰撞: " + collision.gameObject.name);
        }
    }
}