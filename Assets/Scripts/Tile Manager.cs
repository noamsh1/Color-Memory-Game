using System;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    int rowNum;
    int columnNum;
    public GameTile[,] gameTiles { get; private set; }
    public event Action GameTileGridSetUp;
    Vector2 startingCords;
    float gapX = 1.5f;
    float gapY = 1.25f;
    static Color purple = new Color(155, 0, 255);
    static Color cyan = new Color(0, 255, 255);
    [SerializeField] GameObject gameTile;
    Color[] colorArray = { Color.black, Color.white, Color.red, Color.blue, Color.yellow, Color.green, purple, cyan };
    HashSet<TileCombination> tileCombinationSet = new HashSet<TileCombination>();

    struct TileCombination
    {
        Color bgColor;
        Color shapeColor;

        public TileCombination(Color bgClr, Color shapeClr)
        {
            bgColor = bgClr;
            shapeColor = shapeClr;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int tileNum = GameSettings.GetInstance().tileNum;
        switch (tileNum)
        {
            case 24:
                rowNum = 4;
                columnNum = 6;
                startingCords = new Vector2(-3.75f, 4);
                break;
            case 36:
                rowNum = 6;
                columnNum = 6;
                startingCords = new Vector2(-3.75f, 4);
                break;
            case 48:
                rowNum = 6;
                columnNum = 8;
                startingCords = new Vector2(-5.25f, 4);
                break;
            default:
                rowNum = 4;
                columnNum = 6;
                startingCords = new Vector2(-3.75f, 4);
                break;
        }
        gameTiles = new GameTile[rowNum, columnNum];
        int i = 0;
        while (i < rowNum)
        {
            int j = 0;
            while (j < columnNum)
            {
                if (gameTiles[i, j] == null)
                {
                    CreateMatchingTiles(i, j);
                }
                j++;
            }
            i++;
        }
        GameTileGridSetUp?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGridSize(int tileNum)
    {
        
    }

    void CreateMatchingTiles(int i, int j)
    {
        Vector2 tilePos1 = new Vector2(startingCords.x + (j * gapX), startingCords.y - (i * gapY));
        Color bgColor;
        Color shapeColor;
        do
        {
            bgColor = colorArray[UnityEngine.Random.Range(0, colorArray.Length)];
            do
            {
                shapeColor = colorArray[UnityEngine.Random.Range(0, colorArray.Length)];
            }
            while (shapeColor.Equals(bgColor));
        }
        while (tileCombinationSet.Contains(new TileCombination(bgColor, shapeColor)));
        tileCombinationSet.Add(new TileCombination(bgColor, shapeColor));

        GameTile tileScript1 = InstantiateTile(tilePos1, bgColor, shapeColor);
        int k, m;
        do
        {
            k = UnityEngine.Random.Range(i, rowNum - 1);
            if (k == i)
                if (j == columnNum - 1) // no space in the row
                    m = 0;
                else
                    m = UnityEngine.Random.Range(j + 1, columnNum);
            else
                m = UnityEngine.Random.Range(0, columnNum);
        }
        while (gameTiles[k, m] != null);
        Vector2 tilePos2 = new Vector2(startingCords.x + (m * gapX), startingCords.y - (k * gapY));
        GameTile tileScript2 = InstantiateTile(tilePos2, bgColor, shapeColor);
        tileScript1.id = i * 10 + j;
        tileScript2.id = tileScript1.id;
        gameTiles[i, j] = tileScript1;
        gameTiles[k, m] = tileScript2;
    }

    GameTile InstantiateTile(Vector2 tilePos, Color bgColor, Color shapeColor)
    {
        GameObject newTile = Instantiate(gameTile, tilePos, new Quaternion(0, 0, 0, 0));
        GameTile newTileScript = newTile.GetComponent<GameTile>();
        newTileScript.bgColor = bgColor;
        newTileScript.shapeColor = shapeColor;
        return newTileScript;
    }


}
