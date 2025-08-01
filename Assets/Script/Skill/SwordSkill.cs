﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skills
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private UI_SkilltreeSlot bounceUnlockButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Peirce info")]
    [SerializeField] private UI_SkilltreeSlot pierceUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private UI_SkilltreeSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGraivty;

    [Header("Skill info")]
    [SerializeField] private UI_SkilltreeSlot swordUnlockButton;
    public bool swordUnlocked {  get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passive skills")]
    [SerializeField] private UI_SkilltreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked {  get; private set; }
    [SerializeField] private UI_SkilltreeSlot vulnurableUnlockButton;
    public bool vulnurableUnlocked {  get; private set; }

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenereatDots();

        SetupGraivty();

        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnurableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnurable);
    }


    private void SetupGraivty()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGraivty;
           
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for(int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword=Instantiate(swordPrefab,player.transform.position,transform.rotation);
        SwordSkillController newSwordScript=newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount,bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration,hitCooldown);




        newSwordScript.SetupSword(finalDir,swordGravity,player,freezeTimeDuration,returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);

        int soundIndex;
        if (swordType == SwordType.Pierce)
        {
            soundIndex = 28;
        }
        else
        {
            soundIndex = 27;
        }
        AudioManager.instance.PlaySFX(soundIndex, player.transform);
    }

    #region Unlock rengion

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounceSword();
        UnlockSpinSword();
        UnlockPierceSword();
        UnlockTimeStop();
        UnlockVulnurable();
    }

    private void UnlockTimeStop()
    {
        if(timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVulnurable()
    {
        if (vulnurableUnlockButton.unlocked)
            vulnurableUnlocked = true;
    }

    private void UnlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockBounceSword()
    {
        if(bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }

    private void UnlockPierceSword()
    {
        if (pierceUnlockButton.unlocked)
            swordType = SwordType.Pierce;
    }

    private void UnlockSpinSword()
    {
        if(spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }


    #endregion

    #region Aim region

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for(int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenereatDots()
    {
        dots = new GameObject[numberOfDots];
        for(int i = 0;i<numberOfDots;i++)
        {
            dots[i] = Instantiate(dotPrefab,player.transform.position,Quaternion.identity,dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
