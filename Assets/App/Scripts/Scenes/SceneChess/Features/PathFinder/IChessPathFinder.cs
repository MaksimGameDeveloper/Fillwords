
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.PathFinder
{
    public interface IChessPathFinder
    {
        List<Vector2Int> FindPath(ChessUnitType chessUnitType, Vector2Int startPoint, Vector2Int endPoint, ChessGrid grid);
    }
}