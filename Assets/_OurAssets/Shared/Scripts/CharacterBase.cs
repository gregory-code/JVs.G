using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputActions playerInput;
    private Animator animator;

    [SerializeField] private bool isPlayer1;
    [SerializeField] private bool isGrappler;
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

    [SerializeField] Transform[] hitBoxes;
    [SerializeField] float testRange;

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject damageEffect;
    [SerializeField] GameObject groundHit;
    [SerializeField] GameObject groundCharge;
    [SerializeField] GameObject fireball;
    [SerializeField] GameObject yellBlood;
    [SerializeField] GameObject yellHands;
    [SerializeField] GameObject slash;
    [SerializeField] Transform slashPos;
    [SerializeField] GameObject slashGround;
    [SerializeField] GameObject stormHands;
    [SerializeField] GameObject swordSpin;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetBool("isGrappler", isGrappler);
        playerInput = new PlayerInputActions();


        if (isPlayer1)
        {
            playerInput.Grappler.Enable();
        }
        else
        {
            playerInput.Knight.Enable();
        }

        string ID = (isPlayer1) ? "P1" : "P2";
        healthBar = GameObject.Find(ID + "health").GetComponent<Image>();

        healthBar.fillAmount = 1;

        StartCoroutine(DelaySetup());
    }

    IEnumerator DelaySetup()
    {
        yield return new WaitForEndOfFrame();

        otherGuy = (isPlayer1) ? GameObject.FindGameObjectWithTag("P2").GetComponent<CharacterBase>() : GameObject.FindGameObjectWithTag("P1").GetComponent<CharacterBase>();
    }

    public PlayerInputActions GetInput()
    {
        return playerInput;
    }

    void Update()
    {
        Gravity();

        if (isDead)
            return;

        GroundCheck();
        HandleKnockback();
        HandleZoomies();
        HandleSlide();
        ManageBars();


        if(otherGuy != null)
            FaceEnemy();

        if (isIncapacitated)
            return;

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
        transform.localScale = Vector3.Lerp(transform.localScale, rotateDir, 20 * Time.deltaTime);
    }

    private void Movement()
    {
        if (isHoldingDown)
            return;

        Vector2 inputVector = playerInput.Grappler.Movement.ReadValue<Vector2>();

        if (!isPlayer1)
        {
            inputVector = playerInput.Knight.Movement.ReadValue<Vector2>();
        }

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
                if(isGrappler)
                {
                    GameObject charge = Instantiate(groundCharge, hitBoxes[0]);
                    charge.transform.localPosition = Vector3.zero;
                    Destroy(charge, 1.2f);
                }

                return;
            }

            if (isHoldingDown)
            {
                animator.SetTrigger("specialDown");
                if(isGrappler)
                {
                    bonusDamage += 4;
                    GameObject hand1 = Instantiate(yellHands, hitBoxes[0].position, hitBoxes[0].rotation);
                    hand1.GetComponent<Hand>().Init(hitBoxes[0], this);
                    GameObject hand2 = Instantiate(yellHands, hitBoxes[3].position, hitBoxes[3].rotation);
                    hand2.GetComponent<Hand>().Init(hitBoxes[3], this);
                }
                return;
            }

            animator.SetTrigger("specialFoward");
            if(isGrappler)
            {
                Vector3 rot = new Vector3(-90, -90, 0);
                GameObject fireballPrefab = Instantiate(fireball, hitBoxes[2].position, Quaternion.Euler(rot));
                fireballPrefab.GetComponent<Fireball>().Init(hitBoxes[2], transform.right, facingRight, otherGuy, bonusDamage);
            }
            else
            {
                characterController.enabled = false;
                GameObject thunderHand = Instantiate(stormHands, hitBoxes[0].position, hitBoxes[0].rotation);
                thunderHand.GetComponent<KnightDash>().Init(hitBoxes[0], this);
                GameObject thunderHand2 = Instantiate(stormHands, hitBoxes[3].position, hitBoxes[3].rotation);
                thunderHand2.GetComponent<KnightDash>().Init(hitBoxes[3], this);
            }
        }
    }
    public void RageOver()
    {
        bonusDamage -= 2;
    }

    public void Zoom()
    {
        //characterController.enabled = false;
        zoomies = true;

        Vector2 inputVector = playerInput.Knight.Movement.ReadValue<Vector2>();
        Vector3 teleport = otherGuy.transform.position;

        if (inputVector.x > 0)
        {
            teleport.x += 5f;
        }

        if (inputVector.x < 0)
        {
            teleport.x -= 5f;
        }

        if(inputVector.x == 0)
        {
            teleport.x = transform.position.x;
        }

        transform.position = teleport;

        StartCoroutine(EnableTheControllerDammit());
    }
    bool zoomies;
    bool sliding;
    public void Slide()
    {
        sliding = true;
    }

    IEnumerator EnableTheControllerDammit()
    {
        yield return new WaitForEndOfFrame();
        characterController.enabled = true;
    }

    private void HandleZoomies()
    {
        if (zoomies == false)
            return;

        Vector3 fast = Vector3.zero;
        fast.x = 1;

        if (facingRight == false)
            fast *= -1;

        characterController.Move(fast * 30 * Time.deltaTime);
    }

    private void HandleSlide()
    {
        if (sliding == false)
            return;

        Vector3 fast = Vector3.zero;
        fast.x = 1;

        if (facingRight == false)
            fast *= -1;

        characterController.Move(fast * 18 * Time.deltaTime);
    }

    public void spit()
    {
        GameObject spit = Instantiate(yellBlood, hitBoxes[4].position, hitBoxes[4].rotation);
        if(facingRight == false)
        {
            Vector3 rot = new Vector3(0, -90, 90);
            spit.transform.rotation = Quaternion.Euler(rot);
        }
    }

    public void Spin()
    {
        GameObject spin = Instantiate(swordSpin, transform.position, transform.rotation);
        Vector3 rot = new Vector3(-90, 0, 0);
        spin.transform.rotation = Quaternion.Euler(rot);
        Destroy(spin, 1);
    }

    public void HoldingUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isHoldingUp = true;
            FindObjectOfType<Menu>().MoveUp();
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
            FindObjectOfType<Menu>().MoveDown();
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

    private void AnimStop()
    {
        isIncapacitated = true;
    }

    private void AnimGo()
    {
        Physics.IgnoreCollision(characterController, otherGuy.GetComponent<Collider>(), false);
        sliding = false;
        zoomies = false;
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

        if (currentAnimSpeed <= -0.6f && isIncapacitated == false && isGrounded)
        {
            animator.SetTrigger("guard");
            StartCoroutine(IncapacitatedDuration(0.5f));
            return false;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            bool winnerP1 = (isPlayer1) ? false : true ;
            otherGuy.IWon();
            FindObjectOfType<FightMaster>().Winner(winnerP1);

            animator.SetTrigger("death");
        }

        return true;
    }

    public void IWon()
    {
        isDead = true;
        animator.SetTrigger("Win");
    }

    public void Slash()
    {
        zoomies = false;
        GameObject newSlash = Instantiate(slash, slashPos);
        newSlash.transform.localPosition = Vector3.zero;
        Destroy(newSlash, 1);
    }

    public void Menu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FindObjectOfType<Menu>().ChangeMenuState();
        }
    }

    public void SlashGround()
    {
        Vector3 pos = Vector3.zero;
        pos.x += 1.6f;

        GameObject groundslash = Instantiate(slashGround, transform);
        groundslash.transform.localPosition = pos;

        Destroy(groundslash, 1);
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
            knockback_X -= horizontalKnockback;
        }
        else
        {
            knockback_X += horizontalKnockback;
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

    public void GettingSlain()
    {
        StopAllCoroutines();
        animator.SetTrigger("executed");
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

    public void GoThroughDefense()
    {
        currentAnimSpeed = 0;
    }

    public void CheckSlash()
    {
        Collider[] colliders = Physics.OverlapSphere(hitBoxes[0].position, 0.7f);

        bool gotem = false;

        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<CharacterBase>() == otherGuy)
            {
                gotem = true;
                otherGuy.GoThroughDefense();
                Debug.Log("Got em");
                otherGuy.GettingSlain();
            }
        }

        if (gotem == false)
        {
            animator.SetTrigger("missed");
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

    private void OnDrawGizmos()
    {
        if (isDummy)
            return;

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(hitBoxes[0].position, testRange);
    }
}
