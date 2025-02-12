using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header("Base skill")]
    public string skillName;
    public float animationDuration;

    public bool selfInflicted;

    //para implementeación de particulas

    public GameObject effectPrfb;

    protected Fighter emitter;

    protected Fighter receiver;

    //Usado para instanciar la animacion correspondiente y terminarla
    private void Animate()
    {
        var go = Instantiate(this.effectPrfb, this.receiver.transform.position, Quaternion.identity);
        Destroy(go, this.animationDuration);
    }

    //Inicia la accion, la separa en caso de ser autoinflingida o no  
    public void Run()
    {
        if (this.selfInflicted)
        {
            this.receiver = this.emitter;

        }
        this.Animate();

        this.OnRun();

    }
    //Determina quien realiza la accion y quien la recibe 
    public void SetEmitterAndReciver(Fighter _emitter_, Fighter _reciver_)
    {
        this.emitter = _emitter_;
        this.receiver = _reciver_;
            
    }

    protected abstract void OnRun();

}
