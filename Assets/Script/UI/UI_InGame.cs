using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackImage;
    [SerializeField] private Image flaskImage;

    private SkillManger skills;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmoummt;
    [SerializeField] private float increaseRate = 100;

    void Start()
    {
        if(playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skills = SkillManger.instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpDateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.LeftControl) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
            SetCooldownOf(blackImage);

        if (Input.GetKeyDown(KeyCode.Q) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordImage, skills.sword.cooldown);
        CheckCooldownOf(blackImage, skills.blackhole.cooldown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpDateSoulsUI()
    {
        if (soulsAmoummt < PlayerManager.instance.GetCurrency())
        {
            soulsAmoummt += Time.deltaTime * increaseRate;
        }
        else
        {
            soulsAmoummt = PlayerManager.instance.GetCurrency();
        }

        currentSouls.text = ((int)soulsAmoummt).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image,float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

}
