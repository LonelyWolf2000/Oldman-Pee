using UnityEngine;

public interface IEnemy
{
    void Spawn(Vector2 position);
    void Move(Vector2 direction);
}
