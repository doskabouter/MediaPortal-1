//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace Mediaportal.TV.Server.TVDatabase.Entities
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Schedule))]
    public partial class CanceledSchedule: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int IdCanceledSchedule
        {
            get { return _idCanceledSchedule; }
            set
            {
                if (_idCanceledSchedule != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'IdCanceledSchedule' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _idCanceledSchedule = value;
                    OnPropertyChanged("IdCanceledSchedule");
                }
            }
        }
        private int _idCanceledSchedule;
    
        [DataMember]
        public int IdSchedule
        {
            get { return _idSchedule; }
            set
            {
                if (_idSchedule != value)
                {
                    ChangeTracker.RecordOriginalValue("IdSchedule", _idSchedule);
                    if (!IsDeserializing)
                    {
                        if (Schedule != null && Schedule.IdSchedule != value)
                        {
                            Schedule = null;
                        }
                    }
                    _idSchedule = value;
                    OnPropertyChanged("IdSchedule");
                }
            }
        }
        private int _idSchedule;
    
        [DataMember]
        public int IdChannel
        {
            get { return _idChannel; }
            set
            {
                if (_idChannel != value)
                {
                    _idChannel = value;
                    OnPropertyChanged("IdChannel");
                }
            }
        }
        private int _idChannel;
    
        [DataMember]
        public System.DateTime CancelDateTime
        {
            get { return _cancelDateTime; }
            set
            {
                if (_cancelDateTime != value)
                {
                    _cancelDateTime = value;
                    OnPropertyChanged("CancelDateTime");
                }
            }
        }
        private System.DateTime _cancelDateTime;

        #endregion
        #region Navigation Properties
    
        [DataMember]
        public Schedule Schedule
        {
            get { return _schedule; }
            set
            {
                if (!ReferenceEquals(_schedule, value))
                {
                    var previousValue = _schedule;
                    _schedule = value;
                    FixupSchedule(previousValue);
                    OnNavigationPropertyChanged("Schedule");
                }
            }
        }
        private Schedule _schedule;

        #endregion
        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        // This entity type is the dependent end in at least one association that performs cascade deletes.
        // This event handler will process notifications that occur when the principal end is deleted.
        internal void HandleCascadeDelete(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                this.MarkAsDeleted();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        protected virtual void ClearNavigationProperties()
        {
            Schedule = null;
        }

        #endregion
        #region Association Fixup
    
        private void FixupSchedule(Schedule previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.CanceledSchedules.Contains(this))
            {
                previousValue.CanceledSchedules.Remove(this);
            }
    
            if (Schedule != null)
            {
                if (!Schedule.CanceledSchedules.Contains(this))
                {
                    Schedule.CanceledSchedules.Add(this);
                }
    
                IdSchedule = Schedule.IdSchedule;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Schedule")
                    && (ChangeTracker.OriginalValues["Schedule"] == Schedule))
                {
                    ChangeTracker.OriginalValues.Remove("Schedule");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Schedule", previousValue);
                }
                if (Schedule != null && !Schedule.ChangeTracker.ChangeTrackingEnabled)
                {
                    Schedule.StartTracking();
                }
            }
        }

        #endregion
    }
}
