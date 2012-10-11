﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mediaportal.TV.Server.TVDatabase.Entities.Enums;

namespace Mediaportal.TV.Server.TVDatabase.Entities.Factories
{
  public static class ProgramFactory
  {

    public static Program Clone (Program source)
    {
      return CloneHelper.DeepCopy<Program>(source);
      /*Program p = CreateProgram( source.idChannel, source.startTime, source.endTime, source.title, source.description, source.ProgramCategory, (ProgramState)source.state,
                              source.originalAirDate,
                              source.seriesNum, source.episodeNum, source.episodeName, source.episodePart, source.starRating, source.classification,
                              source.parentalRating);
      p.idProgram = source.idProgram;*/      
    }


    public static Program CreateProgram(int idChannel, DateTime startTime, DateTime endTime, string title, string description, ProgramCategory category,
                   ProgramState state, DateTime originalAirDate, string seriesNum, string episodeNum, string episodeName,
                   string episodePart, int starRating,
                   string classification, int parentalRating)
    {
      //todo gibman handle genre
      var program = new Program
                      {
                        IdChannel = idChannel,
                        StartTime = startTime,
                        EndTime = endTime,
                        Title = title,
                        Description = description,
                        State = (int)state,
                        OriginalAirDate = originalAirDate,
                        SeriesNum = seriesNum,
                        EpisodeNum = episodeNum,
                        EpisodeName = episodeName,
                        EpisodePart = episodePart,
                        StarRating = starRating,
                        Classification = classification,
                        ParentalRating = parentalRating,
                        ProgramCategory = category
                      };
      return program;
    }

   /*
    public static Program CreateProgram (int idChannel, DateTime startTime, DateTime endTime, string title, string description,
                   ProgramState state, DateTime originalAirDate, string seriesNum, string episodeNum, string episodeName,
                   string episodePart, int starRating,
                   string classification, int parentalRating)
    {
      var program = new Program();
      return program;
     
      this.idChannel = idChannel;
      this.startTime = startTime;
      this.endTime = endTime;
      this.title = title;
      this.description = description;
      this.state = (int)state;
      this.originalAirDate = originalAirDate;
      this.seriesNum = seriesNum;
      this.episodeNum = episodeNum;
      this.episodeName = episodeName;
      this.episodePart = episodePart;
      this.starRating = starRating;
      this.classification = classification;
      this.parentalRating = parentalRating;
    }


    public static Program CreateProgram (int idProgram, int idChannel, DateTime startTime, DateTime endTime, string title, string description,
                   string genre, ProgramState state, DateTime originalAirDate, string seriesNum, string episodeNum,
                   string episodeName, string episodePart,
                   int starRating, string classification, int parentalRating)
    {
      var program = new Program();
      return program; 
      this.idProgram = idProgram;
      this.idChannel = idChannel;
      this.startTime = startTime;
      this.endTime = endTime;
      this.title = title;
      this.description = description;
      this.state = (int)state;
      this.originalAirDate = originalAirDate;
      this.seriesNum = seriesNum;
      this.episodeNum = episodeNum;
      this.episodeName = episodeName;
      this.episodePart = episodePart;
      this.starRating = starRating;
      this.classification = classification;
      this.parentalRating = parentalRating;
    }

    public static Program CreateProgram (int idProgram, int idChannel, DateTime startTime, DateTime endTime, string title, string description,
                   ProgramState state, DateTime originalAirDate, string seriesNum, string episodeNum,
                   string episodeName, string episodePart,
                   int starRating, string classification, int parentalRating)
    {
      var program = new Program();
      return program;
      this.idProgram = idProgram;
      this.idChannel = idChannel;
      this.startTime = startTime;
      this.endTime = endTime;
      this.title = title;
      this.description = description;
      this.state = (int)state;
      this.originalAirDate = originalAirDate;
      this.seriesNum = seriesNum;
      this.episodeNum = episodeNum;
      this.episodeName = episodeName;
      this.episodePart = episodePart;
      this.starRating = starRating;
      this.classification = classification;
      this.parentalRating = parentalRating;
    }
  */
   
     
      
  }
}
