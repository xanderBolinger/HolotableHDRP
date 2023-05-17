using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operation {
    public class Trooper
    {
        public string identifier;
        public string name;
        public int sl;

        public Trooper(string name, int sl)
        {
            identifier = Identifier.GenerateIdentifier();
            this.name = name;
            this.sl = sl;
        }

    }
}


