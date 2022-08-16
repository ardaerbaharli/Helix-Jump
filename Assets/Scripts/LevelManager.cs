using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

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
        helix.helixParts = new List<HelixPart>();
        helix.pooledObject = levelParent;
        levelParent.transform.position = levelPos;

        var numberOfParts = Random.Range(7, 10);
        var angles = GetAngles(numberOfParts);
        var angle = Vector3.zero;

        for (var i = 0; i < numberOfParts; i++)
        {
            angle.y = angles[i];
            var part = ObjectPool.instance.GetPooledObject("helix-part");
            part.transform.position = levelPos;
            part.transform.rotation = Quaternion.Euler(angle);
            part.transform.parent = levelParent.transform;
            part.rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            var helixPart = part.gameObject.GetComponent<HelixPart>();
            helixPart.pooledObject = part;
            helix.helixParts.Add(helixPart);
        }

        levelIndex++;
        levels.Add(helix);
    }


    //TODO fix this
    private readonly List<float> ANGLES = new() {0, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f};
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