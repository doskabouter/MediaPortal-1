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
using Mediaportal.TV.Server.TVLibrary.Interfaces;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Interfaces;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Logging;
using Mediaportal.TV.Server.TVService.Interfaces.CardHandler;

namespace Mediaportal.TV.Server.TVLibrary.CardManagement.CardHandler
{
  public class ChannelScanning : IChannelScanning
  {
    private readonly ITvCardHandler _cardHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisEqcManagement"/> class.
    /// </summary>
    /// <param name="cardHandler">The card handler.</param>
    public ChannelScanning(ITvCardHandler cardHandler)
    {
      _cardHandler = cardHandler;
    }

    /// <summary>
    /// Returns if the card is scanning or not
    /// </summary>
    /// <returns>true when card is scanning otherwise false</returns>
    public bool IsScanning
    {
      get
      {
        try
        {
          if (_cardHandler.DataBaseCard.Enabled == false)
          {
            return false;
          }        

          return _cardHandler.Card.IsScanning;
        }
        catch (Exception ex)
        {
          Log.Write(ex);
          return false;
        }
      }
    }

    /// <summary>
    /// scans current transponder for more channels.
    /// </summary>
    /// <param name="channel">IChannel containing the transponder tuning details.</param>
    /// <param name="settings">Scan settings</param>
    /// <returns>list of channels found</returns>
    public IChannel[] Scan(IChannel channel, ScanParameters settings)
    {
      try
      {
        if (_cardHandler.DataBaseCard.Enabled == false)
        {
          return new List<IChannel>().ToArray();
        }

        
        ITVScanning scanner = _cardHandler.Card.ScanningInterface;
        if (scanner == null)
          return null;
        scanner.Reset();
        List<IChannel> channelsFound = scanner.Scan(channel, settings);
        if (channelsFound == null)
          return null;
        return channelsFound.ToArray();
      }
      catch (TvExceptionNoSignal)
      {
        //ignore
        return null;
      }
      catch (Exception ex)
      {
        Log.Write(ex);
        return null;
      }
    }

    public IChannel[] ScanNIT(IChannel channel, ScanParameters settings)
    {
      try
      {
        if (_cardHandler.DataBaseCard.Enabled == false)
        {
          return new List<IChannel>().ToArray();
        }
       
        ITVScanning scanner = _cardHandler.Card.ScanningInterface;
        if (scanner == null)
          return null;
        scanner.Reset();
        List<IChannel> channelsFound = scanner.ScanNIT(channel, settings);
        if (channelsFound == null)
          return null;
        return channelsFound.ToArray();
      }
      catch (Exception ex)
      {
        Log.Write(ex);
        return null;
      }
    }
  }
}