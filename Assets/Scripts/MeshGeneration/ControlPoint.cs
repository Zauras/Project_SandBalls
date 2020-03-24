using UnityEngine;

namespace MeshGeneration
{
    public class ControlPoint: Point
    {
        public float surface;
        public bool active => surface >= 0.7f;

        public readonly Point current;
        public readonly Point above;

        public ControlPoint(Vector3 position, float surface, float row, float column, float squareSize)
                : base(position)
        {
            this.surface = surface;
            float fractionLerpPathCurrent = (0.7f - surface) / (row - surface);
            float fractionLerpPathAbove = (0.7f - surface) / (column - surface);

            current = new Point(Vector3.Lerp(
                                        this.position,
                                        this.position + Vector3.right * squareSize,
                                        fractionLerpPathCurrent));
            
            above = new Point(Vector3.Lerp(
                                        this.position,
                                        this.position + Vector3.up * squareSize,
                                        fractionLerpPathAbove));
            
        }
    }
}