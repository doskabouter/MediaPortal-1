﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mediaportal.TV.Server.TVControl.Interfaces.Services;
using Mediaportal.TV.Server.TVDatabase.Entities;

namespace Mediaportal.TV.Server.TVService.ServiceAgents
{
  public class CanceledScheduleServiceAgent : ServiceAgent<ICanceledScheduleService>, ICanceledScheduleService
  {
    public CanceledScheduleServiceAgent(string hostname) : base(hostname)
    {
    }

    public CanceledSchedule SaveCanceledSchedule(CanceledSchedule canceledSchedule)
    {
      canceledSchedule.UnloadAllUnchangedRelationsForEntity();
      return _channel.SaveCanceledSchedule(canceledSchedule);
    }
  }
}
