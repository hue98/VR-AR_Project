using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public GameObject playerPiecePrefab; // 말의 프리팹
    public Transform[] playerTileParents; // 플레이어별 타일 부모 오브젝트 배열 (P1, P2 등)
    public float moveDuration = 0.5f;  // 타일 간 이동 시간
    public float arcHeight = 2.0f; // 포물선의 최대 높이

    private Dictionary<int, Transform[]> playerTiles = new Dictionary<int, Transform[]>(); // 플레이어별 타일 리스트
    private GameObject[] playerPieces; // 플레이어 말들
    private int[] playerTileIndices; // 각 플레이어의 현재 타일 인덱스
    private int currentPlayerIndex = 0; // 현재 플레이어의 차례
    private bool isMoving = false; // 이동 중 여부
    private int totalPlayers = 0; // 게임 인원수
    private List<int> turnOrder; // 플레이어 순서
    private bool isGameStarted = false;

    // 말은 y축으로 3만큼 떨어져 있음
    private float pieceHeight = 1.1f;

    void Start()
    {
        SetTotalPlayers(4);
        // 타일 리스트 초기화
        InitializeTileLists();
    }

    // 타일 리스트 초기화
    private void InitializeTileLists()
    {
        for (int i = 0; i < playerTileParents.Length; i++)
        {
            Transform parent = playerTileParents[i];
            Transform[] tiles = new Transform[parent.childCount];
            for (int j = 0; j < parent.childCount; j++)
            {
                tiles[j] = parent.GetChild(j);
            }
            playerTiles[i] = tiles;
            Debug.Log($"Player {i + 1} has {tiles.Length} tiles.");
        }
    }


    // 플레이어 수 설정 함수
    public void SetTotalPlayers(int players)
    {
        if (players < 2 || players > playerTileParents.Length)
        {
            Debug.LogError("Invalid player count! Must be between 2 and " + playerTileParents.Length + ".");
            return;
        }
        totalPlayers = players;
        Debug.Log("Player count set to: " + totalPlayers);
    }

    // 게임 시작 함수
    public void StartGame()
    {
        if (totalPlayers < 2 || totalPlayers > playerTileParents.Length)
        {
            Debug.LogError("Invalid player count! Set the player count first.");
            return;
        }

        if (isGameStarted)
        {
            Debug.LogError("Game has already started.");
            return;
        }
        // 플레이어 말 생성 및 초기화
        playerPieces = new GameObject[totalPlayers];
        playerTileIndices = new int[totalPlayers];
        turnOrder = new List<int>();

        for (int i = 0; i < totalPlayers; i++)
        {
            // playerPieces[i] = Instantiate(playerPiecePrefab, startingPositionParent.GetChild(i).position, Quaternion.identity);
            playerPieces[i] = Instantiate(playerPiecePrefab, playerTileParents[i].GetChild(0).position + new Vector3(0, pieceHeight, 0), Quaternion.identity);
            playerTileIndices[i] = 0; // 각 플레이어는 첫 번째 타일에서 시작
            turnOrder.Add(i); // 순서를 임시로 추가
        }

        // 플레이어 순서 랜덤화
        turnOrder = Shuffle(turnOrder);
        Debug.Log("Turn order: " + string.Join(", ", turnOrder));
        isGameStarted = true;

        StartTurn();
    }

    // 현재 플레이어의 차례 시작
    public void StartTurn()
    {
        if (!isGameStarted)
        {
            Debug.LogError("Game has not started yet.");
            return;
        }

        Debug.Log($"Player {turnOrder[currentPlayerIndex] + 1}'s turn!");
        // 주사위를 굴리는 부분은 외부에서 호출됨
    }

    // 주사위 결과로 이동 시작
    // public void MovePlayerPiece(int steps)
    public void MovePlayerPiece()
    {
        int steps = dicecheckzone.number;
        if (isMoving)
        {
            Debug.LogWarning("Currently moving a piece. Wait for the turn to finish.");
            return;
        }

        int playerIndex = turnOrder[currentPlayerIndex];
        Debug.Log($"Player {playerIndex + 1} rolls: {steps}");
        StartCoroutine(MovePieceWithArc(playerIndex, steps));
    }

    // 말 이동 Coroutine
    private System.Collections.IEnumerator MovePieceWithArc(int playerIndex, int steps)
    {
        isMoving = true;

        // 해당 플레이어의 타일 리스트 가져오기
        Transform[] tiles = playerTiles[playerIndex];
        int targetTileIndex = (playerTileIndices[playerIndex] + steps) % tiles.Length;

        while (playerTileIndices[playerIndex] != targetTileIndex)
        {
            playerTileIndices[playerIndex] = (playerTileIndices[playerIndex] + 1) % tiles.Length;

            Vector3 startPos = playerPieces[playerIndex].transform.position;
            Vector3 targetPos = tiles[playerTileIndices[playerIndex]].position;
            targetPos += new Vector3(0, pieceHeight, 0);

            float elapsedTime = 0;
            while (elapsedTime < moveDuration)
            {
                float t = elapsedTime / moveDuration;
                float x = Mathf.Lerp(startPos.x, targetPos.x, t);
                float z = Mathf.Lerp(startPos.z, targetPos.z, t);
                float y = Mathf.Lerp(startPos.y, targetPos.y, t) + arcHeight * Mathf.Sin(Mathf.PI * t);

                playerPieces[playerIndex].transform.position = new Vector3(x, y, z);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerPieces[playerIndex].transform.position = targetPos;
            yield return new WaitForSeconds(0.1f);
        }

        isMoving = false;
        Debug.Log($"Player {playerIndex + 1} reached tile {playerTileIndices[playerIndex]}.");
        EndTurn();
    }

    // 차례 종료 및 다음 플레이어로 전환
    private void EndTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % totalPlayers;
        StartTurn();
    }

    // 리스트를 섞는 유틸리티 함수
    private List<int> Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}
