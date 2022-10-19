using System.Diagnostics;

namespace GameApp
{
    internal class Actor
    {
        internal enum State
        {
            Active,
            Paused,
            Dead,
        }

        public State State_ { get; set; } = State.Active;

        public Vector2D Position { get; set; }

        public Vector2D Forward => new() { X = (float)System.Math.Cos(Rotation), Y = -(float)System.Math.Sin(Rotation) };

        public float Scale { get; init; } = 1.0f;

        public float Rotation { get; set; }

        private readonly List<Component> _components = new();

        public Game Game { get; }

        public Actor(Game game)
        {
            Game = game;

            Game.AddActor(this);
        }

        private void UpdateComponents(float deltaTime)
        {
            foreach (Component component in _components)
            {
                component.Update(deltaTime);
            }
        }

        protected virtual void UpdateActor(float deltaTime)
        {

        }

        public void Update(float deltaTime)
        {
            if (State_ != State.Active)
            {
                return;
            }

            UpdateComponents(deltaTime);
            UpdateActor(deltaTime);
        }

        public bool IsDead => State_ == State.Dead;

        public void AddComponent(Component component)
        {
            int order = component.UpdateOrder;
            int index = 0;
            for (; index < _components.Count; index++)
            {
                if (order < _components[index].UpdateOrder)
                {
                    break;
                }
            }
            _components.Insert(index, component);
        }


        public void Remove()
        {
            Debug.Assert(IsDead);

            foreach (Component component in _components)
            {
                component.Remove();
            }
            _components.Clear();
        }

        public void ProcessInput(byte[] keyState)
        {
            if (State_ != State.Active)
            {
                return;
            }

            foreach (Component comp in _components)
            {
                comp.ProcessInput(keyState);
            }
            ActorInput(keyState);
        }

        protected virtual void ActorInput(byte[] keyState)
        {
        }
    }

}

