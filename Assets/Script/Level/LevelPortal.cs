using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{
    public string nextLevelName; // 下一个关卡的场景名称

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            // 保存游戏数据，可根据需求实现
            SaveManager.instance.SaveGame();

            // 加载下一个关卡
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
