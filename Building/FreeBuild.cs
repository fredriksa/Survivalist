using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeBuild : BuildMode {

    public FreeBuild() 
        : base()
    {
        canRotate = true;
        canAlignRotation = true;
    }

	public override void update () {
        base.update();
	}
}
