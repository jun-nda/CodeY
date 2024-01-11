using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParticlePoint
{
    
}

public struct Spring
{
    
}

public abstract class SoftBody
{
    public List<ParticlePoint> m_ParticlePoints;
    public List<Spring> m_Springs;

    public virtual void InitShape()
    {
        
    }

}

public class SoftBodyCircle : SoftBody
{
    public override void InitShape()
    {
        
    }
}

public class SoftBodyQuad : SoftBody
{
    public override void InitShape()
    {
        
    }
}
