using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    enum ObType { Coin, Spike}
    struct SpawnData
    {  
        public ObType type;
        public float distance;
        public int x;
        public SpawnData(ObType type, int x, float distance)
        {
            this.type = type;
            this.distance = distance;
            this.x = x;
        }
    }
    class Pattern
    {
        public List<SpawnData> spawnDatas = new List<SpawnData>();
        float offset;
        public Pattern(float offset = 0f)
        {
            this.offset = offset;
        }
        public Pattern Add(ObType type, int x, float distance)
        {
            spawnDatas.Add(new SpawnData(type,x,offset + distance));
            return this;
        }
    }
    Pattern currentPattern;
    public float passedDistance;
    int index;
    List<Pattern> patterns;
    void Awake()
    {
        InitPatterns();
        passedDistance = 0f;
        StartPattern(true);
    }
    void Update()
    {
        passedDistance += GameManager.Instance.scrollSpeed * Time.deltaTime;
        if(passedDistance >= currentPattern.spawnDatas[index].distance)
        {
            SpawnByData(currentPattern.spawnDatas[index]);
            index++;
            if(index == currentPattern.spawnDatas.Count)
                StartPattern();
        }
    }
    void StartPattern(bool first = false)
    {
        Pattern pattern = patterns[0];
        if(!first)
            passedDistance -= currentPattern.spawnDatas[currentPattern.spawnDatas.Count-1].distance;
        currentPattern = pattern;
        index = 0;
    }
    void SpawnByData(SpawnData data)
    {
        Debug.Log(""+index + ":" + data.type + ","+data.x);
        if(data.type == ObType.Coin)
            CreateCoin(150 * data.x);
        else
            CreateSpike(data.x<0?-1:1);
    }
    void CreateCoin(int x)
    {
        Coin coin = PoolManager.Instance.GetObject<Coin>(Defs.Coin);
        coin.Initialize(x);
    }
    void CreateSpike(int dir)
    {
        Spike spike = PoolManager.Instance.GetObject<Spike>(Defs.Spike);
        spike.Initialize(dir);
    }
    void InitPatterns()
    {
        patterns = new List<Pattern>();
        Pattern p = new Pattern(400)
        .Add(ObType.Coin,   -1, 000)
        .Add(ObType.Coin,   0,  100)
        .Add(ObType.Coin,   1,  200)
        .Add(ObType.Spike,  -1,  200)
        .Add(ObType.Spike,  -1,  400)
        .Add(ObType.Spike,  -1,  600)
        .Add(ObType.Coin,   1,  600)
        .Add(ObType.Coin,   0,  700)
        .Add(ObType.Coin,   -1, 800)
        ;
        patterns.Add(p);
    }
}
