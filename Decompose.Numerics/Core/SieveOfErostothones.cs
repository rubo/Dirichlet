﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Decompose.Numerics
{
    public class SieveOfErostothones : IEnumerable<int>
    {
        private const int initialSize = 1024;
        private Word64BitArray bits;
        int m;
        int n;
        private List<int> primes;

        public SieveOfErostothones()
        {
            bits = new Word64BitArray(initialSize);
            bits[0] = true;
            bits[1] = true;
            m = 0;
            n = initialSize;
            primes = new List<int>();
            primes.Add(2);
            Sieve(2);
        }

        private void NextPrime()
        {
            int p = primes[primes.Count - 1] + 1;
            while (p < m + n && bits[p - m])
                ++p;
            if (p == m + n)
            {
                m += n;
                n *= 2;
                bits = new Word64BitArray(n);
                for (int i = 0; i < primes.Count; i++)
                    Sieve(primes[i]);
                while (p < m + n && bits[p - m])
                    ++p;
            }
            Sieve(p);
            primes.Add(p);
        }

        private void Sieve(int p)
        {
            int q = Math.Max(IntegerMath.Modulus(-m, p), p * p - m);
            for (int i = q; i < n; i += p)
                bits[i] = true;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int n = 0; true; n++)
            {
                if (n == primes.Count)
                    NextPrime();
                yield return primes[n];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
