using System;

namespace dojodachi{
    public class Pet{
        public int fullness {
            get; 
            set;
        }

        public int happiness {
            get; 
            set;
        }
        
        public int meals {
            get; 
            set;
        }
        
        public int energy {
            get; 
            set;
        }

        public Pet() {
            fullness = 20;
            happiness = 20;
            meals = 3;
            energy = 50;
        }
    }
}