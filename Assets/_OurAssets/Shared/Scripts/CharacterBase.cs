using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class CharacterBase : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputActions playerInput;
    private Animator animator;

    [SerializeField] private bool isPlayer1;
    [SerializeField] private bool isDummy;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float runSpeed;
    [SerializeField] private float backSpeed;

    [SerializeField] private float testX;
    [SerializeField] private float testY;

    public int bonusDamage = 0;

    [SerializeField] float hitGravity;
    private float gravity = -20;

    private bool isHoldingUp;
    private bool isHoldingDown;

    private Vector3 playerVelocity;
    private Vector3 moveDirection;

    private float knockback_X;
    private float knockback_Y;

    private bool isGrounded;
    private bool isIncapacitated;
    private bool isDead;
    private bool facingRight;

    public delegate void OnAttackFoward();
    public event OnAttackFoward onAttackFoward;

    private float currentAnimSpeed;

    private bool canAttack = true;
    private CharacterBase otherGuy;

    private float maxHealth = 100;
    private float currentHealth = 100;
    private Image healthBar;

    [SerializeField] float groundCheck;

    private float maxMeter = 100;
    private float currentMeter = 0;
    private Image meterBar;
    private TextMeshProUGUI meterBarText;

    [SerializeField] Transform[] hitBoxes;
    [SerializeField] float testRange;

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject damageEffect;
    [SerializeField] GameObject groundHit;
    [SerializeField] GameObject groundCharge;
    [SerializeField] GameObject fireball;
    [SerializeField] GameObject yellBlood;
    [SerializeField] GameObject yellHands;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = new PlayerInputActions();
        playerInput.Player.Enable();

        string ID = (isPlayer1) ? "P1" : "P2" ;
        healthBar = GameObject.Find(ID + "health").GetComponent<Image>();
        meterBar = GameObject.Find(ID + "special").GetComponent<Image>();
        meterBarText = GameObject.Find(ID + "MeterText").GetComponent<TextMeshProUGUI>();

        healthBar.fillAmount = 1;
        meterBar.fillAmount = 0;
        meterBarText.text = "Meter: " + currentMeter + "%";

        otherGuy = (isPlayer1) ? GameObject.FindGameObjectWithTag("P2").GetComponent<CharacterBase>() : GameObject.FindGameObjectWithTag("P1").GetComponent<CharacterBase>() ;
    }

    void Update()
    {
        Gravity();

        if (isDead)
            return;

        GroundCheck();
        HandleKnockback();
        ManageBars();

        if (isIncapacitated)
            return;

        FaceEnemy();

        if (isGrounded)
        {
            Movement();
        }
        else
        {
            AirMomentum();
        }
    }

    public void Tie()
    {
        isDead = true;
        animator.SetTrigger("Tie");
    }

    private void ManageBars()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, 10 * Time.deltaTime);
        meterBar.fillAmount = Mathf.Lerp(meterBar.fillAmount, currentMeter / maxMeter, 10 * Time.deltaTime);
    }

    public float GetRemainingHealth()
    {
        return currentHealth;
    }

    private void GroundCheck()
    {
        RaycastHit check;

        float distance = 0.3f;

        Vector3 dir = new Vector3(0, -1);

        isGrounded = (Physics.Raycast(transform.position, dir, out check, distance));
        animator.SetBool("grounded", isGrounded);
    }

    private void HandleKnockback()
    {
        if(knockback_X <= 0.6f && knockback_X >= -0.6f && knockback_Y <= 0.6f)
        {
            knockback_X = 0;
            knockback_Y = 0;
            gravity = -20;
            return;
        }

        Vector3 knockbackForward = Vector3.zero;
        knockbackForward.x = 1;
        if (knockback_X <= 0.6f && knockback_X >= -0.6f)
            knockbackForward.x = 0;

        characterController.Move(knockbackForward * knockback_X * Time.deltaTime);

        if(knockback_X < 0)
            knockback_X += Time.deltaTime * 3;

        if(knockback_X > 0)
            knockback_X -= Time.deltaTime * 3;

        Vector3 knockbackUp = Vector3.zero;
        knockbackUp.y = 1;
        if (knockback_Y <= 0.6f)
            knockbackUp.y = 0;


        characterController.Move(knockbackUp * knockback_Y * Time.deltaTime);
        knockback_Y -= Time.deltaTime * 3;
    }

    private void Gravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -3f;
        }

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void FaceEnemy()
    {
        Vector3 faceDir = (otherGuy.transform.position - transform.position).normalized;
        Vector3 crossDir = Vector3.Cross(faceDir, transform.forward);

        if(crossDir.y > 0)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        Vector3 rotateDir = (facingRight) ? Vector3.one : new Vector3(-1,1,-1) ;
        transform.localScale = Vector3.Lerp(transform.localScale, rotateDir, 12 * Time.deltaTime);
    }

    private void Movement()
    {
        if (isHoldingDown)
            return;

        Vector2 inputVector = playerInput.Player.Movement.ReadValue<Vector2>();
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        float moveSpeed = runSpeed;

        if (inputVector.x < 0 && facingRight)
            moveSpeed = backSpeed;

        if (inputVector.x > 0 && !facingRight)
            moveSpeed = backSpeed;

        characterController.Move(transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);

        float speed = characterController.velocity.x;
        if (facingRight == false)
            speed *= -1;

        currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, speed, 6 * Time.deltaTime);

        animator.SetFloat("Speed", currentAnimSpeed);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (isIncapacitated || !canAttack || isDead)
            return;

        if (context.performed)
        {
            canAttack = false;

            if (!isGrounded)
            {
                animator.SetTrigger("attackFowardAir");
                return;
            }

            if (isHoldingUp)
            {
                animator.SetTrigger("attackUp");
                return;
            }

            if(isHoldingDown)
            {
                animator.SetTrigger("attackDown");
                return;
            }

            animator.SetTrigger("attackFoward");
        }
    }

    public void Special(InputAction.CallbackContext context)
    {
        if (isIncapacitated || !canAttack || isDead)
            return;

        if (context.performed)
        {
            canAttack = false;

            if (!isGrounded)
            {
                animator.SetTrigger("specialFowardAir");
                return;
            }

            if (isHoldingUp)
            {
                animator.SetTrigger("specialUp");
                GameObject charge = Instantiate(groundCharge, hitBoxes[0]);
                charge.transform.localPosition = Vector3.zero;
                Destroy(charge, 1.2f);

                return;
            }

            if (isHoldingDown)
            {
                animator.SetTrigger("specialDown");
                bonusDamage += 4;
                GameObject hand1 = Instantiate(yellHands, hitBoxes[0].position, hitBoxes[0].rotation);
                hand1.GetComponent<Hand>().Init(hitBoxes[0], this);
                GameObject hand2 = Instantiate(yellHands, hitBoxes[3].position, hitBoxes[3].rotation);
                hand2.GetComponent<Hand>().Init(hitBoxes[3], this);
                return;
            }

            animator.SetTrigger("specialFoward");
            Vector3 rot = new Vector3(-90, -90, 0);
            GameObject fireballPrefab = Instantiate(fireball, hitBoxes[2].position, Quaternion.Euler(rot));
            fireballPrefab.GetComponent<Fireball>().Init(hitBoxes[2], transform.right, facingRight, otherGuy, bonusDamage);
        }
    }
    public void RageOver()
    {
        bonusDamage -= 2;
    }

    public void spit()
    {
        GameObject spit = Instantiate(yellBlood, hitBoxes[4].position, hitBoxes[4].rotation);
    }

    public void HoldingUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isHoldingUp = true;
        }

        if(context.canceled)
        {
            isHoldingUp = false;
        }
    }

    public void HoldingDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isHoldingDown = true;
            animator.SetBool("crouching", true);
            Debug.Log(isHoldingDown);
        }

        if (context.canceled)
        {
            isHoldingDown = false;
            animator.SetBool("crouching", false);
            Debug.Log(isHoldingDown);
        }
    }

    private void AirMomentum()
    {
        characterController.Move(transform.TransformDirection(moveDirection) * runSpeed * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isIncapacitated || !canAttack || isDead)
            return;

        if (context.performed)
        {
            if (isGrounded)
            {
                StartCoroutine(SquatFrames());
                animator.SetTrigger("jump");
            }
        }
    }

    private IEnumerator SquatFrames()
    {
        yield return new WaitForSeconds(0.1f);

        playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * -20);
    }

    public bool CanAttack()
    {
        return canAttack;
    }

    public CharacterBase OtherPlayer()
    {
        return otherGuy;
    }

    private void AnimStop()
    {
        isIncapacitated = true;
    }

    private void AnimGo()
    {
        Physics.IgnoreCollision(characterController, otherGuy.GetComponent<Collider>(), false);
        isIncapacitated = false;
        canAttack = true;
    }

    public void SendAttack(AnimAttack attack)
    {
        Collider[] colliders = Physics.OverlapSphere(hitBoxes[attack.hitBoxID].position, attack.range); 

        foreach(Collider hit in colliders)
        {
            if (hit.GetComponent<CharacterBase>() == otherGuy)
            {
                if(otherGuy.ApplyDamage(attack.damage + bonusDamage))
                {
                    Vector3 rot = Vector3.zero;
                    rot.y = 90;
                    if (otherGuy.facingRight)
                        rot.y = -90;

                    Instantiate(hitEffect, hitBoxes[attack.hitBoxID].position, Quaternion.Euler(rot));
                    otherGuy.ApplyHitStun(attack.headShot);
                    otherGuy.ApplyKnockback(attack.knockbackX, attack.knockbackY);
                }
            }
        }
    }

    public bool ApplyDamage(float damage)
    {
        if (damage <= 0 || isDead == true)
            return false;

        Camera.main.GetComponent<FightCamera>().StartShake();
        Camera.main.GetComponent<ScreenVFX>().StartFreezeFrame();

        Vector3 pos = Vector3.zero;
        pos.y += 1;
        GameObject localDamageEffect = Instantiate(damageEffect, transform);
        damageEffect.transform.position = pos;

        if (currentAnimSpeed <= -0.6f)
        {
            animator.SetTrigger("guard");
            StartCoroutine(IncapacitatedDuration(0.5f));
            AddMeter(damage / 2);
            return false;
        }

        AddMeter(damage);
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            animator.SetTrigger("death");
        }

        return true;
    }

    public void ApplyHitStun(bool headShot) // headShot determines their flinch, headshots stun for longer
    {
        StopAllCoroutines();

        string flinchType = (headShot) ? "flinchHead" : "flinchBody" ;
        float flinchDuration = (headShot) ? 0.6f : 0.3f ;

        if (!isGrounded)
        {
            flinchType = "flinchBody";
            flinchDuration = 0.4f;
        }

        StartCoroutine(IncapacitatedDuration(flinchDuration));
        animator.SetTrigger(flinchType);
    }

    public void ApplyKnockback(float horizontalKnockback, float verticalKnockback)
    {
        if (facingRight)
        {
            knockback_X += horizontalKnockback;
        }
        else
        {
            knockback_X -= horizontalKnockback;
        }

        knockback_Y = verticalKnockback;
        
        if (knockback_Y > 0)
            gravity = hitGravity;

        Physics.IgnoreCollision(characterController, otherGuy.GetComponent<Collider>());
    }

    public void ApplyGrab()
    {
        StopAllCoroutines();
        animator.SetTrigger("grab");
        StartCoroutine(IncapacitatedDuration(2));
    }

    public void TryGrab()
    {
        Collider[] colliders = Physics.OverlapSphere(hitBoxes[1].position, 1);

        bool gotem = false;

        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<CharacterBase>() == otherGuy)
            {
                gotem = true;
                Debug.Log("Got em");
                otherGuy.ApplyGrab();
            }
        }

        if(gotem == false)
        {
            animator.SetTrigger("missSpecialAir");
        }
    }

    private void GroundHit()
    {
        Vector3 pos = Vector3.zero;
        pos.x += 1;

        GameObject groundSpark = Instantiate(groundHit, transform);
        groundSpark.transform.localPosition = pos;
    }

    IEnumerator IncapacitatedDuration(float duration)
    {
        isIncapacitated = true;
        canAttack = false;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        canAttack = true;
        isIncapacitated = false;

    }

    public bool TryUseMeter(float meterCost)
    {
        if(currentMeter > meterCost)
        {
            currentMeter -= meterCost;
            meterBarText.text = "Meter: " + currentMeter + "%";
            return true;
        }

        return false;
    }

    public void AddMeter(float meterAdded)
    {
        currentMeter += meterAdded;
        currentMeter = Mathf.Clamp(currentMeter, 0, maxMeter);
        meterBarText.text = "Meter: " + currentMeter + "%";
    }

    private void OnDrawGizmos()
    {
        if (isDummy)
            return;

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(hitBoxes[0].position, testRange);
    }
}
