using Unity.Mathematics;
using UnityEngine;

public class SandSimulator : MonoBehaviour
{

    public int gridWidth = 100;
    public int gridHeight = 100;

    private int[,] grid;

    public Texture2D texture;

    private int air = 0;
    public Color airColor = Color.black;
    private int sand = 1;
    public Color sandColor = new Color(0.9f, 0.8f, 0.5f, 1);

    void Start()
    {
        InitializeGrid();
        SetCamera();
    }

    // Update is called once per frame
    void Update()
    {

        HandleClick();
        UpdateTexture();
    }

    private void InitializeGrid()
    {
        grid = new int[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = air;
            }
        }
        texture = new Texture2D(gridWidth, gridHeight);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, gridWidth, gridHeight), new Vector2(0.5f, 0.5f), 1f);
    }

    private void SetCamera()
    {
        float zoomFactor = Mathf.Max(gridWidth, gridHeight) / (1 * 2);
        // zoomFactor = 0.5f;
        Camera.main.orthographicSize = zoomFactor;
    }

    private void HandleClick()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePos.x);
            int y = Mathf.RoundToInt(mousePos.y);
            Debug.Log($"X: {x}, Y: {y}");
        }
    }

    private void UpdateTexture()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] == air)
                {
                    texture.SetPixel(x, y, airColor);
                }
                else if (grid[x, y] == sand)
                {
                    texture.SetPixel(x, y, sandColor);
                }
            }
        }
    }
}
