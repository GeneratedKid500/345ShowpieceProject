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

    float timer;

    private void Start()
    {
        playerHP = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();
        healthBarSlider.maxValue = playerHP.GetMaxHP();
        healthBarSlider.value = playerHP.GetHP();
    }

    private void Update()
    {

        healthBarSlider.value = Mathf.Lerp(healthBarSlider.value, playerHP.GetHP(), Time.deltaTime*2);

        //healthBarSlider.value = playerHP.GetHP();
        if (playerHP.GetHP() < playerHP.GetMaxHP() / 10)
        {
            lowHPWarning.alpha = Mathf.Lerp(lowHPWarning.alpha, 1, Time.deltaTime);
        }
        else
        {
            lowHPWarning.alpha = Mathf.Lerp(lowHPWarning.alpha, 0, Time.deltaTime*2);
        }

        if (weaponSwitch.alpha > 0)
        {
            weaponSwitch.alpha = Mathf.Lerp(lowHPWarning.alpha, 0, Time.deltaTime/2);
        }

        if (playerHP.GetHP() <= 0)
        {
            healthBarSlider.fillRect.gameObject.SetActive(false);
            deadHPWarning.alpha = Mathf.Lerp(deadHPWarning.alpha, 1.1f, Time.deltaTime*2);

            timer += Time.deltaTime;
            if (timer > 10)
            {
                Application.Quit();
            }
        }
    }

    public void AlterTextShowButton(string newText)
    {
        text.text = newText;
        weaponSwitch.alpha = 1;
    }
}
