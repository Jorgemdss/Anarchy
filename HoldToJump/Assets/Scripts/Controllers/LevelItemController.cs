using UnityEngine;
using System.Collections;
using Utility;

public class LevelItemController : MonoBehaviour
{
    byte minPlatFormGap;
    byte maxPlatFormGap;
    float platformSpawnCounter;

    #region Properties

    public ObstaclePooler platformPooler { get; private set; }

    public LevelManagerController LevelManagerController { get; set; }
    public int NormalVerticalPlatformSpeed { get; set; }
    public int FloatingPlatformSpeed { get; set; }

    public int RotatingPlatformSpeed { get; set; }

    public int BasicPlatformSpawnInterval { get; set; }

    #endregion

    void Awake()
    {
        NormalVerticalPlatformSpeed = 4;
        FloatingPlatformSpeed = 4;
    }

    void Start ()
    {        
        minPlatFormGap = 1;
        maxPlatFormGap = 4;
        BasicPlatformSpawnInterval = 3;
        platformPooler = GameObject.FindObjectOfType<ObstaclePooler>().GetComponent<ObstaclePooler>();
        LevelManagerController = FindObjectOfType<LevelManagerController>();
    }
		
	void Update ()
    {
        platformSpawnCounter += Time.deltaTime;

        if (Utilities.IsCounterGreaterThanInterval(platformSpawnCounter, BasicPlatformSpawnInterval))
        {
            ActivatePlatForm(LevelManagerController.CurrentLevel);
        }
        
    }

    /// <summary>
    /// Instantiates platforms based on the player level
    /// </summary>
    /// <param name="currentLevel">The player level</param>
    public void ActivatePlatForm(int currentLevel)
    {
        var floatingPlatformRNGchance = UnityEngine.Random.Range(0, 100);
        GameObject platform = null;

        if (currentLevel >= 1 && currentLevel <= 2)
        {
            platform = platformPooler.GetPooledObject(BasePlatformTypeComponent.PlatformType.NormalVertical);
        }
        else if (currentLevel>=3)
        {
            if (floatingPlatformRNGchance > 60)
            {
                platform = platformPooler.GetPooledObject(BasePlatformTypeComponent.PlatformType.Floating);
            }
            else
            {
                platform = platformPooler.GetPooledObject(BasePlatformTypeComponent.PlatformType.NormalVertical);
            }
        }
        
        

        platform.SetActive(true);
        platformSpawnCounter = 0;

        BasicPlatformSpawnInterval = Random.Range(minPlatFormGap, maxPlatFormGap);
    }
}
