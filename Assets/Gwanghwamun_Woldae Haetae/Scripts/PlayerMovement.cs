using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // 이동 속도
    public float rotateSpeed = 50f; // 회전 속도

    private Vector2 moveInput; // 이동 입력 값
    private Vector2 rotateInput; // 회전 입력 값

    void Update()
    {
        // 키보드 입력을 통한 이동
        Vector3 forwardMovement = transform.forward * moveInput.y * moveSpeed * Time.deltaTime;
        Vector3 sideMovement = transform.right * moveInput.x * moveSpeed * Time.deltaTime;
        transform.position += forwardMovement + sideMovement;

        // 마우스를 이용한 회전
        float yaw = rotateInput.x * rotateSpeed * Time.deltaTime;
        float pitch = -rotateInput.y * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, yaw, 0);
        Camera.main.transform.Rotate(pitch, 0, 0);
    }

    // InputSystem과 연결할 함수
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        rotateInput = value.Get<Vector2>();
    }
}
