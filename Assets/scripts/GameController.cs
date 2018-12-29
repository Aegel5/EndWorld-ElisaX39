using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController ThisInstance = null;

    List<Spawner> lstSpawners = new List<Spawner>();

    public GameController()
    {
        ThisInstance = this;
    }

    private void Awake()
    {

    }

    public float MinX = 10;
    public float MaxX = 490;
    public float MinZ = 10;
    public float MaxZ = 490;

    public void RegisterSpawner(Spawner spawner)
    {
        lock(lstSpawners)
        {
            lstSpawners.Add(spawner);
        }
    }

    public Spawner RandomSpawner(int team, bool enemy)
    {
        lock(lstSpawners)
        {
            var lst = lstSpawners.Where(x => enemy ? x.team != team : x.team == team).Where(x => !x.IsDead);
            if (!lst.Any())
                return null;
            return lst.ElementAt(Random.Range(0, lst.Count() - 1));
        }
    }

    public bool IsCoordCorrect(float x, float z)
    {
        if (x < MinX || x > MaxX || z < MinZ || z > MaxZ)
            return false;
        return true;
    }
}
