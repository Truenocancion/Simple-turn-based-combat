using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<BuffData> buffDataList;

    private Unit leftUnit;
    private Unit rightUnit;
    private List<Buff> buffs = new List<Buff>();
    private List<Buff> possibleSecondBuffsForLeftUnit = new List<Buff>();
    private List<Buff> possibleSecondBuffsForRightUnit = new List<Buff>();

    private bool leftPlayerTurn = true;
    private int currentRound = 1;

    [Header("Left unit")]
    public Slider leftUnitHealthSlider;
    public Slider leftUnitArmorSlider;
    public Slider leftUnitVampirismSlider;
    public TextMeshProUGUI leftUnitHealthText;
    public TextMeshProUGUI leftUnitArmorText;
    public TextMeshProUGUI leftUnitVampirismText;
    public TextMeshProUGUI leftUnitActiveBuffs;
    public Image leftUnitImage;
    [Header("Right unit")]
    public Slider rightUnitHealthSlider;
    public Slider rightUnitArmorSlider;
    public Slider rightUnitVampirismSlider;
    public TextMeshProUGUI rightUnitHealthText;
    public TextMeshProUGUI rightUnitArmorText;
    public TextMeshProUGUI rightUnitVampirismText;
    public TextMeshProUGUI rightUnitActiveBuffs;
    public Image rightUnitImage;
    [Header("Other")]
    public TextMeshProUGUI currentRoundText;

    private void Start()
    {
        InitUnits();
        CreateBuffs();
        UpdateUI();
    }

    private void InitUnits()
    {
        leftUnit = new Unit();
        rightUnit = new Unit();
    }

    private void CreateBuffs()
    {
        foreach (BuffData buffData in buffDataList)
        {
            Buff buff = new Buff(buffData);
            buffs.Add(buff);
        }
        possibleSecondBuffsForLeftUnit = new List<Buff>();
        possibleSecondBuffsForRightUnit = new List<Buff>();
    }

    public void Attack()
    {
        if (leftPlayerTurn)
        {
            leftUnit.Attack(rightUnit);
            if (leftUnit.Health == 0 || rightUnit.Health == 0)
            {
                Restart();
            }
            else
            {
                leftPlayerTurn = false;
            }
            UpdateUI();
            StartCoroutine(FadeUnitColor(rightUnitImage));
        }
        else
        {
            rightUnit.Attack(leftUnit);
            if (leftUnit.Health == 0 || rightUnit.Health == 0)
            {
                Restart();
            }
            else
            {
                leftPlayerTurn = true;
                currentRound += 1;
            }
            UpdateUI();
            StartCoroutine(FadeUnitColor(leftUnitImage));
        }
    }

    public void ApplyRandomBuff()
    {
        if (leftPlayerTurn)
        {
            if (leftUnit.firstBuff == null)
            {
                leftUnit.firstBuff = buffs[Random.Range(0, buffs.Count)];
                possibleSecondBuffsForLeftUnit = new List<Buff>(buffs);
                possibleSecondBuffsForLeftUnit.Remove(leftUnit.firstBuff);
            }
            else if (leftUnit.secondBuff == null)
            {
                leftUnit.secondBuff = possibleSecondBuffsForLeftUnit[Random.Range(0, buffs.Count)];
            }
            UpdateUI();
        }
        else
        {
            if (rightUnit.firstBuff == null)
            {
                rightUnit.firstBuff = buffs[Random.Range(0, buffs.Count)];
                possibleSecondBuffsForRightUnit = new List<Buff>(buffs);
                possibleSecondBuffsForRightUnit.Remove(rightUnit.firstBuff);
            }
            else if (rightUnit.secondBuff == null)
            {
                rightUnit.secondBuff = possibleSecondBuffsForRightUnit[Random.Range(0, buffs.Count)];
            }
            UpdateUI();
        }
    }

    public void Restart()
    {
        InitUnits();
        CreateBuffs();
        leftPlayerTurn = true;
        currentRound = 1;
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateUnitUI(leftUnit, leftUnitHealthSlider, leftUnitArmorSlider, leftUnitVampirismSlider, leftUnitHealthText, leftUnitArmorText, leftUnitVampirismText, leftUnitActiveBuffs);
        UpdateUnitUI(rightUnit, rightUnitHealthSlider, rightUnitArmorSlider, rightUnitVampirismSlider, rightUnitHealthText, rightUnitArmorText, rightUnitVampirismText, rightUnitActiveBuffs);

        currentRoundText.text = "Round " + currentRound.ToString();
    }

    private void UpdateUnitUI(Unit unit, Slider healthSlider, Slider armorSlider, Slider vampirismSlider, TextMeshProUGUI healthText, TextMeshProUGUI armorText, TextMeshProUGUI vampirismText, TextMeshProUGUI activeBuffsText)
    {
        healthSlider.maxValue = 100;
        armorSlider.maxValue = 100;
        vampirismSlider.maxValue = 100;
        healthSlider.value = unit.Health;
        armorSlider.value = unit.Armor;
        vampirismSlider.value = unit.Vampirism;
        healthText.text = unit.Health.ToString();
        armorText.text = unit.Armor.ToString();
        vampirismText.text = unit.Vampirism.ToString();

        string activeBuffsTextString = unit.firstBuff != null ? $"{unit.firstBuff.buffData.name} for {unit.firstBuff.turns}.\n" : "No buffs applied\n";
        activeBuffsTextString += unit.secondBuff != null ? $"{unit.secondBuff.buffData.name} for {unit.secondBuff.turns}." : "";
        activeBuffsText.text = activeBuffsTextString;
    }

    private IEnumerator<WaitForEndOfFrame> FadeUnitColor(Image unitImage)
    {
        Color startColor = unitImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            float t = elapsedTime / 1f;
            unitImage.color = Color.Lerp(startColor, Color.red, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        unitImage.color = startColor;
    }
}
