using System;
using UnityEngine;
using UnityEngine.Events;

public class GameTile : MonoBehaviour
{

    [SerializeField] GameObject unflippedTile;
    [SerializeField] GameObject tileBG;
    [SerializeField] GameObject innerShape;
    public Color bgColor;
    public Color shapeColor;
    bool flipped;
    public int id;
    public event Action<GameObject, int> TileFlipped;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flipped = false;
        unflippedTile = Instantiate(unflippedTile, transform);
        unflippedTile.SetActive(true);
        SpriteRenderer tileBGSpriteRenderer = tileBG.GetComponent<SpriteRenderer>();
        SpriteRenderer innerShapeSpriteRenderer = innerShape.GetComponent<SpriteRenderer>();
        tileBGSpriteRenderer.color = bgColor;
        innerShapeSpriteRenderer.color = shapeColor;
        tileBG.SetActive(false);
        innerShape.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (!flipped)
        {
            TileFlipped?.Invoke(gameObject, id);
        }
    }

    public void FlipTileToFront()
    {
        unflippedTile.SetActive(false);
        tileBG.SetActive(true);
        innerShape.SetActive(true);
        flipped = true;
    }

    public void FlipTileToBack()
    {
        unflippedTile.SetActive(true);
        tileBG.SetActive(false);
        innerShape.SetActive(false);
        flipped = false;
    }
}
