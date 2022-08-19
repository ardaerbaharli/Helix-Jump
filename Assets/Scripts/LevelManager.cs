using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Helix initialLevel;
    
    [SerializeField] private int numberOfInitialLevels;
    [SerializeField] private float yDif;
    [SerializeField] private Ball ball;
    public Helix ActiveHelix => levels[ScoreManager.instance.Score];

    private List<Helix> levels;
    private int levelIndex;

    private void OnEnable()
    {
        levels = new List<Helix>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ObjectPool.instance.isPoolSet);
        
        levelIndex++;
        levels.Add(initialLevel);
        
        LoadInitialLevels();
        
        ball.GameObject().SetActive(true);
    }


    private void LoadInitialLevels()
    {
        for (var j = 0; j < numberOfInitialLevels; j++)
            LoadLevel();

        playerController.activeHelix = levels[0];
    }

    public void LoadLevel()
    {
        var levelParent = ObjectPool.instance.GetPooledObject("levelParent");
        levelParent.gameObject.name = "Level" + (levelIndex + 1);
        var levelPos = Vector3.zero + new Vector3(0, yDif * levelIndex, 0);
        var helix = levelParent.gameObject.GetComponent<Helix>();
        helix.hoops = new List<Hoop>();
        helix.pooledObject = levelParent;
        levelParent.transform.position = levelPos;

        var numberOfParts = Random.Range(2, 7);
        var angles = GetAngles(numberOfParts);
        var angle = Vector3.zero;

        for (var i = 0; i < numberOfParts; i++)
        {
            angle.y = angles[i];
            var hoop = ObjectPool.instance.GetPooledObject("hoop");
            hoop.transform.position = levelPos;
            hoop.transform.rotation = Quaternion.Euler(angle);
            hoop.transform.parent = levelParent.transform;
            hoop.rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            // var helixPart = part.gameObject.GetComponent<HelixPart>();
            var hoopController = hoop.gameObject.GetComponent<Hoop>();
            hoopController.pooledObject = hoop;
            helix.hoops.Add(hoopController);
        }

        levelIndex++;
        levels.Add(helix);
    }


    //TODO fix this
    private readonly List<float> ANGLES = new() {0, 45f, 90f, 135f, 180f, 225f, 270f, 315f};

    private List<float> GetAngles(int numberOfParts)
    {
        // return numberOfParts piece of unique angles from ANGLES list
        var angles = new List<float>();
        for (var i = 0; i < numberOfParts; i++)
        {
            var angle = ANGLES[Random.Range(0, ANGLES.Count)];
            while (angles.Contains(angle))
                angle = ANGLES[Random.Range(0, ANGLES.Count)];
            angles.Add(angle);
        }

        return angles;
    }
}