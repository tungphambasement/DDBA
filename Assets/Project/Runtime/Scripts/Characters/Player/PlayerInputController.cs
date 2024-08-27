using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PlayerInputController : MonoBehaviour
{
    PlayerInputAction _playerInputAction;
    [SerializeField] Animator animator;
    [SerializeField] PlayerMovementController playerMovementController;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerCombatController playerCombatController;

    private void OnEnable()
    {
        _playerInputAction = new();
        _playerInputAction.Player.Enable();

        //Move
        _playerInputAction.Player.Move.performed += Movement_Performed;
        _playerInputAction.Player.Move.canceled += Movement_Canceled;

        //Fire
        _playerInputAction.Player.Fire.performed += Fire_Performed;

        //Jump
        _playerInputAction.Player.Jump.started += Jump_Started;
        _playerInputAction.Player.Jump.canceled += Jump_Canceled;

        //Interact
        _playerInputAction.Player.Interact.performed += Interact_Performed;

        //Weapon Out
        _playerInputAction.Player.UseWeapon.performed += UseWeapon_Performed;

        //Boost
        _playerInputAction.Player.Boost.performed += UseBoost_Performed;

        //Slide
        _playerInputAction.Player.Slide.performed += Slide_Performed;

        //Cast
        _playerInputAction.Player.Beam.started += Cast_Started;
        _playerInputAction.Player.Beam.canceled += Cast_Cancelled;
       
    }

    private void Movement_Performed(InputAction.CallbackContext context)
    {
        //Debug.Log("Moving");
        Vector2 movementInput = context.ReadValue<Vector2>();
        playerMovementController.OnMove(movementInput);
    }

    private void Movement_Canceled(InputAction.CallbackContext context)
    {
        playerMovementController.OnMove(new Vector2(0f, 0f));
    }

    private void Fire_Performed(InputAction.CallbackContext context)
    {
        playerCombatController.OnFire();
    }

    private void Interact_Performed(InputAction.CallbackContext context)
    {
        playerController.Interact();
    }


    private void Jump_Started(InputAction.CallbackContext context)
    {
        playerMovementController.OnJump();
    }

    private void Jump_Canceled(InputAction.CallbackContext context){
        playerMovementController.ReleaseJump();
    }

    private void UseWeapon_Performed(InputAction.CallbackContext context)
    {
        playerCombatController.WeaponOut();
    }

    private void Slide_Performed(InputAction.CallbackContext context)
    {
        Debug.Log("Slide Used");
        playerMovementController.UseSlide();
    }

    private void UseBoost_Performed(InputAction.CallbackContext context){
        playerMovementController.UseDash();
    }

    private void Cast_Started(InputAction.CallbackContext context){
        playerCombatController.OnCast_Performed();
    }

    private void Cast_Performed(){

    }

    private void Cast_Cancelled(InputAction.CallbackContext context){
        playerCombatController.OnCast_Released();
    }
}