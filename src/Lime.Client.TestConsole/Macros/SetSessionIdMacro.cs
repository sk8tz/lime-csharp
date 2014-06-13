﻿using Lime.Client.TestConsole.ViewModels;
using Lime.Protocol;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lime.Client.TestConsole.Macros
{
    [Macro(Name = "Set session id", Category = "Session", IsActiveByDefault = true)]
    public class SetSessionIdMacro : IMacro
    {
        #region IMacro Members

        public Task ProcessAsync(EnvelopeViewModel envelopeViewModel, SessionViewModel sessionViewModel)
        {
            if (envelopeViewModel == null)
            {
                throw new ArgumentNullException("envelopeViewModel");
            }

            if (sessionViewModel == null)
            {
                throw new ArgumentNullException("sessionViewModel");
            }

            var session = envelopeViewModel.Envelope as Session;

            if (session != null &&
                session.Id != Guid.Empty)
            {
                var sessionIdVariableViewModel = sessionViewModel
                    .Variables
                    .FirstOrDefault(v => v.Name.Equals("sessionId"));

                if (sessionIdVariableViewModel == null)
                {
                    sessionIdVariableViewModel = new VariableViewModel()
                    {
                        Name = "sessionId"
                    };

                    sessionViewModel.Variables.Add(sessionIdVariableViewModel);
                }

                sessionIdVariableViewModel.Value = session.Id.ToString();
            }

            return Task.FromResult<object>(null);
        }

        #endregion
    }
}