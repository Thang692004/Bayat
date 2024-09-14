using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask Ground;
    private bool isGround = false;
    [SerializeField] private float speed;
    private Vector3 move = Vector3.zero; // Initialize move
    private string currentAnim;
    [SerializeField] private float jumpForce;
    private bool isJumping = false;
    public int coin = 0;
    [SerializeField] private TextMeshProUGUI CoinText;
    [SerializeField] private Transform resetPoint;
   
    void Start()
    {
        move = Vector3.zero; // Initialize move vector to zero in Start
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        isGround = checkGround();

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJumping = true;
            ChangeAnim("Jumping");
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        }

        // Falling logic
        if (!isGround && rb.velocity.y < 0)
        {
            ChangeAnim("Fall");
            isJumping = false;
        }

        // Horizontal movement
        move.x = Input.GetAxisRaw("Horizontal");
        transform.position += move * Time.deltaTime * speed;

        // Running animation
        if (move.x != 0 && isGround)
        {
            ChangeAnim("Run");
            transform.localScale = new Vector3(Mathf.Sign(move.x) * 1.5f, 1.5f, 1); // Adjust localScale to face the right direction
        }

        // Idle animation
        if (move.x == 0 && isGround && !isJumping)
        {
            ChangeAnim("Idle");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coin++;
            CoinText.SetText(coin.ToString());
            Destroy(other.gameObject);
        }
        if(other.CompareTag("Water"))
        {
            transform.position = resetPoint.position;
        }
    }

    private bool checkGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, Ground);
        return hit.collider != null;
    }

    private void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
}
