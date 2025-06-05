using UnityEngine;

public class ExampleCharacterScript : MonoBehaviour
{
    public AnimationManager animationManager;

    void Start()
    {
        animationManager.Play("idle");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animationManager.Play("walk");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animationManager.Play("surprised");
        }
    }
}