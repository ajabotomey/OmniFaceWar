// Taken from https://medium.com/@kunaltandon.kt/creating-drop-shadows-for-sprites-in-unity-6415d2b2b9cb on 4/11/2019
// Adapted by AJ Abotomey

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DropShadowSprite : MonoBehaviour
{
    [SerializeField] private Vector2 shadowOffset;
    [SerializeField] private Material shadowMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private GameObject shadow;

    // Start is called before the first frame update
    void Start()
    {
        // Create the game object
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform.parent;
        shadow.transform.localPosition = transform.localPosition;
        shadow.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

        // Create new sprite renderer
        SpriteRenderer shadowRenderer = shadow.AddComponent<SpriteRenderer>();

        // Set the shadow gameobject sprite to the original sprite
        shadowRenderer.sprite = spriteRenderer.sprite;

        // Set the shadow gameobject material to the shadow material
        shadowRenderer.material = shadowMaterial;

        // Update the sorting layer of the shadow to always lie behind the sprite
        shadowRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        shadowRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Update the position and rotation of the sprite's shadow with moving sprite
        shadow.transform.localPosition = transform.localPosition + (Vector3)shadowOffset;
    }
}
