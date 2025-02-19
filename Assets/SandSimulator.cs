using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class SandSimulator : MonoBehaviour
{

    public int gridWidth = 256;
    public int gridHeight = 256;

    public Texture2D texture;

    private const int air = 0;
    public Color airColor = new Color(1f, 1f, 1f, 1f);
    private const int sand = 1;
    public Color sandColor = new Color(0.9f, 0.8f, 0.5f, 1f);
    private const int water = 2;
    public Color waterColor = new Color(0.2f, 0.5f, 0.9f, 0.5f);

    private const int brick = 3;
    public Color brickColor = new Color(0.5f, 0.2f, 0.2f, 1f);

    public int currParticleType = sand;

    public int DrawSize = 3;

    public class Particle
    {
        public int type;
        public Vector2 velocity;
        public Color color;

        public Particle(int type, Color color)
        {
            this.type = type;
            this.velocity = Vector2.zero;
            this.color = color;
        }
    }

    private Particle[,] grid;



    void Start()
    {
        Application.targetFrameRate = 60;
        InitializeGrid();
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

    private void InitializeGrid()
    {
        // airColor = new Color(1f, 1f, 1f, 0);
        // sandColor = new Color(0.9f, 0.8f, 0.5f, 1);

        grid = new Particle[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new Particle(air, airColor);
            }
        }
        texture = new Texture2D(gridWidth, gridHeight);
        texture.filterMode = FilterMode.Point;
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, gridWidth, gridHeight), new Vector2(0.5f, 0.5f), 1f);
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
                    if (IsInBounds(mouseX + x, mouseY + y) && UnityEngine.Random.Range(0f, 1f) < 0.75f)
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

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    private void SpawnParticle(int x, int y, int particle)
    {
        grid[x, y].type = particle;
        float H, S, V;
        Color randColor;
        switch (particle)
        {
            case air:
                grid[x, y].color = airColor;
                break;

            case sand:

                Color.RGBToHSV(sandColor, out H, out S, out V);
                V -= UnityEngine.Random.Range(0f, 0.2f);
                randColor = Color.HSVToRGB(H, S, V);
                grid[x, y].color = randColor;
                break;

            case water:
                grid[x, y].color = waterColor;
                break;

            case brick:
                Color.RGBToHSV(brickColor, out H, out S, out V);
                V -= UnityEngine.Random.Range(0f, 0.2f);
                randColor = Color.HSVToRGB(H, S, V);
                grid[x, y].color = randColor;
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
                if (grid[x, y].type != air)
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

            switch (grid[x, y].type)
            {
                case sand:
                    if (CanMoveTo(x, y - 1))
                    {
                        SwapParticles(x, y, x, y - 1);
                    }
                    else
                    {
                        int randSandDirection = UnityEngine.Random.Range(0, 2);
                        if (randSandDirection == 0)
                        {
                            randSandDirection = -1;
                        }
                        if (CanMoveTo(x + randSandDirection, y - 1))
                        {
                            SwapParticles(x, y, x + randSandDirection, y - 1);
                        }
                        else if (CanMoveTo(x - randSandDirection, y - 1))
                        {
                            SwapParticles(x, y, x - randSandDirection, y - 1);
                        }
                    }
                    break;

                case water:
                    int randWaterDirection = UnityEngine.Random.Range(0, 2);
                    if (randWaterDirection == 0)
                    {
                        randWaterDirection = -1;
                    }
                    if (CanMoveTo(x, y - 1))
                    {
                        SwapParticles(x, y, x, y - 1);
                    }
                    else if (CanMoveTo(x + randWaterDirection, y))
                    {
                        SwapParticles(x, y, x + randWaterDirection, y);
                    }
                    else if (CanMoveTo(x - randWaterDirection, y))
                    {
                        SwapParticles(x, y, x - randWaterDirection, y);
                    }
                    break;

                default:
                    break;
            }
        }
    }

    private bool CanMoveTo(int x, int y)
    {
        return IsInBounds(x, y) && grid[x, y].type == air;
    }

    private void SwapParticles(int x1, int y1, int x2, int y2)
    {
        Particle temp = grid[x1, y1];
        grid[x1, y1] = grid[x2, y2];
        grid[x2, y2] = temp;
    }

    private void UpdateTexture()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                texture.SetPixel(x, y, grid[x, y].color);
            }
        }
        texture.Apply();
    }
}
