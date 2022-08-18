using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _grassTile, _mountainTile, _darkTile;

    [SerializeField] private Transform _cam;

    private Card _card;

    private Dictionary<Vector2, Tile> _tiles;

    private void Awake()
    {
        Instance = this;
    }


    public void GenerateGrid()
    {
        int[,] data = {
                        { 0,1,1,0 },
                        { 0,1,1,0 },
                        { 0,1,1,0 },
                        { 0,1,1,0 },
                        { 0,1,1,0 },
                        { 1,1,1,1 }
                      };

        string x = $".FF." +
                   $".FF." +
                   $".FF." +
                   $".FF." +
                   $".FF." +
                   $"FFFF";

        data = ArrayExtensions<int>.Transpose(data);
        data = ArrayExtensions<int>.Reverse2DimArray(data);

        _tiles = new Dictionary<Vector2, Tile>();

        _card = new Card(data, _grassTile, _darkTile);

        int initialXPos = 6;
        int initialYPos = 0;


        for (int x = 0; x < _card.GetCardXSize(); x++)
        {
            for (int y = 0; y < _card.GetCardYSize(); y++)
            {
                //Area de spawning



                var spawnedTile = Instantiate(_card.GetCardTile(x,y), new Vector3(initialXPos + x, initialYPos + y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(x, y);

                _tiles[new Vector2(initialXPos + x, initialYPos + y)] = spawnedTile;
            }
        }



        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        //GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public class ArrayExtensions<T>
    {
        public static T[,] Transpose(T[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            var result = new T[columns, rows];

            for (var c = 0; c < columns; c++)
            {
                for (var r = 0; r < rows; r++)
                {
                    result[c, r] = matrix[r, c];
                }
            }
            return result;
        }

        public static T[,] Reverse2DimArray(T[,] matrix)
        {
            for (int r = 0; r <= (matrix.GetUpperBound(0)); r++)
            {
                for (int c = 0; c <= (matrix.GetUpperBound(1) / 2); c++)
                {
                    T tempHolder = matrix[r, c];
                    matrix[r, c] = matrix[r, matrix.GetUpperBound(1) - c];
                    matrix[r, matrix.GetUpperBound(1) - c] = tempHolder;
                }
            }

            return matrix;
        }
    }

    public Tile GetHeroSpawnTile()
    {
        //Left side AND Available : (t => t.Key.x < _width / 2 && t.Value.Walkable)
        return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => UnityEngine.Random.value).First().Value;
    }

    public Tile GetEnemySpawnTile()
    {
        //Right side AND Available : (t => t.Key.x > _width / 2 && t.Value.Walkable)
        return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => UnityEngine.Random.value).First().Value;
    }


    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}