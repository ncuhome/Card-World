using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState { idle, walk, work }
public class Character : MonoBehaviour
{
    public CharacterState characterState = CharacterState.idle;
    public Vector3 targetEuler;
    public bool isCharacter = false;
    public float turnSpeed = 1f;
    private Quaternion targetQua;
    private Quaternion oldQua;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isCharacter)
        {
            return;
        }

        switch (characterState)
        {
            case CharacterState.idle:
                do
                {
                    targetEuler = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                }
                while (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(targetEuler), ColorSystem.Instance.colors[0]) < 0.1f);
                turnSpeed = 0.1f;
                characterState = CharacterState.walk;
                break;
            case CharacterState.walk:
                targetQua = Quaternion.Euler(targetEuler);
                //  用 slerp 进行插值平滑的旋转
                transform.rotation = Quaternion.Slerp(transform.rotation, targetQua, turnSpeed * Time.deltaTime);
                turnSpeed = Mathf.Lerp(turnSpeed, 1, Time.deltaTime * 0.1f);
                // 当初始角度跟目标角度小于1,将目标角度赋值给初始角度,让旋转角度是我们需要的角度
                if (Quaternion.Angle(targetQua, transform.rotation) < 1)
                {
                    transform.rotation = targetQua;
                    characterState = CharacterState.idle;
                }
                if (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(this.transform.up), ColorSystem.Instance.colors[0]) < 0.1f)
                {
                    characterState = CharacterState.idle;
                }
                break;
            case CharacterState.work:
                break;
        }

    }
}
