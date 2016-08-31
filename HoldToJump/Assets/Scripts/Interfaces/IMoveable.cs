using UnityEngine;
using System.Collections;

public interface IMoveable
{
    /// <summary>
    /// Moves horizontally.
    /// </summary>
    void MoveHorizontally(int moveSpeed);

    /// <summary>
    /// Moves vertically.
    /// </summary>
    void MoveVertically(int moveSpeed);

    
}
