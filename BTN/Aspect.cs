using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BTNFramework.BTN
{
    public class Aspect
    {
        public BigInteger Mask;

        public Aspect(params Type[] types)
        {
            Mask = new BigInteger(0);
            foreach (Type element in types) {
                Mask += ComponentTypeManager.GetBitFor(element);
            }
        }

        public bool Intersects(Aspect mask)
        {
            return (!Mask.IsZero && BigInteger.Compare(Mask & mask.Mask, Mask) == 0);
        }
    }
}
