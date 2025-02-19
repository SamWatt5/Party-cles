using UnityEngine;

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