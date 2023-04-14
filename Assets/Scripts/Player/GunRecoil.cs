using UnityEngine;


public class GunRecoil : MonoBehaviour
{
    [Header("Hipfire Recoil")]
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [Header("ADS Recoil")]
    [SerializeField] private float aimRecoilX;
    [SerializeField] private float aimRecoilY;
    [SerializeField] private float aimRecoilZ;

    [Header("Settings")]
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    InputManager input;
    GunControl gunControl;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private void Start()
    {
        input = InputManager.Instance;
        gunControl = GetComponentInChildren<GunControl>();
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);

        if (input.PlayerShootThisFrame())
        {
            //RecoilFire();
        }
    }

    //private void RecoilFire()
    //{
    //    if (gunControl.zoomPressed)
    //        targetRotation += new Vector3(aimRecoilX, Random.Range(-aimRecoilY, aimRecoilY), Random.Range(-aimRecoilZ, aimRecoilZ));
    //    else
    //        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    //}
}
