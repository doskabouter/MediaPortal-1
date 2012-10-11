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

using Mediaportal.TV.Server.TVLibrary.Interfaces.CiMenu;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Interfaces;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Logging;

namespace Mediaportal.TV.Server.SetupTV.Sections.CIMenu
{

  #region CI Menu

  /// <summary>
  /// Handler class for gui interactions of ci menu
  /// </summary>
  public class CiMenuEventHandler : ICiMenuEventCallback
  {
    private CI_Menu_Dialog _refDlg;

    public void SetCaller(CI_Menu_Dialog caller)
    {
      _refDlg = caller;
    }

    /// <summary>
    /// eventhandler to show CI Menu dialog
    /// </summary>
    /// <param name="menu"></param>
    public void CiMenuCallback(CiMenu menu)
    {
      try
      {
        Log.Debug("Callback from tvserver {0}", menu.Title);

        // pass menu to calling dialog
        if (_refDlg != null)
          _refDlg.CiMenuCallback(menu);
      }
      catch
      {
        menu = new CiMenu("Remoting Exception", "Communication with server failed", null,
                          CiMenuState.Error);
        // pass menu to calling dialog
        if (_refDlg != null)
          _refDlg.CiMenuCallback(menu);
      }
    }
  }

  #endregion
}