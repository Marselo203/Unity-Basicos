using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento horizontal
    public float maxJumpForce = 15f; // Fuerza de salto máxima
    public float initialJumpForce = 7.5f; // Fuerza de salto inicial
    public float maxJumpTime = 0.5f; // Tiempo máximo para seguir aplicando fuerza de salto
    public float fallMultiplier = 2.5f; // Multiplicador para acelerar la caída

    private Rigidbody rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movimiento horizontal
        float moveInputX = Input.GetAxis("Horizontal"); // Teclas A/D o flechas izquierda/derecha
        Vector3 move = new Vector3(moveInputX * moveSpeed, rb.velocity.y, 0);
        rb.velocity = new Vector3(move.x, rb.velocity.y, 0);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = 0f;
            rb.velocity = new Vector3(rb.velocity.x, initialJumpForce, 0);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter < maxJumpTime)
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(initialJumpForce, maxJumpForce, jumpTimeCounter / maxJumpTime), 0);
                jumpTimeCounter += Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        // Aumenta la gravedad durante la caída solo cuando se suelta la tecla Space
        if (!isGrounded && !isJumping && rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto está en el suelo
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false; // Resetea el estado de salto al tocar el suelo
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Verifica si el objeto ya no está en contacto con el suelo
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
