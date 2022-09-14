using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movespeed = 5f; //�յ� �������� �ӵ�
    public float rotateSpeed = 180f; // �¿� ȸ�� �ӵ�

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ

    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    
    void Start() // ����� ������Ʈ���� ���� ��������
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    //FixedUpdate�� ���� ���� �ֱ⿡ ���� �����
    private void FixedUpdate() //���� ���� �ֱ⸶�� ������, ȸ��, �ִϸ��̼� ó�� ����
    {
        Move();
        Rotate();
    }
    
     private void Move() // �Է°��� ���� ĳ���͸� �յڷ� ������
    {

    } 
    
    private void Rotate() // �Է°��� ���� ĳ���͸� �¿�� ȸ��
    {

    }
}
