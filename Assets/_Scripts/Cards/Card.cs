using System;
using UnityEngine;

public class Card
{
    private Tile[,] _tiles;

    public Card(int[,] data, Tile _grassTile, Tile _darkTile)
    {
        _tiles = new Tile[data.GetLength(0), data.GetLength(1)];

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                Tile tile = _grassTile;

                switch (data[x, y])
                {
                    case 0:
                        tile = _darkTile;
                        break;
                    case 1:
                        tile = _grassTile;
                        break;
                }

                _tiles[x,y] = tile;
            }
        }
    }


    public Tile GetCardTile(int x, int y)
    {
        return _tiles[x,y];
    }

    public int GetCardXSize()
    {
        return _tiles.GetLength(0);
    }

    public int GetCardYSize()
    {
        return _tiles.GetLength(1);
    }
}

