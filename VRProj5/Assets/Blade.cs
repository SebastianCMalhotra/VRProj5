using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Blade : MonoBehaviour
{
    private float extend_speed = .1f;
    private bool on;
    private float min_scale = 0.0f;
    private float max_scale;
    private float inter_val;
    private float current_scale;
    private float x_base_scale;
    private float z_base_scale;

    public GameObject blade;
    public GameObject light;
    
    public AudioSource aud;
    public AudioClip sfxA;
    public AudioClip sfxB;
        
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        x_base_scale = transform.localScale.x;
        z_base_scale = transform.localScale.z;
        max_scale = transform.localScale.y;
        current_scale = max_scale;
        inter_val = max_scale/extend_speed;
        on = true;
    }

    // Update is called once per frame
    void Update()
    {
        var action = InputSystem.actions["Enable"];

        if (action.WasPressedThisFrame())  
        {
            on = !on;
            if (on)
            {
                aud.PlayOneShot(sfxA);
            }
            else
            {
                aud.PlayOneShot(sfxB);
            }
            
        }
    }

    void FixedUpdate()
    {
        inter_val = on ? -Mathf.Abs(inter_val) : Mathf.Abs(inter_val);
        current_scale += inter_val * Time.deltaTime;
        current_scale = Mathf.Clamp(current_scale, min_scale, max_scale);
        transform.localScale = new Vector3(x_base_scale, current_scale, z_base_scale);

        if (Mathf.Approximately(current_scale, min_scale))
        {
            MeshRenderer renderer = blade.GetComponent<MeshRenderer>();
            renderer.enabled = false;
            Light glow = light.GetComponent<Light>();
            glow.enabled = false;
        }
        else
        {
            MeshRenderer renderer = blade.GetComponent<MeshRenderer>();
            renderer.enabled = true;
            light.SetActive(true);
            Light glow = light.GetComponent<Light>();
            glow.enabled = true;
        }
    }
    
}
