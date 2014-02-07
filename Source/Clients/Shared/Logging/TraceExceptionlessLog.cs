﻿#region Copyright 2014 Exceptionless

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// 
//     http://www.apache.org/licenses/LICENSE-2.0

#endregion

using System;
using System.Diagnostics;

namespace Exceptionless.Logging {
    public class TraceExceptionlessLog : IExceptionlessLog {
        public void Error(string message, string source = null, Exception exception = null) {
            System.Diagnostics.Trace.TraceError(message);
        }

        public void Info(string message, string source = null) {
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public void Debug(string message, string source = null) {
            if (Debugger.IsAttached)
                System.Diagnostics.Trace.WriteLine(message);
        }

        public void Warn(string message, string source = null) {
            System.Diagnostics.Trace.TraceWarning(message);
        }

        public void Trace(string message, string source = null) {
            System.Diagnostics.Trace.WriteLine(message);
        }

        public void Flush() {}
    }
}