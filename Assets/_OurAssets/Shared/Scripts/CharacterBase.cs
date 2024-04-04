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
    [SerializeField] private bool isDummy;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float runSpeed;

    [SerializeField] private float testX;
    [SerializeField] private float testY;

    private Vector3 playerVelocity;
    private Vector3 moveDirection;

    private float knockback_X;
    private float knockback_Y;

    private bool isGrounded;
    private bool isIncapacitated;
    private bool isDead;

    private bool canAttack = true;
    private CharacterBase otherGuy;

    private int maxHealth = 100;
    private int currentHealth = 100;
    private Image healthBar;

    private int maxMeter = 100;
    private int currentMeter = 100;
    private Image meterBar;
    private TextMeshProUGUI meterBarText;

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

        otherGuy = (isPlayer1) ? GameObject.FindGameObjectWithTag("P2").GetComponent<CharacterBase>() : GameObject.FindGameObjectWithTag("P1").GetComponent<CharacterBase>() ;
    }

    void Update()
    {
        GroundCheck();
        HandleKnockback();
        Gravity();

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

    private void GroundCheck()
    {
        RaycastHit check;

        float distance = 0.2f;

        Vector3 dir = new Vector3(0, -1);

        isGrounded = (Physics.Raycast(transform.position, dir, out check, distance));
        animator.SetBool("grounded", isGrounded);
    }

    private void HandleKnockback()
    {
        if(knockback_X <= 0.6f && knockback_Y <= 0.6f)
        {
            knockback_X = 0;
            knockback_Y = 0;
            return;
        }

        Debug.Log("Moving");

        Vector3 knockbackForward = Vector3.zero;
        knockbackForward.x = 1;
        if (knockback_X <= 0.6f)
            knockbackForward.x = 0;

        characterController.Move(knockbackForward * knockback_X * Time.deltaTime);
        knockback_X -= Time.deltaTime * 3;

        Vector3 knockbackUp = Vector3.zero;
        knockbackUp.y = 1;
        if (knockback_Y <= 0.6f)
            knockbackUp.y = 0;


        characterController.Move(knockbackUp * knockback_Y * Time.deltaTime);
        knockback_Y -= Time.deltaTime * 5;
    }

    private void Gravity()
    {
        playerVelocity.y += -20 * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void Movement()
    {
        Vector2 inputVector = playerInput.Player.Movement.ReadValue<Vector2>();
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        characterController.Move(transform.TransformDirection(moveDirection) * runSpeed * Time.deltaTime);
    }

    private void AirMomentum()
    {
        characterController.Move(transform.TransformDirection(moveDirection) * runSpeed * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isIncapacitated)
            return;

        if (context.performed)
        {
            if (isGrounded)
            {
                //playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * -20);
                ApplyKnockback(testX, testY);
            }
        }
    }

    public bool CanAttack()
    {
        return canAttack;
    }

    public CharacterBase OtherPlayer()
    {
        return otherGuy;
    }

    public void ApplyDamage(int damage)
    {
        if (damage <= 0 || isDead == true)
            return;

        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            isDead = true;
            animator.SetTrigger("death");
        }
    }

    public void ApplyHitStun(bool headShot) // headShot determines their flinch, headshots stun for longer
    {
        StopAllCoroutines();

        string flinchType = (headShot) ? "flinchHead" : "flinchBody" ;
        float flinchDuration = (headShot) ? 0.6f : 0.3f ;

        if (!isGrounded)
        {
            flinchDuration = 0.4f;
        }

        StartCoroutine(IncapacitatedDuration(flinchDuration));
        animator.SetTrigger(flinchType);
    }

    public void ApplyKnockback(float horizontalKnockback, float verticalKnockback)
    {
        knockback_X = horizontalKnockback;
        knockback_Y = verticalKnockback;
    }

    public void ApplyGrab()
    {
        StopAllCoroutines();
        animator.SetTrigger("grab");
        StartCoroutine(IncapacitatedDuration(2));
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

    public bool TryUseMeter(int meterCost)
    {
        if(currentMeter > meterCost)
        {
            currentMeter -= meterCost;
            meterBar.fillAmount = currentMeter / maxMeter;
            return true;
        }

        return false;
    }
}
