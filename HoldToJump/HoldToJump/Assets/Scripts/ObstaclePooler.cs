using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObstaclePooler : MonoBehaviour
{

    #region Vars
  
    // remove this public
    public int pooledAmmount;

    GameObject obstaclePool;
    string path;

    // remove this public
    public List<GameObject> PooledObjectsList;

    /// <summary>
    /// Floating platform component name.
    /// </summary>
    private const string FloatingPlatformComponentName = "FloatingPlatformComponent";

    private const string BasicPlatformComponentName = "BasicPlatformComponent";
    #endregion

    void Start()
    {
        obstaclePool = GameObject.Find("ObstaclePool");
        PooledObjectsList = new List<GameObject>();
        path = "Environment/Obstacles/";

        //for (int i = 0; i < pooledAmmount; i++)
        //{
        //    GameObject obj = Instantiate(Resources.Load(path + "platformTest01") as GameObject);
        //    obj.SetActive(false);
        //    PooledObjectsList.Add(obj);
        //}
    }

    public GameObject GetPooledObject(BasePlatformTypeComponent.PlatformType platformType)
    {        
        if (PooledObjectsList != null && PooledObjectsList.Count != default(int))
        {
            foreach (var obj in PooledObjectsList)
            {
                bool notNullAndReadyToUse = obj != null && !obj.activeInHierarchy;
                
                if (notNullAndReadyToUse)
                {
                    
                    /// Returns object according to the object type, else iterate through next object.                    
                    switch (platformType)
                    {
                        case BasePlatformTypeComponent.PlatformType.NormalVertical:
                            if (obj.GetComponent<BasePlatformTypeComponent>()!= null && 
                                obj.GetComponent<BasePlatformTypeComponent>().Type == BasePlatformTypeComponent.PlatformType.NormalVertical)
                            {
                                return obj;
                            }
                            else
                            {
                                break;
                            }
                        case BasePlatformTypeComponent.PlatformType.Floating:
                            if (obj.GetComponent<BasePlatformTypeComponent>() != null 
                                && obj.GetComponent<BasePlatformTypeComponent>().Type == BasePlatformTypeComponent.PlatformType.Floating)
                            {
                                return obj;
                            }
                            else
                            {
                                break;
                            }
                        default:
                            return null;
                    }
                }
            }
        }

        // If all objects are being used, need to create more.
        switch (platformType)
        {
            case BasePlatformTypeComponent.PlatformType.NormalVertical:                
                return InstantiateRawObstacle("newBasicPlatform");              
            case BasePlatformTypeComponent.PlatformType.Floating:
                return InstantiateRawObstacle("newFloatingPlatform");
            default:
                return null;
        }
    }

    #region Private Methods
    
    private GameObject InstantiateRawObstacle(string objectName)
    {
        // currently only instantiating basic platforms
        GameObject obj;
        obj = Instantiate(Resources.Load(path + objectName) as GameObject);
        obj.transform.parent = obstaclePool.transform;
        obj.SetActive(false);
        PooledObjectsList.Add(obj);
        return obj;
    }

    /// <summary>
    /// Builds a new platform with 1 or 2 components.
    /// </summary>
    /// <param name="platform">The raw gameObject.</param>
    /// <param name="platformComponent">PlatformComponent, a platform type.</param>
    /// <param name="extraComponent">ExtraComponent, bonus or other component type.</param>
    /// <returns>The platform.</returns>
    private GameObject BuildPlatformComponents(GameObject platform, string platformComponent, string extraComponent)
    {
        if (!string.IsNullOrEmpty(platformComponent))
        {
            Type t1 = Type.GetType(platformComponent);

            if (t1 != null)
                platform.AddComponent(t1);
        }

        if (!string.IsNullOrEmpty(extraComponent))
        {
            Type t2 = Type.GetType(extraComponent);

            if (t2 != null)
                platform.AddComponent(t2);
        }

        return platform;
    }
    #endregion

}
