#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using Gentle.Framework;
using TvLibrary.Log;

namespace TvDatabase
{
  /// <summary>
  /// Instances of this class represent the properties and methods of a row in the table <b>Server</b>.
  /// </summary>
  [TableName("Satellite")]
  public class Satellite : Persistent
  {
    #region Members

    private bool isChanged;
    [TableColumn("idSatellite", NotNull = true), PrimaryKey(AutoGenerated = true)] private int idSatellite;
    [TableColumn("satelliteName", NotNull = true)] private string satelliteName;
    [TableColumn("transponderFileName", NotNull = true)] private string transponderFileName;

    #endregion

    #region Constructors

    /// <summary> 
    /// Create a new object by specifying all fields (except the auto-generated primary key field). 
    /// </summary> 
    public Satellite(string satelliteName, string transponderFileName)
    {
      isChanged = true;
      this.satelliteName = satelliteName;
      this.transponderFileName = transponderFileName;
    }

    /// <summary> 
    /// Create an object from an existing row of data. This will be used by Gentle to 
    /// construct objects from retrieved rows.
    /// </summary> 
    public Satellite(int idSatellite, string satelliteName, string transponderFileName)
    {
      this.idSatellite = idSatellite;
      this.satelliteName = satelliteName;
      this.transponderFileName = transponderFileName;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Indicates whether the entity is changed and requires saving or not.
    /// </summary>
    public bool IsChanged
    {
      get { return isChanged; }
    }

    /// <summary>
    /// Property relating to database column idServer
    /// </summary>
    public int IdSatellite
    {
      get { return idSatellite; }
    }

    /// <summary>
    /// Property relating to database column hostName
    /// </summary>
    public string SatelliteName
    {
      get { return satelliteName; }
      set
      {
        isChanged |= satelliteName != value;
        satelliteName = value;
      }
    }

    /// <summary>
    /// Property relating to database column hostName
    /// </summary>
    public string TransponderFileName
    {
      get { return transponderFileName; }
      set
      {
        isChanged |= transponderFileName != value;
        transponderFileName = value;
      }
    }

    #endregion

    #region Storage and Retrieval

    /// <summary>
    /// Static method to retrieve all instances that are stored in the database in one call
    /// </summary>
    public static IList<Satellite> ListAll()
    {
      return Broker.RetrieveList<Satellite>();
    }

    /// <summary>
    /// Retrieves an entity given it's id.
    /// </summary>
    public static Satellite Retrieve(int id)
    {
      // Return null if id is smaller than seed and/or increment for autokey
      if (id < 1)
      {
        return null;
      }
      Key key = new Key(typeof (Satellite), true, "idSatellite", id);
      return Broker.RetrieveInstance<Satellite>(key);
    }

    /// <summary>
    /// Retrieves an entity given it's id, using Gentle.Framework.Key class.
    /// This allows retrieval based on multi-column keys.
    /// </summary>
    public static Satellite Retrieve(Key key)
    {
      return Broker.RetrieveInstance<Satellite>(key);
    }

    /// <summary>
    /// Persists the entity if it was never persisted or was changed.
    /// </summary>
    public override void Persist()
    {
      if (IsChanged || !IsPersisted)
      {
        try
        {
          base.Persist();
        }
        catch (Exception ex)
        {
          Log.Error("Exception in Satellite.Persist() with Message {0}", ex.Message);
          return;
        }
        isChanged = false;
      }
    }

    #endregion
  }
}