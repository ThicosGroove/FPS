using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDamage : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float lifeTime = 0.6f;
    [SerializeField] private float minDist = 2f;
    [SerializeField] private float maxDist = 2f;

    private Vector3 _iniPos;
    private Vector3 _targetPos;
    private float _timer;

    private void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);

        var direction = Random.rotation.eulerAngles.z;
        _iniPos = transform.position;
        var dist = Random.Range(minDist, maxDist);
        _targetPos = _iniPos + (Quaternion.Euler(0f, 0f, 0f) * new Vector3(dist, dist, 0f));

        transform.localScale = Vector3.zero;
    }


    private void Update()
    {
        _timer += Time.deltaTime;

        var fraction = lifeTime / 2f;

        if (_timer > lifeTime) Destroy(gameObject);
        else if (_timer > fraction) text.color = Color.Lerp(text.color, Color.clear, (_timer / fraction) / (lifeTime / fraction));


        transform.position = Vector3.Lerp(_iniPos, _targetPos, Mathf.Sin(_timer / lifeTime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(_timer / lifeTime));
    }

    public void SetDamageText(int damage)
    {
        text.text = damage.ToString();
    }
}
