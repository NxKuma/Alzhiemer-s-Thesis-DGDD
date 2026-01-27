using UnityEngine;

[CreateAssetMenu(fileName = "PuzzlePiece", menuName = "ScriptableObjects/Item/PuzzlePiece", order = 1)]
public class PuzzlePiece : Item
{
    [Header("Puzzle Adjustment Data")]
    [SerializeField] private Vector2 _puzzleDimensions;
    [SerializeField] private Vector2 _puzzlePosition;
    
    [Header("Puzzle Mesh Data")]
    [SerializeField] private Material _puzzleShaderMaterial;
    [SerializeField] private Sprite _puzzleMeshTexture;
   
   //Getters
    public Material GetPuzzleShaderMaterial() => _puzzleShaderMaterial;
    public Sprite GetPuzzleMeshTexture() => _puzzleMeshTexture;
    public float GetPuzzleWidth() => _puzzleDimensions.x;
    public float GetPuzzleHeight() => _puzzleDimensions.y;
    public float GetPuzzleX() => _puzzlePosition.x;
    public float GetPuzzleY() => _puzzlePosition.y;

}
