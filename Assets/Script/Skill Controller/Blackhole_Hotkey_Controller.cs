using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackHole;

    public void SetupHotKey(KeyCode _myHotkey,Transform _myEnemy,BlackholeSkillController _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotkey = _myHotkey;
        myText.text = _myHotkey.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotkey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
