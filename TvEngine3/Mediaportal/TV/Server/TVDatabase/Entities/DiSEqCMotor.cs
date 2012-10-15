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
    [KnownType(typeof(Card))]
    [KnownType(typeof(Satellite))]
    public partial class DisEqcMotor: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int IdDiSEqCMotor
        {
            get { return _idDiSEqCMotor; }
            set
            {
                if (_idDiSEqCMotor != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'IdDiSEqCMotor' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _idDiSEqCMotor = value;
                    OnPropertyChanged("IdDiSEqCMotor");
                }
            }
        }
        private int _idDiSEqCMotor;
    
        [DataMember]
        public int IdCard
        {
            get { return _idCard; }
            set
            {
                if (_idCard != value)
                {
                    ChangeTracker.RecordOriginalValue("IdCard", _idCard);
                    if (!IsDeserializing)
                    {
                        if (Card != null && Card.IdCard != value)
                        {
                            Card = null;
                        }
                    }
                    _idCard = value;
                    OnPropertyChanged("IdCard");
                }
            }
        }
        private int _idCard;
    
        [DataMember]
        public int IdSatellite
        {
            get { return _idSatellite; }
            set
            {
                if (_idSatellite != value)
                {
                    ChangeTracker.RecordOriginalValue("IdSatellite", _idSatellite);
                    if (!IsDeserializing)
                    {
                        if (Satellite != null && Satellite.IdSatellite != value)
                        {
                            Satellite = null;
                        }
                    }
                    _idSatellite = value;
                    OnPropertyChanged("IdSatellite");
                }
            }
        }
        private int _idSatellite;
    
        [DataMember]
        public int Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged("Position");
                }
            }
        }
        private int _position;

        #endregion
        #region Navigation Properties
    
        [DataMember]
        public Card Card
        {
            get { return _card; }
            set
            {
                if (!ReferenceEquals(_card, value))
                {
                    var previousValue = _card;
                    _card = value;
                    FixupCard(previousValue);
                    OnNavigationPropertyChanged("Card");
                }
            }
        }
        private Card _card;
    
        [DataMember]
        public Satellite Satellite
        {
            get { return _satellite; }
            set
            {
                if (!ReferenceEquals(_satellite, value))
                {
                    var previousValue = _satellite;
                    _satellite = value;
                    FixupSatellite(previousValue);
                    OnNavigationPropertyChanged("Satellite");
                }
            }
        }
        private Satellite _satellite;

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
            Card = null;
            Satellite = null;
        }

        #endregion
        #region Association Fixup
    
        private void FixupCard(Card previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.DisEqcMotors.Contains(this))
            {
                previousValue.DisEqcMotors.Remove(this);
            }
    
            if (Card != null)
            {
                if (!Card.DisEqcMotors.Contains(this))
                {
                    Card.DisEqcMotors.Add(this);
                }
    
                IdCard = Card.IdCard;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Card")
                    && (ChangeTracker.OriginalValues["Card"] == Card))
                {
                    ChangeTracker.OriginalValues.Remove("Card");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Card", previousValue);
                }
                if (Card != null && !Card.ChangeTracker.ChangeTrackingEnabled)
                {
                    Card.StartTracking();
                }
            }
        }
    
        private void FixupSatellite(Satellite previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.DisEqcMotors.Contains(this))
            {
                previousValue.DisEqcMotors.Remove(this);
            }
    
            if (Satellite != null)
            {
                if (!Satellite.DisEqcMotors.Contains(this))
                {
                    Satellite.DisEqcMotors.Add(this);
                }
    
                IdSatellite = Satellite.IdSatellite;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Satellite")
                    && (ChangeTracker.OriginalValues["Satellite"] == Satellite))
                {
                    ChangeTracker.OriginalValues.Remove("Satellite");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Satellite", previousValue);
                }
                if (Satellite != null && !Satellite.ChangeTracker.ChangeTrackingEnabled)
                {
                    Satellite.StartTracking();
                }
            }
        }

        #endregion
    }
}
