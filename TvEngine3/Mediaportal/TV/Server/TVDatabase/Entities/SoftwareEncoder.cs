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
using System.Collections;
using Gentle.Framework;
using TvLibrary.Log;

namespace TvDatabase
{
  /// <summary>
  /// Instances of this class represent the properties and methods of a row in the table <b>SoftwareEncoder</b>.
  /// </summary>
  [TableName("SoftwareEncoder")]
  public class SoftwareEncoder : Persistent
  {
    #region Members

    private bool isChanged;
    [TableColumn("idEncoder", NotNull = true), PrimaryKey(AutoGenerated = true)] private int idEncoder;
    [TableColumn("priority", NotNull = true)] private int priority;
    [TableColumn("name", NotNull = true)] private string name;
    [TableColumn("type", NotNull = true)] private int type;
    [TableColumn("reusable", NotNull = true)] private bool reusable;

    #endregion

    #region Constructors

    /// <summary> 
    /// Create a new object by specifying all fields (except the auto-generated primary key field). 
    /// </summary> 
    public SoftwareEncoder(int priority, string name, int type, bool reusable)
    {
      isChanged = true;
      this.priority = priority;
      this.name = name;
      this.type = type;
      this.reusable = reusable;
    }

    /// <summary> 
    /// Create an object from an existing row of data. This will be used by Gentle to 
    /// construct objects from retrieved rows.
    /// </summary> 
    public SoftwareEncoder(int idEncoder, int priority, string name, int type, bool reusable)
    {
      this.idEncoder = idEncoder;
      this.priority = priority;
      this.name = name;
      this.type = type;
      this.reusable = reusable;
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
    /// Property relating to database column idEncoder
    /// </summary>
    public int IdEncoder
    {
      get { return idEncoder; }
    }

    /// <summary>
    /// Property relating to database column priority
    /// </summary>
    public int Priority
    {
      get { return priority; }
      set
      {
        isChanged |= priority != value;
        priority = value;
      }
    }

    /// <summary>
    /// Property relating to database column name
    /// </summary>
    public string Name
    {
      get { return name; }
      set
      {
        isChanged |= name != value;
        name = value;
      }
    }

    /// <summary>
    /// Property relating to database column type
    /// </summary>
    public int Type
    {
      get { return type; }
      set
      {
        isChanged |= type != value;
        type = value;
      }
    }

    /// <summary>
    /// Property relating to database column reusable
    /// </summary>
    public bool Reusable
    {
      get { return reusable; }
      set
      {
        isChanged |= reusable != value;
        reusable = value;
      }
    }

    #endregion

    #region Storage and Retrieval

    /// <summary>
    /// Static method to retrieve all instances that are stored in the database in one call
    /// </summary>
    public static IList ListAll()
    {
      return Broker.RetrieveList(typeof (SoftwareEncoder));
    }

    /// <summary>
    /// Retrieves an entity given it's id.
    /// </summary>
    public static SoftwareEncoder Retrieve(int id)
    {
      // Return null if id is smaller than seed and/or increment for autokey
      if (id < 1)
      {
        return null;
      }
      Key key = new Key(typeof (SoftwareEncoder), true, "idEncoder", id);
      return Broker.RetrieveInstance<SoftwareEncoder>(key);
    }

    /// <summary>
    /// Retrieves an entity given it's id, using Gentle.Framework.Key class.
    /// This allows retrieval based on multi-column keys.
    /// </summary>
    public static SoftwareEncoder Retrieve(Key key)
    {
      return Broker.RetrieveInstance<SoftwareEncoder>(key);
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
          Log.Error("Exception in SoftwareEncoder.Persist() with Message {0}", ex.Message);
          return;
        }
        isChanged = false;
      }
    }

    #endregion
  }
}