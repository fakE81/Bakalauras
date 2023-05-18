public class ANode
{
    public int X { get; }
    public int Y { get; }
    public int G { get; set; }
    public int H { get; set; }
    public int F => G + H;
    public ANode Parent { get; set; }

    public ANode(int x, int y)
    {
        X = x;
        Y = y;
        G = int.MaxValue;
        H = 0;
        Parent = null;
    }
}
