using System.Collections;
using UnityEngine;

namespace Holographic
{
    public class MaterialChange : MonoBehaviour
    {
        [SerializeField] private GameObject _model;

        [SerializeField] private Material _mat1, _mat2, _mat3;

        [SerializeField] private float _dissolveTime;
        [SerializeField] private float _dissolveSpeed;
        [SerializeField] private float _xAngle, _yAngle, _zAngle;

        private bool isFlickering = false;

        void Start()
        {
            _model.GetComponent<MeshRenderer>().material = _mat1;
        }

        void Update()
        {
            //Rotate inself
            _model.transform.Rotate(_xAngle, _yAngle, _zAngle, Space.World);

            //Dissolve Shader
            if (_dissolveTime > -1)
            {
                _dissolveTime -= _dissolveSpeed * Time.deltaTime;
                _mat1.SetFloat("_Dissolve_Amount", _dissolveTime);
            }

            // After Dissolve
            if (_dissolveTime < -1 && isFlickering == false)
            {
                StartCoroutine(ChangeMaterial());
            }
        }

        IEnumerator ChangeMaterial()
        {
            isFlickering = true;
            _model.GetComponent<MeshRenderer>().material = _mat2;
            yield return new WaitForSeconds(0.7f);
            _model.GetComponent<MeshRenderer>().material = _mat3;
        }
    }
}