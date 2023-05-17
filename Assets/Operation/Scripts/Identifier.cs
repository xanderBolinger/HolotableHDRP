using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace Operation {
    public class Identifier
    {
        private static string ALPHA_NUMERIC_STRING = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GenerateIdentifier()
        {
            int count = 10;
            StringBuilder builder = new StringBuilder();

            System.Random random = new System.Random();

            while (count-- != 0)
            {
                int character = random.Next(0, ALPHA_NUMERIC_STRING.Length);
                builder.Append(ALPHA_NUMERIC_STRING[character]);
            }

            return builder.ToString();
        }

        public static bool CompareTo(Unit a, Unit b)
        {
            return a.identifier == b.identifier;
        }
        public static bool CompareTo(Trooper a, Trooper b)
        {
            return a.identifier == b.identifier;
        }
        public static bool CompareTo(Vehicle a, Vehicle b)
        {
            return a.identifier == b.identifier;
        }

    }
}


