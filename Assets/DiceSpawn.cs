using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    public GameObject dicePrefab;
    public Transform spawnPoint;

    public void SpawnAndLaunchDice()
    {
        GameObject dice = Instantiate(dicePrefab, spawnPoint.position, Random.rotation);
        DiceMover mover = dice.GetComponent<DiceMover>();
        mover.Roll();
    }
}