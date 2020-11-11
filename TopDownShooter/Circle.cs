// TopDownShooter.Circle
using Microsoft.Xna.Framework;

public struct Circle
{
    private Vector2 v;

    private Vector2 direction;

    private float distanceSquared;

    public Vector2 Center;

    public float Radius;

    public Circle(Vector2 position, float radius)
    {
        distanceSquared = 0f;
        direction = Vector2.Zero;
        v = Vector2.Zero;
        Center = position;
        Radius = radius;
    }

    public bool Intersects(Rectangle rectangle)
    {
        v = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right), MathHelper.Clamp(Center.Y, rectangle.Top, rectangle.Bottom));
        direction = Center - v;
        distanceSquared = direction.LengthSquared();
        return distanceSquared > 0f && distanceSquared < Radius * Radius;
    }
}
