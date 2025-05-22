using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skills
{
    [Header("Clone info")]
    [SerializeField] private float attackMultipller;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("Clone attack")]
    [SerializeField] private UI_SkilltreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggresive clone")]
    [SerializeField] private UI_SkilltreeSlot aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }


    [Header("Multiple clone")]
    [SerializeField] private UI_SkilltreeSlot multipleUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;


    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkilltreeSlot crystalInsteadUnlockButton;
    public bool crysatlInsteadOfClone;


    protected override void Start()
    {
        base.Start();


        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    #region Unlock region

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultipller = cloneAttackMultiplier;
        }
    }

    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultipller = aggresiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultipller = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockButton.unlocked)
        {
            crysatlInsteadOfClone = true;
        }
    }


    #endregion


    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggresiveClone();
        UnlockMultiClone();
        UnlockCrystalInstead();
    }

    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if (crysatlInsteadOfClone)
        {
            SkillManger.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition,cloneDuration,canAttack,_offset,FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player,attackMultipller);
    }



    //防反成功后创造克隆攻击
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
            StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }
    
    //延时创造克隆
    private IEnumerator CloneDelayCorotine(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.5f);
            CreateClone(_transform,_offset);
    }
}
