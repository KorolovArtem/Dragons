using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private bool scrollLeft = true;

    private float singleTextureWidth;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;

        SetupTexture();

        if (scrollLeft) moveSpeed = -moveSpeed;
    }
    private void SetupTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    private void Scroll()
    {
        float delta = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta, 0f, 0f);
    }

    private void CheckReset()
    {
        if (Mathf.Abs(transform.position.x - startPosition.x) >= singleTextureWidth)
        {
            transform.position = startPosition;
        }
    }

    private void Update()
    {
        Scroll();
        CheckReset();
    }
}
