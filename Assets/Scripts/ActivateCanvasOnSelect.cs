using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ActivateCanvasOnSelect : MonoBehaviour
{
    public GameObject canvas;  // Asigna el canvas desde el inspector

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
        }
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        canvas.SetActive(true);  // Activa el canvas cuando se agarra el objeto
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        canvas.SetActive(false);  // Desactiva el canvas cuando se suelta el objeto
    }
}