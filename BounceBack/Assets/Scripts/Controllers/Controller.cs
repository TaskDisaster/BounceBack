using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour, IController
{
    public Pawn pawn;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (pawn != null)
        {
            PossessPawn(pawn);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (pawn != null)
        {
            ProcessInputs();
        }
    }

    public abstract void ProcessInputs();

    public virtual void PossessPawn(Pawn pawnToPossess)
    {
        // Set our pawn variable to the pawn we want to poassess
        pawn = pawnToPossess;

        // Set the pawn's controller to this controller
        pawn.SetController(this);
    }

    public virtual void UnpossessPawn()
    {
        // Set our pawn's controller to null
        pawn.SetController(null);

        // Set our pawn variable to null
        pawn = null;
    }
}
