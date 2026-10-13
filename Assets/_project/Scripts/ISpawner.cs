using System;

public interface ISpawner
{
    public int CountSpawnObjects { get; protected set; }
    public int CountCreateObjects { get; protected set; }
    public int CountActiveObjects  { get;}
    public event Action CountChanged;
}
