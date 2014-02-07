﻿#region Copyright 2014 Exceptionless

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// 
//     http://www.apache.org/licenses/LICENSE-2.0

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exceptionless.Mvc {
    public class ExceptionlessModule : IHttpModule {
        private HttpApplication _context;

        public void Dispose() {
            ExceptionlessClient.Current.Shutdown();
            _context.Error -= OnError;
        }

        public virtual void Init(HttpApplication context) {
            ExceptionlessClient.Current.LastErrorIdManager = new WebLastErrorIdManager(ExceptionlessClient.Current);
            ExceptionlessClient.Current.RegisterPlugin(new ExceptionlessMvcPlugin());
            ExceptionlessClient.Current.Startup();
            ExceptionlessClient.Current.Configuration.IncludePrivateInformation = true;
            _context = context;
            _context.Error += OnError;

            ReplaceErrorHandler();
        }

        private void ReplaceErrorHandler() {
            Filter filter = GlobalFilters.Filters.FirstOrDefault(f => f.Instance is IExceptionFilter);
            var handler = new ExceptionlessHandleErrorAttribute();

            if (filter != null) {
                if (filter.Instance is ExceptionlessHandleErrorAttribute)
                    return;

                GlobalFilters.Filters.Remove(filter.Instance);

                handler.WrappedHandler = (IExceptionFilter)filter.Instance;
            }

            GlobalFilters.Filters.Add(handler);
        }

        private void OnError(object sender, EventArgs e) {
            HttpContext context = HttpContext.Current;
            if (context == null)
                return;

            Exception exception = context.Server.GetLastError();
            if (exception == null)
                return;

            var contextData = new Dictionary<string, object> {
                { "HttpContext", new HttpContextWrapper(context) }
            };
            ExceptionlessClient.Current.ProcessUnhandledException(exception, "HttpApplicationError", true, contextData);
        }
    }
}