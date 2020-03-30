using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{    
    public delegate void SpawnFunction();
    class Spawner
    {
        public float distInterval;
        public int expectCount;
        public int fullCount;
        int actualCount;
        int intervalCount;
        float dist;
        SpawnFunction function;
        public Spawner(float distInterval, int expectCount, int fullCount, SpawnFunction function)
        {
            this.distInterval = distInterval;
            this.expectCount = expectCount;
            this.fullCount = fullCount;
            this.function = function;
            actualCount = 0;
            intervalCount = 0;
            dist = 0f;
        }
        public void Update()
        {
            dist += GameManager.Instance.scrollSpeed * Time.deltaTime;
            if(dist >= distInterval / fullCount)
            {
                dist -= distInterval / fullCount;
                if(Random.Range(0f,1f) < (float)(expectCount-actualCount)/(fullCount-intervalCount))
                {
                    function();
                    actualCount ++;
                }
                intervalCount ++;
                if(intervalCount == fullCount)
                {
                    actualCount = 0;
                    intervalCount = 0;
                }
            }
        }
    }

    Spawner coinSpawner;
    Spawner spikeSpawner;

    void Awake()
    {
        coinSpawner = new Spawner(3000,8,10,CreateCoin);
        spikeSpawner = new Spawner(2500,4,5,CreateSpike);
    }

    void Update()
    {
        coinSpawner.Update();
        spikeSpawner.Update();
    }
    void CreateCoin()
    {
        Coin coin = PoolManager.Instance.GetObject<Coin>(Defs.Coin);
        coin.Initialize(Random.Range(-1,2) * 150);
    }
    void CreateSpike()
    {
        Spike spike = PoolManager.Instance.GetObject<Spike>(Defs.Spike);
        int dir = Random.Range(0,2)*2 - 1;
        spike.Initialize(dir);
        if(Random.Range(0f,1f) > 0.66f)
        {
            Spike spike2 = PoolManager.Instance.GetObject<Spike>(Defs.Spike);
            spike2.Initialize(dir,true);
        }
    }
}
