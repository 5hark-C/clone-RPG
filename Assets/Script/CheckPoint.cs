using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string ID;
    public bool activationStatus;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    [ContextMenu("Generate checkpoint ID")]
    private void GenerateID()
    {
        ID = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        if(activationStatus == false)
            AudioManager.instance.PlaySFX(5, transform);


        activationStatus = true;
        anim.SetBool("Active", true);
    }
}
