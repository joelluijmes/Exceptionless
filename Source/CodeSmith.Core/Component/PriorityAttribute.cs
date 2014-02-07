﻿using System;

namespace CodeSmith.Core.Component {
    /// <summary>
    /// Used to determine action priority.
    /// </summary>
    public class PriorityAttribute : Attribute {
        public PriorityAttribute(int priority) {
            Priority = priority;
        }

        public int Priority { get; private set; }
    }
}
