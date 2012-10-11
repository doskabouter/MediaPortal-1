﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mediaportal.TV.Server.TVDatabase.Entities;

namespace Mediaportal.TV.Server.RuleBasedScheduler.ScheduleConditions
{
  [Serializable]
  public class OnlyRecordNewTitlesCondition : IScheduleCondition
  {    
    private readonly IList<string> _skipTitles;

    public OnlyRecordNewTitlesCondition(IList<string> skipTitles)
    {
      _skipTitles = skipTitles;
    }
    public OnlyRecordNewTitlesCondition()
    {
    }

    public IQueryable<Program> ApplyCondition(IQueryable<Program> baseQuery)
    {
      return baseQuery.Where(program => !(_skipTitles.Any(t => program.Title.ToUpperInvariant().Equals(t.ToUpperInvariant()))));
    }
   
  }
}
