using UnityEngine;
using System.Collections;

public class BasePlatformTypeComponent : LevelItemController, IMoveable , IObstacle
{
    Vector2 normalVerticalPlatformSpawnPos = new Vector2(14, -3.45f);

    public enum PlatformType
    {
        NormalVertical,
        DoubleVertical,
        Floating,
    }

    public PlatformType Type;

    void OnEnable()
    {
        // Reset position
        transform.ResetTransform(resetPosition: true, resetRotation: true, resetScale: false);

        switch (Type)
        {
            case PlatformType.NormalVertical:
                gameObject.transform.position = normalVerticalPlatformSpawnPos;
                break;
            case PlatformType.DoubleVertical:
                break;
            case PlatformType.Floating:
                var randomPositionY = Random.Range(-2.5f, 2.5f);
                gameObject.transform.position = new Vector2(14, randomPositionY);
                break;
            default:
                break;
        }
        
    }

    void Update()
    {
        switch (Type)
        {
            case PlatformType.NormalVertical:                
                MoveHorizontally(NormalVerticalPlatformSpeed);
                break;
            case PlatformType.DoubleVertical:                
                break;
            case PlatformType.Floating:
                MoveHorizontally(FloatingPlatformSpeed);
                break;
            default:
                break;
        }
    }


    #region IMoveable Implementation

    public void MoveHorizontally(int moveSpeed)
    {
        transform.position = new Vector2(transform.position.x - Time.deltaTime * moveSpeed, transform.position.y);
    }

    public void MoveVertically(int moveSpeed)
    {       
    }

    #endregion

    // TODO : make component?
    #region Collisions   
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlatformCatcher")
        {
            gameObject.SetActive(false);
        }
    }    
    #endregion
}
