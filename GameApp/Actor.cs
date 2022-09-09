using System.Diagnostics;

namespace GameApp;

public class Actor
{
    protected enum State
    {
        Active,
        Paused,
        Dead,
    }

    protected State _state = State.Active;

    public Vector2D Position { get; set; }

    public Vector2D Forward { get; set; }

    public float Scale { get; init; } = 1.0f;

    public float Rotation { get; set; }

    private readonly List<Component> _components = new();

    public readonly Game Game;

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
        if (_state != State.Active)
        {
            return;
        }

        UpdateComponents(deltaTime);
        UpdateActor(deltaTime);
    }

    public bool IsDead => _state == State.Dead;

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
}
