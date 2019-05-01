using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {

    public Sprite obj;
    public Sprite flag;
    
    private SpriteRenderer sr;
    private Sprite blank;

    private bool mouseHere;

    private void Start() {
        sr = gameObject.GetComponent<SpriteRenderer>();

        blank = sr.sprite;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && mouseHere) LeftClick();

        if (Input.GetMouseButtonDown(1) && mouseHere) RightClick();
    }

    private void OnMouseEnter() { mouseHere = true; }

    private void OnMouseExit() { mouseHere = false; }
    
    private void RightClick() {
        if (sr.sprite == blank) {
            sr.sprite = flag;

            transform.parent.SendMessage("SetFlag", transform.position);
        }

        else {
            sr.sprite = blank;

            transform.parent.SendMessage("RemoveFlag", transform.position);
        }
    }

    private void LeftClick() { sr.sprite = obj; }

    public void SetOcultSprite(Sprite sprite) { obj = sprite; }
}
