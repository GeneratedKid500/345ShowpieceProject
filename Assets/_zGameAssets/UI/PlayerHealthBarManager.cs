using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBarManager : MonoBehaviour
{
    [Header("Player HP Bar")]
    [SerializeField] GameObject healthBar;
    [SerializeField] Slider healthBarSlider;
    [SerializeField] CanvasGroup lowHPWarning;
    [SerializeField] CanvasGroup deadHPWarning;
    HealthSystem playerHP;

    [Header("Weapon Switch Icon")]
    [SerializeField] CanvasGroup weaponSwitch;
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        playerHP = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();
        healthBarSlider.maxValue = playerHP.GetMaxHP();
    }

    private void Update()
    {
        healthBarSlider.value = playerHP.GetHP();
        if (playerHP.GetHP() < playerHP.GetMaxHP() / 10)
        {
            lowHPWarning.alpha = Mathf.Lerp(lowHPWarning.alpha, 1, Time.deltaTime);
        }
        else
        {
            lowHPWarning.alpha = Mathf.Lerp(lowHPWarning.alpha, 0, Time.deltaTime*2);
        }

        if (weaponSwitch.alpha > 1)
        {
            weaponSwitch.alpha = Mathf.Lerp(lowHPWarning.alpha, 0, Time.deltaTime/2);
        }

        if (playerHP.GetHP() <= 0)
        {
            healthBarSlider.fillRect.gameObject.SetActive(false);
            deadHPWarning.alpha = Mathf.Lerp(lowHPWarning.alpha, 1, Time.deltaTime);
        }
    }

    public void AlterTextShowButton(string newText)
    {
        text.text = newText;
        weaponSwitch.alpha = 1;
    }
}
