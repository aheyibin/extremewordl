using Common.Data;
using Managers;
using Models;
using System.Collections;
using UnityEngine;

public class NpcController : MonoBehaviour {

	public int NPCid;
	SkinnedMeshRenderer renderer;
	Animator animator;
	Color originColor;

	public bool inInteractive = false;

	NpcDefine npcDefine;
	// Use this for initialization
	void Start () {
		renderer = GetComponentInChildren<SkinnedMeshRenderer>();
		animator = GetComponentInChildren<Animator>();
		originColor = renderer.sharedMaterial.color;
		npcDefine = NpcManager.Instance.GetNpcDefine(NPCid);
		this.StartCoroutine(Actions());
	}

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
				yield return new WaitForSeconds(2f);
            }
            else
            {
				yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
			this.Relax();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

	void Relax()
    {
		animator.SetTrigger("Relax");
    }

	void Interactive()
    {
        if (!inInteractive)
        {
			inInteractive = true;
			StartCoroutine(DoInteractive());
        }
    }
	IEnumerator DoInteractive()
    {
		yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(NPCid))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward,faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    void OnMouseDown()
    {
        Interactive();
    }
    private void OnMouseOver()
    {
        HighLight(true);
    }
    private void OnMouseEnter()
    {
        HighLight(true);
    }
    private void OnMouseExit()
    {
        HighLight(false);
    }
    void HighLight(bool hl)
    {
        if (hl)
        {
            if (renderer.sharedMaterial.color!=Color.white)
            {
                renderer.sharedMaterial.color = Color.white;
            }

        }
        else
        {
            if (renderer.sharedMaterial.color != originColor)
            {
                renderer.sharedMaterial.color = originColor;
            }
        }
    }
}
