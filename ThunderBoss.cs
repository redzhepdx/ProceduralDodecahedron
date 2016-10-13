using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ThunderBoss : MonoBehaviour {

    public GameObject[] smiteOrbs;
    public GameObject shockwave;
    public GameObject hellBreaker;
    public Lightning lightning;

    public int Health;

    public enum AttackingState {Idle , Stun , Level1 , Level2 , Level3 , Level4 , Level5};
    [HideInInspector]
    public AttackingState currentState = AttackingState.Level2;

    [HideInInspector]
    public AttackingState lastState;

    DodeShooter dodeShooter;

    Transform player;

    GameObject mapController;

    bool smiteReady = true;

    void Awake()
    {

        dodeShooter = Camera.main.GetComponent<DodeShooter>();

        mapController = GameObject.Find("NQueenInstantiator");

        OpenCloseSmiteOrbs(false);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    void Update()
    {
        ChangeStates();

        DestroySelf();

        AttackToThePlayer();

        SmitePlayer();
    }

    void ChangeStates()
    {
        if(dodeShooter.playerState == DodeShooter.PlayerState.Inside)
        {
            currentState = AttackingState.Idle;
        }
        else if(dodeShooter.playerState == DodeShooter.PlayerState.Oustside && currentState != AttackingState.Stun)
        {

            if (Health <= 100 && Health > 75)
            {
                currentState = AttackingState.Level1;
            }
            else if(Health <= 75 && Health > 50)
            {
                currentState = AttackingState.Level2;
            }
            else if (Health <= 50 && Health > 25)
            {
                currentState = AttackingState.Level3;
            }
            else if (Health <= 25 && Health > 15)
            {
                currentState = AttackingState.Level4;
            }
            else if (Health <= 15)
            {
                currentState = AttackingState.Level5;
            }

            lastState = currentState;

            OpenFeaturesWithLastState();
        }
    }

    void AttackToThePlayer()
    {
        if(currentState == AttackingState.Level1)
        {
            OpenCloseSmiteOrbs(true);
        }
        else if(currentState == AttackingState.Level2)
        {
            OpenHellBreaker(true);
        }
        else if(currentState == AttackingState.Level3)
        {
            MoveGrid(true);
        }
        else if (currentState == AttackingState.Level4)
        {
            OpenShockWave(true);
        }
        else if(currentState == AttackingState.Level5)
        {
            OpenShockWave(true);
        }
        else if(currentState == AttackingState.Stun)
        {
            CloseAllFeatures();
        }
        else if(currentState == AttackingState.Idle)
        {
            CloseAllFeatures();
        }
    }

    public void TakeDamage()
    {
        Health -= Random.Range(90, 90);
    }

    void DestroySelf()
    {
        if(Health <= 0)
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene("Level9(Dodecahedron)");
        }
    }

    void OpenCloseSmiteOrbs(bool enable)
    {
        for(int i = 0; i < smiteOrbs.Length; i++)
        {
            smiteOrbs[i].gameObject.SetActive(enable);
        }
    }

    void OpenShockWave(bool enable)
    {
         shockwave.SetActive(enable);
    }

    void MoveGrid(bool moveable)
    {
        if (!mapController.GetComponent<NQueens>().moveGrid)
            mapController.GetComponent<NQueens>().moveGrid = moveable;
    }

    void OpenHellBreaker(bool enable)
    {
        hellBreaker.SetActive(enable);
    }

    void CloseAllFeatures()
    {
        OpenShockWave(false);

        OpenHellBreaker(false);

        MoveGrid(false);

        OpenCloseSmiteOrbs(false);
    }

    public void OpenFeaturesWithLastState()
    {
        switch (lastState)
        {
            case AttackingState.Level1:
                OpenCloseSmiteOrbs(true);
                break;

            case AttackingState.Level2:
                OpenCloseSmiteOrbs(true);
                OpenHellBreaker(true);
                break;

            case AttackingState.Level3:
                OpenCloseSmiteOrbs(true);
                OpenHellBreaker(true);
                MoveGrid(true);
                break;

            case AttackingState.Level4:
                OpenCloseSmiteOrbs(true);
                OpenHellBreaker(true);
                MoveGrid(true);
                OpenShockWave(true);
                break;

            case AttackingState.Level5:
                OpenCloseSmiteOrbs(true);
                OpenHellBreaker(true);
                MoveGrid(true);
                OpenShockWave(true);
                break;
        }
    }

    public void SetLastStateToCurrentState()
    {
        currentState = lastState;
    }

    void SmitePlayer()
    {
        if(Vector3.Distance(transform.GetChild(3).position , player.position) < 200 && smiteReady)
        {
            Lightning l1 = Instantiate(lightning , transform.GetChild(3).localPosition , Quaternion.identity) as Lightning;

            l1.startPoint = transform.GetChild(3).position;

            l1.endPoint = player.position + Random.onUnitSphere * 20;

            l1.transform.parent = transform.GetChild(6);

            Destroy(l1.gameObject, 0.15f);

            smiteReady = false;

            Invoke("SetReadySmite", .5f);
        }
    }

    void SetReadySmite()
    {
        smiteReady = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SendedPill"))
        {
            if (currentState != AttackingState.Idle && currentState != AttackingState.Stun)
            {
                lastState = currentState;

                currentState = AttackingState.Stun;

                dodeShooter.bossStunned = true;

                Destroy(other.gameObject);
            }
        }
    }
}
