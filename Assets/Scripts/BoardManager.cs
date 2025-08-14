using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardManager : NetworkBehaviour
{
    [Header("Prefabs & References")]
    public DiceManager diceManager;
    public Point pointPrefab;
    public Transform boardParent;
    public GameObject playerPrefab;
    public GameObject playerPrefab2;

    [Header("Board Settings")]
    [SerializeField] private float pointSpacing = 5f;

    private List<Point> pointList = new();
    private GameObject player1Instance;
    private GameObject player2Instance;

    
    private int movesLeft;
    private bool isMoving;
    private bool isPlayerTurn = true;

    private void OnEnable() => diceManager.OnSumDiceCalculated += StartMovement;
    private void OnDisable() => diceManager.OnSumDiceCalculated -= StartMovement;

    private void Start()
    {
        GenerateBoard();
        //SpawnPlayerAtStart(playerPrefab, ref player1Instance);
        //SpawnPlayerAtStart(playerPrefab2, ref player2Instance);
    }

    private void GenerateBoard()
    {
        for (int i = 1; i <= 100; i++)
        {
            Point newPoint = Instantiate(pointPrefab, boardParent);
            newPoint.Init(i, i.ToString());

            int row = (i - 1) / 10;
            int col = (i - 1) % 10;
            bool isLeftToRight = row % 2 == 0;

            float xOffset = (isLeftToRight ? col : 9 - col) * pointSpacing;
            float yOffset = row * pointSpacing;

            newPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset, yOffset);
            pointList.Add(newPoint);
        }
    }

    private void SpawnPlayerAtStart(GameObject prefab, ref GameObject instance)
    {
        instance = Instantiate(prefab);
        instance.transform.position = pointList[0].PointPosition.position;
        instance.name = $"Player_{prefab.name}";
        Debug.Log($"{instance.name} starting at position: 1");
    }

    public Vector3 SetNetworkStartSpawnPlayer()
    {
        return pointList[0].PointPosition.position;
    }
    private void StartMovement(int totalDice)
    {
        movesLeft = totalDice;
        isMoving = true;
        diceManager.HideDice();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RollAllDice();
        }

        if (isMoving)
        {
            //MovePlayerStepByStep();
        }
    }

    public void RollAllDice()
    {
        if (isMoving)
        {
            Debug.LogWarning("Cannot roll dice while moving!");
            return;
        }

        diceManager.RollAllDice();
    }

    //public void MovePlayerStepByStep(GameObject player)
    //{
    //    if (CheckWins()) return;

    //    int currentPos = isPlayerTurn ? currentPlayer1Pos : currentPlayer2Pos;
    //    GameObject currentPlayer = player;

    //    if (movesLeft <= 0 || currentPos >= pointList.Count - 1)
    //    {
    //        isMoving = false;
    //        isPlayerTurn = !isPlayerTurn;
    //        diceManager.ShowDice();
    //        return;
    //    }

    //    Vector3 targetPosition = pointList[currentPos + 1].PointPosition.position;
    //    currentPlayer.transform.position = Vector3.Lerp(currentPlayer.transform.position, targetPosition, Time.deltaTime * 15f);

    //    if (Vector3.Distance(currentPlayer.transform.position, targetPosition) < 0.01f)
    //    {
    //        if (isPlayerTurn)
    //            currentPlayer1Pos++;
    //        else
    //            currentPlayer2Pos++;

    //        movesLeft--;

    //        Debug.Log($"{currentPlayer.name} moved to position: {(isPlayerTurn ? currentPlayer1Pos : currentPlayer2Pos) + 1}");
    //    }
    //}

    //private bool CheckWins()
    //{
    //    if (currentPlayer1Pos >= pointList.Count - 1)
    //    {
    //        Debug.Log("🎉 Player 1 Wins!");
    //        isMoving = false;
    //        return true;
    //    }
    //    if (currentPlayer2Pos >= pointList.Count - 1)
    //    {
    //        Debug.Log("🎉 Player 2 Wins!");
    //        isMoving = false;
    //        return true;
    //    }
    //    return false;
    //}
}