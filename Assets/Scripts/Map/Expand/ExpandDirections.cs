public class ExpandDirections
{
    public bool right;
    public bool left;
    public bool up;
    public bool down;

    public ExpandDirections()
    {
    }

    public ExpandDirections(bool right, bool left, bool up, bool down)
    {
        this.right = right;
        this.left = left;
        this.up = up;
        this.down = down;
    }

    public void setDirections(bool right, bool left, bool up, bool down)
    {
        this.right = right;
        this.left = left;
        this.up = up;
        this.down = down;
    }

    public bool getRight()
    {
        return right;
    }

    public bool getLeft()
    {
        return left;
    }

    public bool getUp()
    {
        return up;
    }

    public bool getDown()
    {
        return down;
    }
}