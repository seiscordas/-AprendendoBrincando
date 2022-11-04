[System.Serializable]
public class PiecesList
{
    public Piece[] Pieces;
}
[System.Serializable]
public struct Piece
{
    public string theme;
    public string image;
    public string sound;
}