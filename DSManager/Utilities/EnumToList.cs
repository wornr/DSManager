using System;
using System.Collections.Generic;
using System.Linq;

namespace DSManager.Utilities {
    public class EnumToList<T> {
        public List<T> Enums { get; set; }

        public EnumToList() {
            Enums = new List<T>();

            foreach (var singleEnum in Enum.GetValues(typeof(T)).Cast<T>()) {
                Enums.Add(singleEnum);
            }
        }
    }
}