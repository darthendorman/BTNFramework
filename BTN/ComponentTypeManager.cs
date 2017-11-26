using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BTNFramework.BTN
{
    public static class ComponentTypeManager
    {
        private static IDictionary<Type, int> _ids = new Dictionary<Type, int>();

        private static IDictionary<Type, BigInteger> _bits = new Dictionary<Type,BigInteger>();

        private static int _nextId = 0;

        private static BigInteger _nextBit = new BigInteger(1);

        public static int GetIdFor<T>() where T : IComponent
        {
            int id = -1;

            if (!_ids.TryGetValue(typeof(T), out id)) {
                id = _nextId;
                BigInteger bit = _nextBit;

                _ids.Add(typeof(T), id);
                _bits.Add(typeof(T), bit);

                _nextId++;
                _nextBit <<= 1;
            }
            return id;
        }

        public static BigInteger GetBitFor<T>() where T : IComponent
        {
            BigInteger bit = -1;

            if (!_bits.TryGetValue(typeof(T), out bit)) {
                int id = _nextId;
                bit = _nextBit;

                _ids.Add(typeof(T), id);
                _bits.Add(typeof(T), bit);

                _nextId++;
                _nextBit <<= 1;
            }
            return bit;
        }

        public static BigInteger GetBitFor(Type t)
        {
            BigInteger bit = -1;

            if (!_bits.TryGetValue(t, out bit)) {
                int id = _nextId;
                bit = _nextBit;

                _ids.Add(t, id);
                _bits.Add(t, bit);

                _nextId++;
                _nextBit <<= 1;
            }
            return bit;
        }

        public static int GetIdFor(Type t)
        {
            int id = -1;

            if (!_ids.TryGetValue(t, out id)) {
                id = _nextId;
                BigInteger bit = _nextBit;

                _ids.Add(t, id);
                _bits.Add(t, bit);

                _nextId++;
                _nextBit <<= 1;
            }
            return id;
        }
    }
}
