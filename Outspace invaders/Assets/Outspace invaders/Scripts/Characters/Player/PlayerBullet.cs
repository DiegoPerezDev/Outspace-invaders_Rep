using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [HideInInspector] public Player playerThatShootThis;
    private readonly float timeForAutoDestruction = 3f;
    private bool destroyingBarricade;

    void Start()
    {
        StartCoroutine(AutoDestructionDelay());
        AddSpeed();
    }
    void OnDestroy()
    {
        playerThatShootThis.StartCoroutine(playerThatShootThis.DelayBeforeShootingAgain());
        StopAllCoroutines();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject)
            Destroy(gameObject);

        string colidedObject = collision.gameObject.tag;
        if (colidedObject == "Enemy")
        {
            Destroy(collision.gameObject);
            AlienArmy.RemovingAlien(collision.gameObject.GetComponent<Alien>());
            AlienArmy.OnAlienDestroyed?.Invoke();
        }
        else if (colidedObject == "DestroyableEnemyAttack")
            Destroy(collision.gameObject);
        else if(colidedObject == "RandomEnemy")
        {
            Destroy(collision.gameObject);
            RandomAlien.OnRandomAlienDestroyed?.Invoke();
        }
        else if (colidedObject == "Barricade")
        {
            if(!destroyingBarricade)
            {
                destroyingBarricade = true;
                Destroy(collision.contacts[0].collider.gameObject);
            }
        }
            
    }

    private void AddSpeed() => GetComponent<Rigidbody2D>().velocity = new Vector2(0f, playerThatShootThis.bulletVel);
    private IEnumerator AutoDestructionDelay()
    {
        //Delay
        var timer = 0f;
        while(timer < timeForAutoDestruction)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        // Destroy self gameObject
        if (gameObject)
            Destroy(gameObject);
    }

}