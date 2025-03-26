using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using UnityEngine.UI;

public class ButtonColliderMaker : MonoBehaviour
{
    public GameObject colliderPrefab;
    private BoxCollider boxCollider;
    private RectTransform rectTransform;
    private RayInteractable rayInteractable;

    void Start()
    {
        boxCollider = gameObject.AddComponent<BoxCollider>();
        rectTransform = GetComponent<RectTransform>();
        boxCollider.size = rectTransform.sizeDelta;
        transform.Translate(Vector3.back * 0.01f);

        GameObject collider = Instantiate(colliderPrefab, transform);
        BoxCollider boxColider = collider.AddComponent<BoxCollider>();
        boxColider.size = rectTransform.sizeDelta;
        ColliderSurface surface = collider.AddComponent<ColliderSurface>();
        surface.InjectCollider(boxColider);

        rayInteractable = gameObject.AddComponent<RayInteractable>();
        rayInteractable.InjectSurface(surface);
    }

    private void Update()
    {
        if (rayInteractable.State == InteractableState.Select)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
}
