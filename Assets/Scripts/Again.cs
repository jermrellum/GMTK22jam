using UnityEngine;
using UnityEngine.SceneManagement;

public class Again : MonoBehaviour
{
    [SerializeField] private Material normal;
    [SerializeField] private Material hilite;

    private Renderer rend;
    private readonly CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new(6.0f, 0.0f);
    public Texture2D cursorTexture;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        rend.material = hilite;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void OnMouseExit()
    {
        rend.material = normal;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    private void OnMouseDown()
    {
        GameObject brgo = GameObject.Find("BulletRememberer");
        Destroy(brgo);
        Cursor.SetCursor(null, Vector2.zero, cursorMode);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
