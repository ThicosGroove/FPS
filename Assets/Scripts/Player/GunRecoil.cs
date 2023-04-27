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

    InputManager _input;
    GunControl gunControl;

    private Vector3 _currentRotation;
    private Vector3 _targetRotation;

    private void Start()
    {
        _input = InputManager.Instance;
        gunControl = GetComponentInChildren<GunControl>();
    }

    void Update()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);

        if (_input.IsShootGoOff)
        {
            //RecoilFire();
        }
    }

    // Trocar por anmimação

    //private void RecoilFire()
    //{
    //    if (gunControl.zoomPressed)
    //        targetRotation += new Vector3(aimRecoilX, Random.Range(-aimRecoilY, aimRecoilY), Random.Range(-aimRecoilZ, aimRecoilZ));
    //    else
    //        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    //}
}
