using UnityEngine;

// 挂载到新场景的任意初始化对象（如Canvas、空物体）
public class SceneMouseInit : MonoBehaviour
{
    void Start()
    {
        // 重置鼠标：显示鼠标 + 解除锁定（根据新场景需求调整）
        Cursor.visible = true; // 显示鼠标（关键）
        Cursor.lockState = CursorLockMode.None; // 解除锁定（可选，根据场景需求）
    }
}
