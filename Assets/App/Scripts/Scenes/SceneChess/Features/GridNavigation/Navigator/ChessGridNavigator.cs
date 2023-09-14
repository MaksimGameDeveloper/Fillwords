using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using App.Scripts.Scenes.SceneChess.Features.PathFinder;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        private readonly IChessPathFinder _chessPathFinder;

        public ChessGridNavigator(IChessPathFinder chessPathFinder)
        {
            _chessPathFinder = chessPathFinder;
        }
        
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            var path = _chessPathFinder.FindPath(unit, from, to, grid);
            return path;
        }
    }
}