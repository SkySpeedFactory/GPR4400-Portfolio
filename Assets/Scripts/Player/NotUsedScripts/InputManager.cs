//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class InputManager : MonoBehaviour
//{
//    [SerializeField] Movement movement;
//    [SerializeField] MouseLook mouseLook;
//
//    PlayerControls controls;
//    PlayerControls.PlayerActions player;
//
//    Vector2 horizontalInput;
//    Vector2 mouseInput;
//    private void Awake()
//    {
//        controls = new PlayerControls();
//        player = controls.Player;
//
//        player.Movement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
//        player.Jump.performed += _ => movement.OnJumpPressed();
//
//        player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
//        player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
//    }
//    private void Update()
//    {
//        movement.ReceiveInput(horizontalInput);
//        mouseLook.ReceiveInput(mouseInput);
//    }
//    private void OnEnable()
//    {
//        controls.Enable();
//    }
//    private void OnDestroy()
//    {
//        controls.Disable();
//    }
//}

//player.[action].performed += context => do something
