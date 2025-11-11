using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Elements")]
    [Range(2, 8)][SerializeField] private int puzzleDifficulty = 4;
    [SerializeField] private Transform puzzleHolder;
    [SerializeField] private Transform piecePrefab;

    [Header("UI Elements")] //can prolly remove
    [SerializeField] private List<Texture2D> spriteTextures;
    [SerializeField] private Transform selectPanel;
    [SerializeField] private Image selectPrefab;

    private List<Transform> pieces;
    private Vector2Int dimensions;
    private float width, height;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Texture2D tex in spriteTextures) 
        {
            Image img = Instantiate(selectPrefab, selectPanel);
            img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

            img.GetComponent<Button>().onClick.AddListener(delegate{StartPuzzle(tex);});
        }
    }

    public void StartPuzzle(Texture2D tex)
    {
        selectPanel.gameObject.SetActive(false); //removes start menu ui (not for final product)

        pieces = new List<Transform>();

        // Calculate the size of each piece
        dimensions = GetDimensions(tex, puzzleDifficulty);

        CreatePieces(tex);
    }

    Vector2Int GetDimensions(Texture2D tex, int diff)
    {
        Vector2Int dimensions = Vector2Int.zero;
        if (tex.width < tex.height)
        {
            dimensions.x = diff;
            dimensions.y = (diff * tex.height) / tex.width;
        }
        else
        {
            dimensions.x = (diff * tex.width) / tex.height;
            dimensions.y = diff;
        }
        return dimensions;
    }

    void CreatePieces(Texture2D tex)
    {
        height = 1f / dimensions.y;
        float aspectRatio = (float)tex.width / tex.height;
        width = aspectRatio / dimensions.x; 

        for (int row = 0; row < dimensions.y; row++)
        {
            for (int col = 0; col < dimensions.x; col++)
            {
                Transform piece = Instantiate(piecePrefab, puzzleHolder);
                piece.localPosition = new Vector3(
                    (-width * dimensions.x / 2) + (width * col) + (width / 2),
                    (-height * dimensions.y / 2) + (height * row) + (height / 2),
                    -1);
                piece.localScale = new Vector3(width, height, 1);
                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
