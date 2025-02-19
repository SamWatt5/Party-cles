using NUnit.Framework;
using UnityEngine;

public class SandSimulator : MonoBehaviour
{

    public int gridWidth = 256;
    public int gridHeight = 256;

    public Texture2D texture;

    private const int air = 0;
    public Color airColor = new Color(1f, 1f, 1f, 1f);
    private const int sand = 1;
    public Color sandColor = new Color(0.9f, 0.8f, 0.5f, 1);
    private const int water = 2;
    public Color waterColor = new Color(0.2f, 0.5f, 0.9f, 0.5f);

    public class Particle
    {
        public int type;
        public float velocity;
        public Color color;

        public Particle(int type, Color color)
        {
            this.type = type;
            this.velocity = 0;
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

        HandleClick();
        FallParticles();
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
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePos.x + gridWidth / 2);
            int y = Mathf.RoundToInt(mousePos.y + gridHeight / 2);
            Debug.Log($"X: {x}, Y: {y}");
            if (IsInBounds(x, y))
            {

                SpawnParticle(x, y, sand);
            }

        }

        if (Input.GetMouseButton(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePos.x + gridWidth / 2);
            int y = Mathf.RoundToInt(mousePos.y + gridHeight / 2);
            Debug.Log($"X: {x}, Y: {y}");
            if (IsInBounds(x, y))
            {

                SpawnParticle(x, y, water);
            }

        }
    }



    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    private void SpawnParticle(int x, int y, int particle)
    {
        Debug.Log($"Spawning particle at ({x}, {y})");
        grid[x, y].type = particle;
        switch (particle)
        {
            case air:
                grid[x, y].color = airColor;
                break;

            case sand:
                float H, S, V;
                Color.RGBToHSV(sandColor, out H, out S, out V);
                V -= Random.Range(0, 0.2f);
                Color randColor = Color.HSVToRGB(H, S, V);
                grid[x, y].color = randColor;
                break;

            case water:
                grid[x, y].color = waterColor;
                break;

            default:
                break;
        }
    }

    private void FallParticles()
    {
        for (int y = 1; y < gridHeight; y++)
        {
            for (int x = gridWidth - 1; x >= 0; x--)
            {
                // Check directly below
                if (CanMoveTo(x, y - 1))
                {
                    SwapParticles(x, y, x, y - 1);
                }
                else
                {
                    int randDirection = Random.Range(0, 2);
                    if (randDirection == 0)
                    {
                        randDirection = -1;
                    }
                    if (CanMoveTo(x + randDirection, y - 1))
                    {
                        SwapParticles(x, y, x + randDirection, y - 1);
                    }
                    else if (CanMoveTo(x - randDirection, y - 1))
                    {
                        SwapParticles(x, y, x - randDirection, y - 1);
                    }
                }
                if (grid[x, y].type == water)
                {
                    int randDirection = Random.Range(0, 2);
                    if (randDirection == 0)
                    {
                        randDirection = -1;
                    }
                    if (CanMoveTo(x + randDirection, y))
                    {
                        SwapParticles(x, y, x + randDirection, y);
                    }
                    else if (CanMoveTo(x - randDirection, y))
                    {
                        SwapParticles(x, y, x - randDirection, y);
                    }
                }
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
