using System;
using System.Collections.Generic;
using App.Scripts.Libs.CustomLogger;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.PathFinder
{
    public class ChessPathFinder : IChessPathFinder
    {
        private ChessGrid _currentGrid;
        private readonly int[] _knightXMove = {  2, 2, -2, -2, 1,  1, -1, -1 };
        private readonly int[] _knightYMove = { -1, 1,  1, -1, 2, -2,  2, -2 };
        private readonly int[] _kingXMove = { -1, 1, -1,  1,  0, 0, 1, -1 };
        private readonly int[] _kingYMove = { -1, 1,  1, -1, -1, 1, 0,  0 };
        private readonly int[] _queenXMove = { -1, 1, -1,  1,  0, 0, 1, -1 };
        private readonly int[] _queenYMove = { -1, 1,  1, -1, -1, 1, 0,  0 };
        private readonly int[] _bishopXMove = { -1, 1, -1,  1  };
        private readonly int[] _bishopYMove = { -1, 1,  1, -1, };
        private readonly int[] _rookYMove = { -1, 1, 0,  0 };
        private readonly int[] _rookXMove = {  0, 0, 1, -1 };
        private readonly int[] _ponXMove = { 0,  0  };
        private readonly int[] _ponYMove = { 1, -1 };

        public List<Vector2Int> FindPath(ChessUnitType chessUnitType, Vector2Int startPoint, Vector2Int endPoint, ChessGrid grid)
        {
            _currentGrid = grid;
            Node endPathNode = new(Vector2Int.zero);
            Node startNode = new Node(startPoint);
            Node endNode = new Node(endPoint);
            
            switch (chessUnitType)
            {
                case ChessUnitType.Knight:
                    endPathNode = FindPath(startNode, endNode, grid, SetupNearNodesByXYStep, _knightXMove, _knightYMove);
                    break;
                case ChessUnitType.Bishop:
                    endPathNode = FindPath(startNode, endNode, grid, SetupDirectNearNodes, _bishopXMove, _bishopYMove);
                    break;
                case ChessUnitType.King:
                    endPathNode = FindPath(startNode, endNode, grid, SetupPointedNearNodes, _kingXMove, _kingYMove);
                    break;
                case ChessUnitType.Pon:
                    endPathNode = FindPath(startNode, endNode, grid, SetupPointedNearNodes, _ponXMove, _ponYMove);
                    break;
                case ChessUnitType.Queen:
                    endPathNode = FindPath(startNode, endNode, grid, SetupDirectNearNodes, _queenXMove, _queenYMove);
                    break;
                case ChessUnitType.Rook:
                    endPathNode = FindPath(startNode, endNode, grid, SetupDirectNearNodes, _rookXMove, _rookYMove);
                    break;
            
                default:
                    CustomLogger.LogError<Exception>("For this unit path finder doesn't setuped");
                    break;
            }   
            
            if(endPathNode == null)
                return null;

            List<Vector2Int> movePoints = new List<Vector2Int>();
            SetupMovePoints(movePoints, endPathNode);

            return movePoints;
        }

        private void SetupMovePoints(List<Vector2Int> movePoints, Node node)
        {
            do
            {
                movePoints.AddRange(node.MovePoints);
                node = node.PreviousNode;
            } while (node.PreviousNode != null);

            movePoints.Reverse();
        }

        private bool IsValidPosition(Vector2Int nodePosition, int N) => 
            (nodePosition.x >= 0 && nodePosition.x < N) && (nodePosition.y >= 0 && nodePosition.y < N);
    
        private bool IsValidPosition(int x, int y, int N) => 
            (x >= 0 && x < N) && (y >= 0 && y < N);

        private bool IsAvailablePosition(Vector2Int NodePosition)
        {
            ChessUnit chessUnit = GetChess(NodePosition.x, NodePosition.y);
            return chessUnit == null || (chessUnit.IsAvailable);
        }

        private bool IsAvailablePosition(int x, int y)
        {
            ChessUnit chessUnit = GetChess(x, y);
            return chessUnit == null || (chessUnit.IsAvailable);
        }

        private ChessUnit GetChess(int x, int y)
        {
            ChessUnit chessUnit = _currentGrid.Get(y,x);
            return chessUnit;
        }

        private Node FindPath(Node startPoint, Node endPoint, ChessGrid grid, Action<int, int, int, Node, Queue<Node>> SetupNearPoints, int[] xMove, int[] yMove)
        {
            List<Vector2Int> visitedNodes  = new();
            Queue<Node> nearNodes = new();

            nearNodes.Enqueue(startPoint);

            while (nearNodes.Count != 0)
            {
                Node currentNode = nearNodes.Dequeue();

            
                if (currentNode.Position == endPoint.Position)
                    return currentNode;
            
                if(visitedNodes.Contains(currentNode.Position) || (currentNode.Position != startPoint.Position && !IsAvailablePosition(currentNode.Position)))
                    continue;

                visitedNodes.Add(currentNode.Position);

                for (int i = 0; i < xMove.Length; i++)
                {
                    SetupNearPoints(grid.Size.x, xMove[i], yMove[i], currentNode, nearNodes);
                }
            }

            return null;
        }

        private void SetupNearNodesByXYStep(int gridSize, int xDelta, int yDelta, Node currentNode, Queue<Node> nearNodes)
        {
            Vector2Int newNodePosition = new Vector2Int(currentNode.Position.x + xDelta, currentNode.Position.y + yDelta);
            List<Vector2Int> movePoints = new List<Vector2Int>();

            movePoints.Add(new Vector2Int(currentNode.Position.x, currentNode.Position.y + yDelta));
            movePoints.Add(new Vector2Int(currentNode.Position.x + xDelta, currentNode.Position.y + yDelta));
            movePoints.Reverse();

            if (IsValidPosition(newNodePosition, gridSize))
                nearNodes.Enqueue(new Node(newNodePosition, currentNode, movePoints));
        }
    
        private void SetupPointedNearNodes(int gridSize, int xDelta, int yDelta, Node currentNode, Queue<Node> nearNodes)
        {
            Vector2Int newNodePosition = new Vector2Int(currentNode.Position.x + xDelta, currentNode.Position.y + yDelta);

            if (IsValidPosition(newNodePosition, gridSize))
                nearNodes.Enqueue(new Node(newNodePosition, currentNode));
        }

        private void SetupDirectNearNodes(int gridSize, int xDelta, int yDelta, Node currentNode, Queue<Node> nearNodes)
        {
            int x = currentNode.Position.x + xDelta;
            int y = currentNode.Position.y + yDelta;

            while (IsValidPosition(x,y, gridSize) && IsAvailablePosition(x,y))
            {
                Vector2Int newPoint = new Vector2Int(x, y);
                nearNodes.Enqueue(new Node(newPoint, currentNode));
                x += xDelta;
                y += yDelta;
            }
        }
    }

    public class Node
    {
        public Vector2Int Position;
        public Node PreviousNode;
        public List<Vector2Int> MovePoints;

        public Node(Vector2Int position, Node previousNode = null, List<Vector2Int> movePoints = default)
        {
            Position = position;
            PreviousNode = previousNode;

            if (movePoints == default)
                MovePoints = new() { position };
            else
                MovePoints = movePoints;
        }
    }
}