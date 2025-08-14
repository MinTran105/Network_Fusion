using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceManager : MonoBehaviour
{
    public Action<int> OnSumDiceCalculated;

    [SerializeField] private RollDice[] rollDices;
    private Dictionary<RollDice, Transform> rollDiceTransforms = new Dictionary<RollDice, Transform>();
    private int[] diceResults;
    private int diceStoppedCount;
    private bool canRoll = true;

    private void Start()
    {
        foreach (RollDice rollDice in rollDices)
        {
            if (rollDice != null)
            {
                Transform transform = rollDice.gameObject.transform;
                if (transform != null)
                {
                    rollDiceTransforms[rollDice] = transform;
                }
                else
                {
                    Debug.LogWarning($"RollDice {rollDice.name} does not have a Transform component.");
                }
            }
        }
    }
    private void OnEnable()
    {
        diceResults = new int[rollDices.Length];
        diceStoppedCount = 0;

        for (int i = 0; i < rollDices.Length; i++)
        {
            int index = i;
            rollDices[i].OnDiceStopped += result => DiceStopped(index, result);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < rollDices.Length; i++)
        {
            rollDices[i].OnDiceStopped = null;
        }
    }

    private void Update()
    {
    }

    public void RollAllDice()
    {
        foreach (RollDice rollDice in rollDices)
        {
            if (!rollDice.gameObject.activeSelf)
            {
                return;
            }
        }
        diceStoppedCount = 0;
        for (int i = 0; i < diceResults.Length; i++)
            diceResults[i] = 0;

        foreach (RollDice rollDice in rollDices)
            rollDice.DiceRolling();
        canRoll = false;
    }

    private void DiceStopped(int index, int result)
    {
        diceResults[index] = result;
        diceStoppedCount++;

        if (diceStoppedCount >= rollDices.Length)
        {
            int totalSum = 0;
            foreach (int r in diceResults)
                totalSum += r;

            OnSumDiceCalculated?.Invoke(totalSum);
            Debug.Log($"🎯 All dice stopped. Total sum: {totalSum}");

        }
        canRoll = true;
    }
    public void HideDice()
    {
        StartCoroutine(DiceCoroutine(-1,new WaitForSeconds(2)));
        
    }
    public void ShowDice()
    {
        StartCoroutine(DiceCoroutine(1,new WaitForSeconds(0.5f)));
    }
    IEnumerator  DiceCoroutine(int index,WaitForSeconds sec)
    {
       yield return  sec;
        if (index == -1)
        {
            foreach (RollDice dice in rollDices)
            {
                dice.gameObject.SetActive(false);
            }
        }else
        if(index == 1)
        {
            foreach (RollDice rollDice in rollDices)
            {              
               if(rollDiceTransforms.ContainsKey(rollDice))
                {
                    rollDice.gameObject.SetActive(true);
                    rollDice.ResetDice();

                }
                else
                {
                    Debug.LogWarning($"RollDice {rollDice.name} does not have a Transform mapping.");
                }
            }
        }
       

            yield return null;
    }
}