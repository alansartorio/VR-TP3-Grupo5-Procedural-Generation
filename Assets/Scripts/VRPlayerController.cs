using UnityEngine;
using UnityEngine.XR;

public class VRPlayerController : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float jumpForce = 5.0f;
    public Transform vrCamera; // Asigna la cámara de VR (la cabeza del jugador)

    private CharacterController characterController;
    private bool isGrounded;
    private float verticalVelocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Mover al jugador en la dirección de la mirada de la cámara de VR
        Vector3 moveDirection = vrCamera.TransformDirection(Vector3.forward);
        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);

        // Verificar si el jugador está en el suelo
        isGrounded = characterController.isGrounded;

        // Saltar si se pulsa el botón de salto en los controladores de VR
        if (isGrounded && Input.GetButtonDown("JumpVR"))
        {
            verticalVelocity = jumpForce;
        }

        // Aplicar la gravedad
        if (!isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // Aplicar la velocidad vertical
        Vector3 jumpVector = Vector3.up * verticalVelocity;
        characterController.Move(jumpVector * Time.deltaTime);
    }
}
