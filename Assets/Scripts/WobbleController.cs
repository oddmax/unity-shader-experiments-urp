using UnityEngine;

public class WobbleController : MonoBehaviour
{
    private const string ShaderWobbleXReference = "_WobbleX";
    private const string ShaderWobbleZReference = "_WobbleZ";

    [SerializeField]
    private Renderer wobbleRenderer;
    
    [SerializeField]
    private float MaxWobble = 0.03f;
    
    [SerializeField]
    private float WobbleSpeed = 1f;
    
    [SerializeField]
    private float Recovery = 1f;
    
    private Vector3 lastPos;
    private Vector3 velocity;
    private Vector3 lastRot;  
    private Vector3 angularVelocity;
   
    private float wobbleAmountX;
    private float wobbleAmountZ;
    private float wobbleAmountToAddX;
    private float wobbleAmountToAddZ;
    private float pulse;
    private float time = 0.5f;
    
    void Start()
    {
        wobbleRenderer = GetComponent<Renderer>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        // send it to the shader
        wobbleRenderer.material.SetFloat(ShaderWobbleXReference, wobbleAmountX);
        wobbleRenderer.material.SetFloat(ShaderWobbleZReference, wobbleAmountZ);

        // velocity
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;
        
        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }

}
