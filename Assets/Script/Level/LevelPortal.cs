using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{
    public string nextLevelName; // ��һ���ؿ��ĳ�������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            // ������Ϸ���ݣ��ɸ�������ʵ��
            SaveManager.instance.SaveGame();

            // ������һ���ؿ�
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
