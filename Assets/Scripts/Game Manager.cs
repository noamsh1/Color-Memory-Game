using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TileManager tileManager;
    [SerializeField] Canvas singlePlayerCanvas;
    TMP_Text turnsNumText;
    [SerializeField] Canvas twoPlayersCanvas;
    TMP_Text playerOneScoreText;
    int playerOneScore = 0;
    TMP_Text playerTwoScoreText;
    int playerTwoScore = 0;
    TMP_Text playerTurnText;
    int tilesFlipped = 0;
    Dictionary<GameObject, int> flippedTilesDict = new Dictionary<GameObject, int>();
    public float delayTime = 1f;
    int playerNum;
    int turnsNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileManager.GameTileGridSetUp += SubscribeToTiles;
        playerNum = GameSettings.GetInstance().playerNum;
        switch (playerNum)
        {
            case 1:
                singlePlayerCanvas.gameObject.SetActive(true);
                twoPlayersCanvas.gameObject.SetActive(false);
                turnsNumText = singlePlayerCanvas.GetComponentInChildren<TMP_Text>();
                break;
            case 2:
                singlePlayerCanvas.gameObject.SetActive(false);
                twoPlayersCanvas.gameObject.SetActive(true);
                playerOneScoreText = twoPlayersCanvas.transform.Find("player 1 score text").gameObject.GetComponent<TMP_Text>();
                playerTwoScoreText = twoPlayersCanvas.transform.Find("player 2 score text").gameObject.GetComponent<TMP_Text>();
                playerTurnText = twoPlayersCanvas.transform.Find("player turn text").gameObject.GetComponent<TMP_Text>();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubscribeToTiles()
    {
        GameTile[,] gameTiles = tileManager.gameTiles;
        foreach (GameTile gameTile in gameTiles)
        {
            gameTile.TileFlipped += AddFlippedTile;
        }
    }

    public void AddFlippedTile(GameObject sender, int id)
    {
        if (tilesFlipped < 2)
        {
            sender.GetComponent<GameTile>().FlipTileToFront();
            flippedTilesDict.Add(sender, id);
            tilesFlipped++;
            if (tilesFlipped == 2)
            {
                StartCoroutine(CheckIfTilesMatch());
            }
        }
    }

    IEnumerator CheckIfTilesMatch()
    {
        int[] tileIdArray = new int[2];
        int i = 0;
        foreach (int id in flippedTilesDict.Values)
        {
            tileIdArray[i] = id;
            i++;
        }
        yield return new WaitForSeconds(delayTime);

        if (tileIdArray[0] == tileIdArray[1]) // tiles are matching
        {
            foreach (GameObject tile in flippedTilesDict.Keys)
            {
                Destroy(tile);
            }
            if (playerNum == 2)
            {
                if (turnsNum % 2 == 0) // player 1's turn
                {
                    playerOneScore++;
                    playerOneScoreText.SetText($"player 1's score: {playerOneScore}");
                }
                else // player 2's turn
                {
                    playerTwoScore++;
                    playerTwoScoreText.SetText($"player 2's score: {playerTwoScore}");
                }
            }
        }
        else // tiles are not matching
        {
            foreach (GameObject tile in flippedTilesDict.Keys) 
            {
                tile.GetComponent<GameTile>().FlipTileToBack();
            }
        }

        flippedTilesDict.Clear();
        tilesFlipped = 0;


        turnsNum++;
        switch (playerNum)
        {
            case 1:
                turnsNumText.SetText($"Turns: {turnsNum}");
                break;
            case 2:
                if (turnsNum % 2 == 0)
                    playerTurnText.SetText("player 1's turn");
                else
                    playerTurnText.SetText("player 2's turn");
                break;
        }
    }
}
