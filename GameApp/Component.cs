namespace GameApp
{
    internal abstract class Component
    {
        protected Actor Owner { get; }

        public int UpdateOrder { get; }

        protected Component(Actor owner, int updateOrder = 100)
        {
            Owner = owner;
            UpdateOrder = updateOrder;

            owner.AddComponent(this);
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void Remove()
        {
        }

        public virtual void ProcessInput(byte[] keyState)
        {
        }
    }
}

