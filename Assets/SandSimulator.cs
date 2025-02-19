using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class SandSimulator : MonoBehaviour
{

    public int gridWidth = 256;
    public int gridHeight = 256;

    public Texture2D texture;

    public Color airColor = new Color(1f, 1f, 1f, 1f);
    public Color sandColor = new Color(0.9f, 0.8f, 0.5f, 1f);
    public Color waterColor = new Color(0.2f, 0.5f, 0.9f, 0.5f);
    public Color brickColor = new Color(0.5f, 0.2f, 0.2f, 1f);

    private const int air = 0;
    private const int sand = 1;
    private const int water = 2;
    private const int brick = 3;

    public int currParticleType = sand;

    public int DrawSize = 3;

    private GridManager gridManager;

    void Start()
    {
        Application.targetFrameRate = 60;
        gridManager = new GridManager(gridWidth, gridHeight, airColor);
        InitializeTexture();
        SetCamera();
    }

    // Update is called once per frame
    void Update()
    {
        handleKeyboard();
        HandleClick();
        FallAllParticles();
        UpdateTexture();
    }

    private void InitializeTexture()
    {
        texture = new Texture2D(gridWidth, gridHeight);
        texture.filterMode = FilterMode.Point;
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, gridWidth, gridHeight), new Vector2(0.5f, 0.5f), 1f);
    }

    private void InitializeGrid()
    {
        // airColor = new Color(1f, 1f, 1f, 0);
        // sandColor = new Color(0.9f, 0.8f, 0.5f, 1);

        gridManager.grid = new Particle[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                gridManager.grid[x, y] = new Particle(air, airColor);
            }
        }

        // GetComponent<SpriteRenderer>().transform.position = new Vector3(gridWidth / 2f, gridHeight / 2f, 0);
    }

    private void SetCamera()
    {
        float zoomFactor = Mathf.Max(gridWidth, gridHeight) / (1 * 2);
        // zoomFactor = 0.5f;
        Camera.main.orthographicSize = zoomFactor;
        // Camera.main.transform.position = new Vector3(gridWidth / 2f, gridHeight / 2f, -10);
        Camera.main.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1);
        // Camera.main.aspect = (float)gridWidth / gridHeight;
    }

    private void HandleClick()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int mouseX = Mathf.RoundToInt(mousePos.x + gridWidth / 2);
            int mouseY = Mathf.RoundToInt(mousePos.y + gridHeight / 2);
            Debug.Log($"X: {mouseX}, Y: {mouseY}");

            int radius = Mathf.RoundToInt(math.floor(DrawSize / 2));
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (gridManager.IsInBounds(mouseX + x, mouseY + y) && UnityEngine.Random.Range(0f, 1f) <= 1f)
                    {
                        int particleType = Input.GetMouseButton(0) ? currParticleType : air;
                        SpawnParticle(mouseX + x, mouseY + y, particleType);
                    }
                }
            }
        }
    }

    private void handleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            InitializeGrid();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            currParticleType = sand;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            currParticleType = water;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            currParticleType = brick;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            DrawSize = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            DrawSize = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            DrawSize = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            DrawSize = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            DrawSize = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            DrawSize = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            DrawSize = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            DrawSize = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            DrawSize = 9;
        }
    }
    private void SpawnParticle(int x, int y, int particle)
    {
        gridManager.grid[x, y].type = particle;
        gridManager.grid[x, y].velocity = Vector2.zero;
        float H, S, V;
        Color randColor;
        switch (particle)
        {
            case air:
                gridManager.grid[x, y].color = airColor;
                break;

            case sand:

                Color.RGBToHSV(sandColor, out H, out S, out V);
                V -= UnityEngine.Random.Range(0f, 0.2f);
                randColor = Color.HSVToRGB(H, S, V);
                gridManager.grid[x, y].color = randColor;
                break;

            case water:
                Color.RGBToHSV(waterColor, out H, out S, out V);
                V -= UnityEngine.Random.Range(0f, 0.2f);
                randColor = Color.HSVToRGB(H, S, V);
                gridManager.grid[x, y].color = randColor;
                break;

            case brick:
                Color.RGBToHSV(brickColor, out H, out S, out V);
                V -= UnityEngine.Random.Range(0f, 0.2f);
                randColor = Color.HSVToRGB(H, S, V);
                gridManager.grid[x, y].color = randColor;
                break;

            default:
                break;
        }
    }

    private void shuffleList(List<Vector2Int> listToShuffle)
    {
        for (int i = 0; i < listToShuffle.Count; i++)
        {
            Vector2Int temp = listToShuffle[i];
            int randomIndex = UnityEngine.Random.Range(i, listToShuffle.Count);
            listToShuffle[i] = listToShuffle[randomIndex];
            listToShuffle[randomIndex] = temp;
        }
    }

    private void FallAllParticles()
    {
        List<Vector2Int> particlePositions = new List<Vector2Int>();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (gridManager.grid[x, y].type != air)
                {
                    particlePositions.Add(new Vector2Int(x, y));
                }
            }
        }

        shuffleList(particlePositions);

        foreach (Vector2Int pos in particlePositions)
        {
            int x = pos.x;
            int y = pos.y;

            Particle particle = gridManager.grid[x, y];

            switch (particle.type)
            {
                case sand:
                    UpdateParticleVelocity(particle, new Vector2(0, -1));
                    MoveParticle(x, y, particle);
                    break;

                case water:
                    UpdateParticleVelocity(particle, new Vector2(0, -1));
                    MoveWaterParticle(x, y, particle);
                    break;
            }
        }
    }

    private void UpdateParticleVelocity(Particle particle, Vector2 gravity)
    {
        particle.velocity += gravity * 1f;
        particle.velocity = Vector2.ClampMagnitude(particle.velocity, 10f); // Limit the maximum velocity
        // Debug.Log($"Updated Velocity: {particle.velocity}");
    }

    private void MoveParticle(int x, int y, Particle particle)
    {

        // Compute the intended final position based on velocity
        Vector2 finalPosF = new Vector2(x, y) + particle.velocity;
        int targetX = Mathf.RoundToInt(finalPosF.x);
        int targetY = Mathf.RoundToInt(finalPosF.y);

        // Determine how many steps to check based on the greater difference
        int steps = Mathf.Max(Mathf.Abs(targetX - x), Mathf.Abs(targetY - y));

        // Start at current position
        Vector2Int lastValidPos = new Vector2Int(x, y);

        // Check each intermediate step along the straight line path
        for (int i = 1; i <= steps; i++)
        {
            int checkX = x + Mathf.RoundToInt((targetX - x) * (i / (float)steps));
            int checkY = y + Mathf.RoundToInt((targetY - y) * (i / (float)steps));

            if (gridManager.CanMoveTo(checkX, checkY, particle.type))
            {
                lastValidPos = new Vector2Int(checkX, checkY);
            }
            else
            {
                // Stop once we hit an obstacle
                break;
            }
        }

        // Only swap if we moved at least one cell
        if (lastValidPos.x != x || lastValidPos.y != y)
        {
            //Try moving based on velocity first
            if (gridManager.CanMoveTo(lastValidPos.x, lastValidPos.y, particle.type))
            {
                gridManager.SwapParticles(x, y, lastValidPos.x, lastValidPos.y);
            }
        }
        else
        {
            if (gridManager.CanMoveTo(x, y - 1, particle.type))
            {
                gridManager.SwapParticles(x, y, x, y - 1);
            }
            else
            {
                int randDir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
                if (gridManager.CanMoveTo(x + randDir, y - 1, particle.type))
                {
                    gridManager.SwapParticles(x, y, x + randDir, y - 1);
                }
                else
                {
                    // If no movement is possible, reset velocity
                    particle.velocity = Vector2.zero;
                }
            }
        }
        // }
    }

    private void MoveWaterParticle(int x, int y, Particle particle)
    {
        // Compute the intended final position based on velocity
        Vector2 finalPosF = new Vector2(x, y) + particle.velocity;
        int targetX = Mathf.RoundToInt(finalPosF.x);
        int targetY = Mathf.RoundToInt(finalPosF.y);

        // Determine how many steps to check based on the greater difference
        int steps = Mathf.Max(Mathf.Abs(targetX - x), Mathf.Abs(targetY - y));

        // Start at current position
        Vector2Int lastValidPos = new Vector2Int(x, y);

        // Check each intermediate step along the straight line path
        for (int i = 1; i <= steps; i++)
        {
            int checkX = x + Mathf.RoundToInt((targetX - x) * (i / (float)steps));
            int checkY = y + Mathf.RoundToInt((targetY - y) * (i / (float)steps));

            if (gridManager.CanMoveTo(checkX, checkY, particle.type))
            {
                lastValidPos = new Vector2Int(checkX, checkY);
            }
            else
            {
                // Stop once we hit an obstacle
                break;
            }
        }

        // Only swap if we moved at least one cell
        if (lastValidPos.x != x || lastValidPos.y != y)
        {
            // Try moving based on the intermediate steps result first
            if (gridManager.CanMoveTo(lastValidPos.x, lastValidPos.y, particle.type))
            {
                gridManager.SwapParticles(x, y, lastValidPos.x, lastValidPos.y);
                return;
            }
        }

        // Fallback: if direct below is available, move down
        if (gridManager.CanMoveTo(x, y - 1, particle.type))
        {
            gridManager.SwapParticles(x, y, x, y - 1);
            return;
        }

        // Fallback water: try moving left or right directly
        int randDir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        if (gridManager.CanMoveTo(x + randDir, y, particle.type))
        {
            gridManager.SwapParticles(x, y, x + randDir, y);
        }
        else if (gridManager.CanMoveTo(x - randDir, y, particle.type))
        {
            gridManager.SwapParticles(x, y, x - randDir, y);
        }
        else
        {
            // No movement possible: reset velocity with new random values
            particle.velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.5f, 1.5f));
        }
    }

    private void UpdateTexture()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                texture.SetPixel(x, y, gridManager.grid[x, y].color);
            }
        }
        texture.Apply();
    }
}
