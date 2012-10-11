/*
 *  Copyright (C) 2006-2008 Team MediaPortal
 *  http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */
#pragma once

#include "..\..\shared\BasePmtParser.h"

using namespace std;

class IPmtCallBack2
{
  public:
    virtual void OnPmtReceived(const CPidTable& pidInfo) = 0;
};

class CPmtParser : public CBasePmtParser
{
  public:
    CPmtParser(void);
    virtual ~CPmtParser(void);
    void OnNewSection(CSection& sections);
    void SetCallBack(IPmtCallBack2* callBack);
    void Reset();

  private:
    IPmtCallBack2* m_pCallBack;
};
