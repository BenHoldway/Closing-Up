using System;
using TMPro;
using UnityEngine;

public class GameFail : MonoBehaviour
{
    bool gameFailed;

    [SerializeField] GameObject gameFailUI;

    private void Start()
    {
        gameFailed = false;
        gameFailUI.SetActive(false);
    }

    void OnEnable()
    {
        MoneyManager.Fail += EndGame;
        FamilyConditions.Fail += EndGame;
    }

    void OnDisable()
    {
        MoneyManager.Fail -= EndGame;
        FamilyConditions.Fail -= EndGame;
    }

    void EndGame(bool ranOutOfMoney)
    {
        if (gameFailed)
            return;

        gameFailed = true;
        Time.timeScale = 0;

        gameFailUI.SetActive(true);

        //Display how many shifts the player completed
        gameFailUI.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = ShiftManager.Instance.ShiftCount.ToString();

        //Display which reason the player failed; money or family death
        GameObject moneyReason = gameFailUI.transform.GetChild(2).GetChild(0).gameObject;
        GameObject familyReason = gameFailUI.transform.GetChild(2).GetChild(1).gameObject;
        if (ranOutOfMoney)
        {
            moneyReason.SetActive(true);
            familyReason.SetActive(false);
        }
        else
        {
            moneyReason.SetActive(false);
            familyReason.SetActive(true);
        }
    }
}
