
namespace MeshGeneration
{
    public class Square
    {
        public ControlPoint topLeft;
        public ControlPoint topRight;
        public ControlPoint bottomRight;
        public ControlPoint bottomLeft;
        public Point centreTop;
        public Point centreRight;
        public Point centreBottom;
        public Point centreLeft;
        
        public int configuration;

        public Square(ControlPoint topLeft,ControlPoint topRight,
                      ControlPoint bottomRight, ControlPoint bottomLeft)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomRight = bottomRight;
            this.bottomLeft = bottomLeft;
            centreTop = topLeft.current;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.current;
            centreLeft = bottomLeft.above;

            if (topLeft.active)
            {
                configuration += 8;
            }

            if (topRight.active)
            {
                configuration += 4;
            }

            if (bottomRight.active)
            {
                configuration += 2;
            }

            if (bottomLeft.active)
            {
                configuration++;
            }
        }
    }
}