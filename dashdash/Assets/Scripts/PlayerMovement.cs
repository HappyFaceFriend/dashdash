using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public int landPosX;
    public float powerTime;
    float chargeTime;
    bool isPowered;
    public enum State 
    {
        Prep, Idle, Charging, Jumping, Landing, Dead
    }
    public State state;
    //left : -1 , right : 1
    int dir;

    public float jumpDuration;
    public float powerJumpDuration;

    float jumpSpeed;
    float powerJumpSpeed;

    Animator animator;
    float lastX;

    public enum BufferType { None, Charge, Jump }
    public BufferType buffer;
    float bufferETime;
    public float bufferTime;

    public float shakeValue;
    public float shakeSpeed;
    float shakeDist;
    int shakeDir;
    float originalY;

    public GameObject sweat;

    public CameraScript cameraScript;

    public int life;
    bool isImmune;
    public float immuneTime;
    float immuneETime;
    float immuneTick;

    public List<SpriteRenderer> sprites;

    public LifeSet lifeSet;

    BoxCollider2D collider;
    public bool isStomping;
    public int landCount = 0;
    bool scaleLock;
    public GameObject deathParticle;

    public CharacterChange characterChange;

    public Color[] dieColors1;
    public Color[] dieColors2;

    public ParticleSystem deathParticle1;
    public ParticleSystem deathParticle2;
    public ParticleSystem hitParticle;
    
    public bool isPaused;
    
    public GameObject pausePanel;

    float scrollAlpha = 1f;

    public GameObject pinkStomp;
    public GameObject goldStomp;
    public GameObject goldParticle;
    public GameObject pauseButton;

    public AudioSource startChargeSound;
    public AudioSource powerChargeSound;
    public AudioSource jumpSound;
    public AudioSource landSound;
    public AudioSource stompSound;
    public AudioSource stompSound2;
    public AudioSource hurtSound;
    public AudioSource shakeSound;
    bool chargeSoundPlayed;
    bool powerSoundPlayed;
    int shakeCount = 0;

    void Awake()
    {
        //state = State.Idle;
        dir = 1;
        isPowered = false;
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        jumpSpeed = landPosX*2 / jumpDuration;
        powerJumpSpeed = landPosX*2 / powerJumpDuration;
        buffer = BufferType.None;
        shakeDist = 0f;
        shakeDir = 1;
        originalY = transform.position.y;
        life = 3;
        isImmune = false;
        isStomping = false;
        chargeSoundPlayed = false;
        powerSoundPlayed = false;
        GameManager.Instance.targetScrollSpeed = 900;
        if(DataManager.Instance.character > 3)
        {
            SetTimes(1.2f);
        }
        StartCoroutine(IncreaseSpeed());
        scaleLock = false;
        GameManager.Instance.StartGame();
        characterChange.ChangeImage(DataManager.Instance.character);
        ChangeParticleColor(deathParticle1, dieColors1[DataManager.Instance.character-1]);
        ChangeParticleColor(deathParticle2, dieColors2[DataManager.Instance.character-1]);
        ChangeParticleColor(hitParticle, dieColors1[DataManager.Instance.character-1]);
        ChangeParticleColor(sweat.GetComponent<ParticleSystem>(), dieColors2[DataManager.Instance.character-1]);
    }
    void Start()
    {
        StartCoroutine(Prep());
        if(DataManager.Instance.character > 3)
        {
            if(DataManager.Instance.character == 4)
                PoolManager.Instance.SetPoolPrefab("Stomp", pinkStomp);
            else if(DataManager.Instance.character == 5)
            {
                PoolManager.Instance.SetPoolPrefab("Stomp", goldStomp);
            }
        }
    }
    IEnumerator IncreaseSpeed()
    {
        yield return new WaitForSeconds(18f);
        if(scrollAlpha * 1.05f <= 1.6f)
        {
            StartCoroutine(IncreaseSpeed());
            SetTimes(1.05f);
        }
        else
            SetTimes(1.6f/scrollAlpha);
    }
    public void PauseButtonClicked()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        SoundManager.Instance.gameBgm.Pause();
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        SoundManager.Instance.gameBgm.Play();
    }
    public void Exit()
    {
        Resume();
        pauseButton.SetActive(false);
        SoundManager.Instance.gameBgm.Stop();
        Die();
    }
    void SetTimes(float alpha)
    {
        scrollAlpha *= alpha;
        GameManager.Instance.targetScrollSpeed  *= alpha;
        GameManager.Instance.scrollSpeed *= alpha;
        powerTime /= alpha;
        bufferTime /= alpha;
        jumpDuration /= alpha;
        powerJumpDuration /= alpha;
        immuneTime /= alpha;
        animator.SetFloat("AnimSpeed",alpha);
    }
    IEnumerator Prep()
    {
        float eTime = 0f;
        float duration = 0.3f;
        while(eTime <= duration)
        {
            eTime += Time.deltaTime;
            transform.eulerAngles += new Vector3(0,0,(360*3-90)/duration) * Time.deltaTime;
            transform.position += new Vector3(120f,0,0) * Time.deltaTime / (duration+0.15f);
            yield return null;
        }
        transform.eulerAngles = new Vector3(0,0,270);
        while(eTime <= duration + 0.15f)
        {
            eTime += Time.deltaTime;
            transform.position += new Vector3(120f,0,0) * Time.deltaTime / (duration+0.15f);
            yield return null;
        }
        if(DataManager.Instance.character == 5)
            goldParticle.SetActive(true);
        StartCharging();
        isPowered = true;
        Jump();
    }
    void Update()
    {
        if(state == State.Dead || state == State.Prep)
            return;
        ProcessInputs();
        UpdateBuffer();
        goldParticle.transform.position = transform.position + new Vector3(0,0,500);
        if(state == State.Charging)
        {
            chargeTime += Time.deltaTime;
            if(!chargeSoundPlayed && chargeTime >= 0.2f)
            {
                startChargeSound.Play();
                chargeSoundPlayed = true;
            }
                
            if(chargeTime >= powerTime)
            {
                isPowered = true;
                if(!powerSoundPlayed)
                    powerChargeSound.Play();
                powerSoundPlayed = true;
                sweat.gameObject.SetActive(true);
                shakeDist += shakeSpeed * Time.deltaTime;
                transform.Translate(new Vector3(shakeDir * shakeSpeed,0f,0f) * Time.deltaTime);
                if(shakeDist >= shakeValue)
                {
                    shakeDist -= shakeValue;
                    shakeDir *= -1;
                    if(shakeCount == 0)
                    {
                        shakeSound.Play();
                        shakeCount = 2;
                    }
                    shakeCount--;
                }
            }   
        }
        else if(state == State.Jumping)
        {
            if(!isPowered)
                transform.position += (new Vector3(jumpSpeed * Time.deltaTime * -dir, 0f, 0f));
            else
                transform.position += (new Vector3(powerJumpSpeed * Time.deltaTime * -dir, 0f, 0f));
            if(lastX * transform.position.x < 0 && scaleLock)
            {
                sweat.transform.localScale = new Vector3(sweat.transform.localScale.x,-sweat.transform.localScale.y,sweat.transform.localScale.z);
                transform.localScale = new Vector3(transform.localScale.x,-transform.localScale.y,transform.localScale.z);
            }
            lastX = transform.position.x;
            if(Mathf.Abs(transform.position.x) >= landPosX)
            {
                Land();
                scaleLock = true;
            }
        }
        else if(state == State.Landing)
        {
            if(landCount < 2)
                landCount++;
            else
            {
                StopLand();
            }
        }
        if(isImmune)
        {
            immuneETime += Time.deltaTime;
            immuneTick += Time.deltaTime;
            if(immuneTick >= 0.125f)
                SetAlpha(1f);
            else
                SetAlpha(0.2f);
            if(immuneTick >=0.25f)
                immuneTick-=0.25f;
            if(immuneETime >= immuneTime)
            {
                immuneTick = 0f;
                isImmune = false;
                SetAlpha(1f);
            }
        }
        
    }
    void ChangeState(State state)
    {
        this.state = state;
    }
    void StopLand()
    {
        if(isStomping)
        {
            SetCollider(-1);
        }
        isStomping = false;
        isPowered = false;
        ChangeState(State.Idle);
        landCount = 0;        
        if(buffer == BufferType.Charge)
            StartCharging();
        else if(buffer == BufferType.Jump)
        {
            StartCharging();
            Jump();
        }
    }
    void Land()
    {
        dir *= -1;
        transform.position = new Vector3(dir * landPosX, transform.position.y ,0f);
        ChangeState(State.Landing); 
        animator.SetTrigger("Land");
        cameraScript.ShakeLand(isPowered);
        ScrollingParticle stomp = PoolManager.Instance.GetObject<ScrollingParticle>(Defs.Stomp);
        if(isPowered)
            stomp.Initialize(Defs.Stomp, transform.position + new Vector3(53f*dir,0f,0f), dir*90f, 1.3f);
        else
            stomp.Initialize(Defs.Stomp, transform.position + new Vector3(53f*dir,0f,0f), dir*90f, 1f);

        if(isPowered)
        {
            isStomping = true;
            SetCollider(1);
            ScrollingParticle stompeff = PoolManager.Instance.GetObject<ScrollingParticle>(Defs.StompGrass);
            stompeff.Initialize(Defs.StompGrass, transform.position + new Vector3(53f*dir,0f,0f), dir*90f);
            stompSound.Play();
            stompSound2.Play();
            
        }
        else
            landSound.Play();
    }
    void SetCollider(int a)
    {
        if(a > 0)
        {
            collider.size = new Vector2(collider.size.x * 2.2f, collider.size.y);
            collider.offset += new Vector2(-50,0);
        }
        else
        {
            collider.size = new Vector2(collider.size.x / 2.2f, collider.size.y);
            collider.offset -= new Vector2(-50,0);
        }
    }
    void Jump()
    {
        sweat.gameObject.SetActive(false);
        ChangeState(State.Jumping); 
        if(!isPowered)
            animator.SetFloat("jumpAnimSpeed", 1f/jumpDuration);
        else
            animator.SetFloat("jumpAnimSpeed", 1f/powerJumpDuration);
        animator.SetTrigger("Jump");
        lastX = transform.position.x;
        shakeDist = 0f;
        transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
        cameraScript.ShakeJump();
        startChargeSound.Stop();
        if(!isPowered)
            jumpSound.Play();
        chargeSoundPlayed = false;
                powerChargeSound.Stop();
                powerSoundPlayed = false;
    }
    void StartCharging()
    {
        chargeTime = 0f;
        isPowered = false;
        ChangeState(State.Charging);
        animator.SetTrigger("Charge");
        buffer = BufferType.None;
    }
    void ColWithSpike(Spike spike)
    {
        if((isPowered && state == State.Jumping) || isStomping)
        {
            spike.ColWithStomp();
            GameManager.Instance.StompSpike(transform.position);
        }
        else if(!isImmune)
        {
            life -=1;
            lifeSet.GetDamage();
            isImmune = true;
            immuneTick = 0f;
            immuneETime = 0f;
            cameraScript.ShakeSpike();
            if(life ==0)
                Die();
            else
            {
                hitParticle.transform.position = transform.position + new Vector3(0,0,500);
                hitParticle.transform.localScale = transform.localScale;
                hitParticle.gameObject.SetActive(true);
                hurtSound.Play();
            }
        }
    }
    void Die()
    {
        ChangeState(State.Dead);
        gameObject.SetActive(false);
        goldParticle.SetActive(false);
        deathParticle.transform.position = transform.position;
        deathParticle.transform.localScale = transform.localScale;
        deathParticle.SetActive(true);
        GameManager.Instance.EndGame();
    }
    //void 
    void ColWithCoin(Coin coin)
    {
        if(!coin.isDead)
        {
            GameManager.Instance.EatCoin(coin.transform.position);
            coin.ColWithPlayer();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spikes")
            ColWithSpike(collision.GetComponent<Spike>());
        else if(collision.gameObject.tag == "Coin")
            ColWithCoin(collision.GetComponent<Coin>());
    }

    void QueueCharge()
    {
        buffer = BufferType.Charge;
        bufferETime = 0f;
    }
    void QueueJump()
    {
        buffer = BufferType.Jump;
        bufferETime = 0f;
    }
    void UpdateBuffer()
    {
        if(buffer !=BufferType.None)
        {
            bufferETime += Time.deltaTime;
            if(buffer ==BufferType.Jump && bufferETime >= bufferTime-0.05f)
                buffer = BufferType.None;
            else if(buffer ==BufferType.Charge && bufferETime >= bufferTime+0.05f)
                buffer = BufferType.None;
        }
    }
    void ProcessInputs()
    {
        if(Input.GetMouseButtonDown(0) && !Funcs.IsPointerOverGameObject())
        {
            if(state == State.Idle || state == State.Landing)
            {
                StopLand();
                StartCharging();
            }
            else if(state == State.Jumping)
            {
                QueueCharge();
            }
        }
        else if(Input.GetMouseButtonUp(0) && !Funcs.IsPointerOverGameObject())
        {
            if(state == State.Charging)
            {
                Jump();
            }
            if((state == State.Jumping || state == State.Landing) && buffer == BufferType.Charge)
            {
                QueueJump();
            }
        }
    }
    void SetAlpha(float a)
    {
        if(sprites[0].color.a == a)
            return;
        foreach(SpriteRenderer sr in sprites)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
        }
    }
    void ChangeParticleColor(ParticleSystem particle, Color color)
    {
        ParticleSystem.MainModule mod = particle.main;
        mod.startColor = color;
    }
}
