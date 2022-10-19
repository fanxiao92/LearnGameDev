namespace GameApp
{
    internal class CircleComponent : Component
    {
        public float Raidus { get; set; }

        public CircleComponent(Actor owner) : base(owner)
        {
        }

        public Vector2D Center => Owner.Position;

        public bool Intersect(CircleComponent other)
        {
            Vector2D diff = Center - other.Center;
            float distSq = diff.LengthSq;

            float radiusSq = Raidus + other.Raidus;
            radiusSq *= radiusSq;

            return distSq <= radiusSq;
        }
    }
}
