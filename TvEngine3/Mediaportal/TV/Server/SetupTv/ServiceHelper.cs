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
using System.Net;
using System.ServiceProcess;
using MediaPortal.Common.Utils;
using Mediaportal.TV.Server.TVControl;
using Mediaportal.TV.Server.TVControl.ServiceAgents;
using MediaPortal.Common.Utils;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Logging;
using Microsoft.Win32;

namespace Mediaportal.TV.Server.SetupTV
{
  /// <summary>
  /// Offers basic control functions for services
  /// </summary>
  public static class ServiceHelper
  {

    public const string SERVICENAME_TVSERVICE = @"TvService";
    private static bool _isRestrictedMode;
    private static bool _ignoreDisconnections;

    /// <summary>
    /// Indicates if the connection to TvService is done using WCF services.
    /// </summary>
    public static bool IsRestrictedMode
    {
      get { return _isRestrictedMode; }
      set { _isRestrictedMode = value; }
    }

    /// <summary>
    /// Indicates if the connection with the TvService is available, which can be local service (<see cref="IsRunning"/>) or remote service via WCF (<see cref="IsRestrictedMode"/>).
    /// </summary>
    public static bool IsAvailable
    {
      get { return IsRestrictedMode || IsRunning; }
    }

    /// <summary>
    /// Read from DB card detection delay 
    /// </summary>
    /// <returns>number of seconds</returns>
    public static int DefaultInitTimeOut()
    {
      return Convert.ToInt16(Singleton<ServiceAgents>.Instance.SettingServiceAgent.GetSettingWithDefaultValue("delayCardDetect", "0").Value) + 10;
    }

    /// <summary>
    /// Does a given service exist
    /// </summary>
    /// <param name="serviceToFind"></param>
    /// <param name="hostname"></param>
    /// <returns></returns>
    public static bool IsInstalled(string serviceToFind, string hostname)
    {
      try
      {
        Log.Error("serviceToFind ={0}, hostname={1}", serviceToFind, hostname);

        ServiceController[] services = ServiceController.GetServices(hostname);

        Log.Error("services count = {0}", services.Length);
        

        foreach (ServiceController service in services)
        {
          Log.Error("services name= {0}", service.ServiceName);
          if (String.Compare(service.ServiceName, serviceToFind, true) == 0)
          {
            return true;
          }
        }
        return false;
      }
      catch (Exception ex)
      {
        //_isRestrictedMode = !Network.IsSingleSeat();
        
        Log.Error(
          "ServiceHelper: Check hostname the tvservice is running failed. Try another hostname. {0}", ex);
        return false;
      }
    }

    /// <summary>
    /// Does a given service exist
    /// </summary>
    /// <param name="serviceToFind"></param>
    /// <returns></returns>
    public static bool IsInstalled(string serviceToFind)
    {
      return IsInstalled(serviceToFind, Dns.GetHostName());
    }

    /// <summary>
    /// Is the status of TvService == Running
    /// </summary>
    public static bool IsRunning
    {
      get
      {
        try
        {
          using (ServiceController sc = new ServiceController("TvService", Singleton<ServiceAgents>.Instance.Hostname))
          {
            return sc.Status == ServiceControllerStatus.Running;
          }
        }
        catch (Exception ex)
        {
          Log.Error(ex, 
            "ServiceHelper: Check whether the tvservice is running failed. Please check your installation.");
          return false;
        }
      }
    }

    /// <summary>
    /// Is TvService fully initialized?
    /// </summary>
    public static bool IsInitialized
    {
      get { return WaitInitialized(0); }
    }

    /// <summary>
    /// Wait until TvService is fully initialized, wait for the default timeout
    /// </summary>
    /// <returns>true if thTvService is initialized</returns>
    public static bool WaitInitialized()
    {
      return WaitInitialized(DefaultInitTimeOut() * 1000);
    }

    /// <summary>
    /// Wait until TvService is fully initialized
    /// </summary>
    /// <param name="millisecondsTimeout">the maximum time to wait in milliseconds</param>
    /// <remarks>If <paramref name="millisecondsTimeout"/> is 0, the current status is immediately returned.
    /// Use <paramref name="millisecondsTimeout"/>=-1 to wait indefinitely</remarks>
    /// <returns>true if thTvService is initialized</returns>
    public static bool WaitInitialized(int millisecondsTimeout)
    {
      try
      {
        using (ServiceController sc = new ServiceController("TvService", Singleton<ServiceAgents>.Instance.Hostname))
        {
          sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 0, 0, millisecondsTimeout));
          return (sc.Status == ServiceControllerStatus.Running);
        }
        /*
        EventWaitHandle initialized = EventWaitHandle.OpenExisting(RemoteControl.InitializedEventName,
                                                                    EventWaitHandleRights.Synchronize);
        return initialized.WaitOne(millisecondsTimeout);*/
      }
      catch (Exception ex) // either we have no right, or the event does not exist
      {
        Log.Error("Failed to wait for {0}", RemoteControl.InitializedEventName);
        Log.Error(ex);
      }

      /*
      // Fall back: try to call a method on the server (for earlier versions of TvService)
      DateTime expires = millisecondsTimeout == -1
                           ? DateTime.MaxValue
                           : DateTime.Now.AddMilliseconds(millisecondsTimeout);

      // Note if millisecondsTimeout = 0, we never enter the loop and always return false
      // There is no way to determine if TvService is initialized without waiting
      while (DateTime.Now < expires)
      {
        try
        {
          int cards = ServiceAgents.Instance.ControllerServiceAgent.Cards;
          return true;
        }
        catch (System.Runtime.Remoting.RemotingException)
        {
          Log.Info("ServiceHelper: Waiting for tvserver to initialize. (remoting not initialized)");
        }
        catch (System.Net.Sockets.SocketException)
        {
          Log.Info("ServiceHelper: Waiting for tvserver to initialize. (socket not initialized)");
        }
        catch (Exception ex)
        {
          Log.Error(
            "ServiceHelper: Could not check whether the tvservice is running. Please check your network as well. \nError: {0}",
            ex.ToString());
          break;
        }
        Thread.Sleep(250);
      }*/
      return false;
    }

    /// <summary>
    /// Is the status of TvService == Stopped
    /// </summary>
    public static bool IsStopped
    {
      get
      {
        try
        {
          using (ServiceController sc = new ServiceController("TvService", Singleton<ServiceAgents>.Instance.Hostname))
          {
            return sc.Status == ServiceControllerStatus.Stopped; // should we consider Stopping as stopped?
          }
        }
        catch (Exception ex)
        {
          Log.Error(ex,
            "ServiceHelper: Check whether the tvservice is stopped failed. Please check your installation.");
          return false;
        }
      }
    }

    public static bool IgnoreDisconnections
    {
      get { return _ignoreDisconnections; }
      set { _ignoreDisconnections = value; }
    }

    /// <summary>
    /// Stop the TvService
    /// </summary>
    /// <returns></returns>
    public static bool Stop()
    {
      try
      {
        using (ServiceController sc = new ServiceController("TvService", Singleton<ServiceAgents>.Instance.Hostname))
        {
          switch (sc.Status)
          {
            case ServiceControllerStatus.Running:
              sc.Stop();
              break;
            case ServiceControllerStatus.StopPending:
              break;
            case ServiceControllerStatus.Stopped:
              return true;
            default:
              return false;
          }
          sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(60));
          return sc.Status == ServiceControllerStatus.Stopped;
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex, 
          "ServiceHelper: Stopping tvservice failed. Please check your installation.");
        return false;
      }
    }

    /// <summary>
    /// Starts the TvService
    /// </summary>
    /// <returns></returns>
    public static bool Start()
    {
      return Start("TvService");
    }

    public static bool Start(string aServiceName)
    {
      try
      {
        using (ServiceController sc = new ServiceController(aServiceName, Singleton<ServiceAgents>.Instance.Hostname))
        {
          switch (sc.Status)
          {
            case ServiceControllerStatus.Stopped:
              sc.Start();
              _ignoreDisconnections = false;
              break;
            case ServiceControllerStatus.StartPending:
              break;
            case ServiceControllerStatus.Running:
              return true;
            default:
              return false;
          }
          sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(60));
          return sc.Status == ServiceControllerStatus.Running;
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex, "ServiceHelper: Starting {0} failed. Please check your installation.", aServiceName);
        return false;
      }
    }

    /// <summary>
    /// Start/Stop cycles TvService
    /// </summary>
    /// <returns>Always true</returns>
    public static bool Restart()
    {
      if (!IsInstalled(SERVICENAME_TVSERVICE, Singleton<ServiceAgents>.Instance.Hostname))
      {
        return false;
      }
      Stop();
      return Start();
    }

    /// <summary>
    /// Looks up the database service name for tvengine 3
    /// </summary>
    /// <param name="partOfSvcNameToComplete">Supply a (possibly unique) search term to indentify the service</param>
    /// <returns>true when search was successfull - modifies the search pattern to return the correct full name</returns>
    public static bool GetDBServiceName(ref string partOfSvcNameToComplete)
    {
      ServiceController[] services = ServiceController.GetServices(Singleton<ServiceAgents>.Instance.Hostname);
      foreach (ServiceController service in services)
      {
        if (service.ServiceName.Contains(partOfSvcNameToComplete))
        {
          partOfSvcNameToComplete = service.ServiceName;
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Checks via registry whether a given service is set to autostart on boot
    /// </summary>
    /// <param name="aServiceName">The short name of the service</param>
    /// <param name="aSetEnabled">Enable autostart if needed</param>
    /// <returns>true if the service will start at boot</returns>
    public static bool IsServiceEnabled(string aServiceName, bool aSetEnabled)
    {
      try
      {
        using (
          RegistryKey rKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + aServiceName, true)
          )
        {
          if (rKey != null)
          {
            int startMode = (int)rKey.GetValue("Start", 3);
            if (startMode == 2) // autostart
              return true;
            if (aSetEnabled)
            {
              rKey.SetValue("Start", 2, RegistryValueKind.DWord);
              return true;
            }
            return false;
          }
          return false; // probably wrong service name
        }
      }
      catch (Exception)
      {
        return false;
      }
    }

    /// <summary>
    /// Write dependency info for TvService.exe to registry
    /// </summary>
    /// <param name="dependsOnService">the database service that needs to be started</param>
    /// <returns>true if dependency was added successfully</returns>
    public static bool AddDependencyByName(string dependsOnService)
    {
      try
      {
        using (RegistryKey rKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\TVService", true)
          )
        {
          if (rKey != null)
          {
            rKey.SetValue("DependOnService", new[] {dependsOnService, "Netman"}, RegistryValueKind.MultiString);
            rKey.SetValue("Start", 2, RegistryValueKind.DWord); // Set TVService to autostart
          }
        }

        return true;
      }
      catch (Exception ex)
      {
        Log.Error(ex, "ServiceHelper: Failed to access registry");
        return false;
      }
    }
  }
}