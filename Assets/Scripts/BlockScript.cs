using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {

    public Sprite obj;
    public Sprite flag;
    public GameObject particle;

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

    private void OnMouseEnter() {
        mouseHere = true;
    }

    private void OnMouseExit() {
        mouseHere = false;
    }
    
    private void RightClick() {
        if (sr.sprite == blank) {
            sr.sprite = flag;

            transform.parent.SendMessage("SetFlag", transform.position);
        }

        else if(sr.sprite == flag) {
            sr.sprite = blank;

            transform.parent.SendMessage("RemoveFlag", transform.position);
        }
    }
    
    public void LeftClick() {
        if (sr.sprite.name != "blank"|| sr.sprite.name == "flag")
            return;

        StartCoroutine(InstantiateParticle());

        sr.sprite = obj;

        if (obj.name == "0") {
            Vector3 middle = new Vector3(0.5f, 0.5f);
            Vector3[] points = new Vector3[8];

            points[0] = transform.position + middle + Vector3.up;
            points[1] = transform.position + middle + Vector3.left;
            points[2] = transform.position + middle + Vector3.right;
            points[3] = transform.position + middle + Vector3.down;
            points[4] = points[0] + Vector3.left;
            points[5] = points[0] + Vector3.right;
            points[6] = points[3] + Vector3.left;
            points[7] = points[3] + Vector3.right;

            for (int i = 0; i < 8; i++) {
                Collider2D n = Physics2D.OverlapCircle(points[i], 0.1f);

                if (n != null) n.gameObject.GetComponent<BlockScript>().LeftClick();
            }
        }

        else if (obj.name == "bomb") {
            sr.color = Color.red;

            transform.parent.SendMessage("GameOver");
        }
    }

    public void SetOcultSprite(Sprite sprite) {
        obj = sprite;
    }

    public void ShowBombBlocks(string s) {
        if (obj.name == "bomb") {
            sr.sprite = obj;

            if (s == "win")
                sr.color = Color.green;
        }
    }

    private IEnumerator InstantiateParticle() {
        GameObject p = Instantiate(particle, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        Destroy(p);
    }
}
