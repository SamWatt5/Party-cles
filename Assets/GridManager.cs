using System.Security.Cryptography;
using UnityEngine;

public class GridManager
{
    public Particle[,] grid;
    public int gridWidth;
    public int gridHeight;
    public Color airColor;

    public GridManager(int width, int height, Color airColor)
    {
        this.gridWidth = width;
        this.gridHeight = height;
        this.airColor = airColor;
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        grid = new Particle[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new Particle(0, airColor); // 0 = air
            }
        }
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    public bool CanMoveTo(int x, int y, int movingParticleType)
    {
        if (!IsInBounds(x, y))
        {
            return false;
        }
        int targetType = grid[x, y].type;

        if (targetType == 0)
        {
            return true;
        }
        if (targetType == 2 && movingParticleType != 2)
        {
            return true;
        }
        return false;
    }

    public void SwapParticles(int x1, int y1, int x2, int y2)
    {
        Particle temp = grid[x1, y1];
        grid[x1, y1] = grid[x2, y2];
        grid[x2, y2] = temp;
    }
}
