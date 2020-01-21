#region Copyright (C) 2005-2020 Team MediaPortal

// Copyright (C) 2005-2020 Team MediaPortal
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

using MediaPortal.Configuration;
using MediaPortal.Database;
using MediaPortal.GUI.Library;
using MediaPortal.GUI.Pictures;
using MediaPortal.Player;
using MediaPortal.Util;

using SQLite.NET;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace MediaPortal.Picture.Database
{
  /// <summary>
  /// Summary description for PictureDatabaseSqlLite.
  /// </summary>
  public class PictureDatabaseSqlLite : IPictureDatabase, IDisposable
  {
    private bool disposed = false;
    private SQLiteClient m_db = null;
    private bool _useExif = true;
    private bool _usePicasa = false;
    private bool _dbHealth = false;

    public PictureDatabaseSqlLite()
    {
      Open();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void Open()
    {
      try
      {
        // Maybe called by an exception
        if (m_db != null)
        {
          try
          {
            m_db.Close();
            m_db.Dispose();
            m_db = null;
            Log.Warn("Picture.DB.SQLite: Disposing current DB instance..");
          }
          catch (Exception ex)
          {
            Log.Error("Picture.DB.SQLite: Open: {0}", ex.Message);
          }
        }

        // Open database
        try
        {
          Directory.CreateDirectory(Config.GetFolder(Config.Dir.Database));
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Create DB directory: {0}", ex.Message);
        }

        m_db = new SQLiteClient(Config.GetFile(Config.Dir.Database, "PictureDatabaseV3.db3"));
        // Retry 10 times on busy (DB in use or system resources exhausted)
        m_db.BusyRetries = 10;
        // Wait 100 ms between each try (default 10)
        m_db.BusyRetryDelay = 100;

        _dbHealth = DatabaseUtility.IntegrityCheck(m_db);

        DatabaseUtility.SetPragmas(m_db);

        CreateTables();
        InitSettings();
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: Open DB: {0} stack:{1}", ex.Message, ex.StackTrace);
        Open();
      }
      Log.Info("Picture database opened...");
    }

    private void InitSettings()
    {
      using (Profile.Settings xmlreader = new Profile.MPSettings())
      {
        _useExif = xmlreader.GetValueAsBool("pictures", "useExif", true);
        _usePicasa = xmlreader.GetValueAsBool("pictures", "usePicasa", false);
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private bool CreateTables()
    {
      if (m_db == null)
      {
        return false;
      }

      #region Tables
      DatabaseUtility.AddTable(m_db, "picture",
                               "CREATE TABLE picture (idPicture INTEGER PRIMARY KEY, strFile TEXT, iRotation INTEGER, strDateTaken TEXT)");
      #endregion

      #region Indexes
      DatabaseUtility.AddIndex(m_db, "idxpicture_idPicture", "CREATE INDEX idxpicture_idPicture ON picture(idPicture)");
      DatabaseUtility.AddIndex(m_db, "idxpicture_strFile", "CREATE INDEX idxpicture_strFile ON picture (strFile ASC)");
      DatabaseUtility.AddIndex(m_db, "idxpicture_strDateTaken", "CREATE INDEX idxpicture_strDateTaken ON picture (strDateTaken ASC)");
      DatabaseUtility.AddIndex(m_db, "idxpicture_strDateTaken_Year", "CREATE INDEX idxpicture_strDateTaken_Year ON picture (SUBSTR(strDateTaken,1,4))");
      DatabaseUtility.AddIndex(m_db, "idxpicture_strDateTaken_Month", "CREATE INDEX idxpicture_strDateTaken_Month ON picture (SUBSTR(strDateTaken,6,2))");
      DatabaseUtility.AddIndex(m_db, "idxpicture_strDateTaken_Day", "CREATE INDEX idxpicture_strDateTaken_Day ON picture (SUBSTR(strDateTaken,9,2))");
      #endregion

      #region Exif Tables
      DatabaseUtility.AddTable(m_db, "camera",
                               "CREATE TABLE camera (idCamera INTEGER PRIMARY KEY, strCamera TEXT, strCameraMake TEXT);");
      // DatabaseUtility.AddTable(m_db, "cameralinkpicture",
      //                         "CREATE TABLE cameralinkpicture (idCamera INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "lens",
                               "CREATE TABLE lens (idLens INTEGER PRIMARY KEY, strLens TEXT, strLensMake TEXT);");
      // DatabaseUtility.AddTable(m_db, "lenslinkpicture",
      //                         "CREATE TABLE lenslinkpicture (idLens INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "orientation",
                               "CREATE TABLE orientation (idOrientation INTEGER PRIMARY KEY, strOrientation TEXT);");
      // DatabaseUtility.AddTable(m_db, "orientationlinkpicture",
      //                         "CREATE TABLE orientationlinkpicture (idOrientation INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "flash",
                               "CREATE TABLE flash (idFlash INTEGER PRIMARY KEY, strFlash TEXT);");
      // DatabaseUtility.AddTable(m_db, "flashlinkpicture",
      //                         "CREATE TABLE flashlinkpicture (idFlash INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "meteringmode",
                               "CREATE TABLE meteringmode (idMeteringMode INTEGER PRIMARY KEY, strMeteringMode TEXT);");
      // DatabaseUtility.AddTable(m_db, "meteringmodelinkpicture",
      //                         "CREATE TABLE meteringmodelinkpicture (idMeteringMode INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "country",
                               "CREATE TABLE country (idCountry INTEGER PRIMARY KEY, strCountryCode TEXT, strCountry TEXT);");
      // DatabaseUtility.AddTable(m_db, "countrylinkpicture",
      //                         "CREATE TABLE countrylinkpicture (idCountry INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "state",
                               "CREATE TABLE state (idState INTEGER PRIMARY KEY, strState TEXT);");
      // DatabaseUtility.AddTable(m_db, "statelinkpicture",
      //                         "CREATE TABLE statelinkpicture (idState INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "city",
                               "CREATE TABLE city (idCity INTEGER PRIMARY KEY, strCity TEXT);");
      // DatabaseUtility.AddTable(m_db, "citylinkpicture",
      //                         "CREATE TABLE citylinkpicture (idCity INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "sublocation",
                               "CREATE TABLE sublocation (idSublocation INTEGER PRIMARY KEY, strSublocation TEXT);");
      // DatabaseUtility.AddTable(m_db, "sublocationlinkpicture",
      //                         "CREATE TABLE sublocationlinkpicture (idSublocation INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "exposureprogram",
                               "CREATE TABLE exposureprogram (idExposureProgram INTEGER PRIMARY KEY, strExposureProgram TEXT);");
      // DatabaseUtility.AddTable(m_db, "exposureprogramlinkpicture",
      //                         "CREATE TABLE exposureprogramlinkpicture (idExposureProgram INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "exposuremode",
                               "CREATE TABLE exposuremode (idExposureMode INTEGER PRIMARY KEY, strExposureMode TEXT);");
      // DatabaseUtility.AddTable(m_db, "exposuremodelinkpicture",
      //                         "CREATE TABLE exposuremodelinkpicture (idExposureMode INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "sensingmethod",
                               "CREATE TABLE sensingmethod (idSensingMethod INTEGER PRIMARY KEY, strSensingMethod TEXT);");
      // DatabaseUtility.AddTable(m_db, "sensingmethodlinkpicture",
      //                         "CREATE TABLE sensingmethodlinkpicture (idSensingMethod INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "scenetype",
                               "CREATE TABLE scenetype (idSceneType INTEGER PRIMARY KEY, strSceneType TEXT);");
      // DatabaseUtility.AddTable(m_db, "scenetypelinkpicture",
      //                         "CREATE TABLE scenetypelinkpicture (idSceneType INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "scenecapturetype",
                               "CREATE TABLE scenecapturetype (idSceneCaptureType INTEGER PRIMARY KEY, strSceneCaptureType TEXT);");
      // DatabaseUtility.AddTable(m_db, "scenecapturetypelinkpicture",
      //                         "CREATE TABLE scenecapturetypelinkpicture (idSceneCaptureType INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "whitebalance",
                               "CREATE TABLE whitebalance (idWhiteBalance INTEGER PRIMARY KEY, strWhiteBalance TEXT);");
      // DatabaseUtility.AddTable(m_db, "whitebalancelinkpicture",
      //                         "CREATE TABLE whitebalancelinkpicture (idWhiteBalance INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "author",
                               "CREATE TABLE author (idAuthor INTEGER PRIMARY KEY, strAuthor TEXT);");
      // DatabaseUtility.AddTable(m_db, "authorlinkpicture",
      //                         "CREATE TABLE authorlinkpicture (idAuthor INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "byline",
                               "CREATE TABLE byline (idByline INTEGER PRIMARY KEY, strByline TEXT);");
      // DatabaseUtility.AddTable(m_db, "bylinelinkpicture",
      //                         "CREATE TABLE bylinelinkpicture (idByline INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "software",
                               "CREATE TABLE software (idSoftware INTEGER PRIMARY KEY, strSoftware TEXT);");
      // DatabaseUtility.AddTable(m_db, "softwarelinkpicture",
      //                         "CREATE TABLE softwarelinkpicture (idSoftware INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "usercomment",
                               "CREATE TABLE usercomment (idUserComment INTEGER PRIMARY KEY, strUserComment TEXT);");
      // DatabaseUtility.AddTable(m_db, "usercommentlinkpicture",
      //                         "CREATE TABLE usercommentlinkpicture (idUserComment INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "copyright",
                               "CREATE TABLE copyright (idCopyright INTEGER PRIMARY KEY, strCopyright TEXT);");
      // DatabaseUtility.AddTable(m_db, "copyrightlinkpicture",
      //                         "CREATE TABLE copyrightlinkpicture (idCopyright INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "copyrightnotice",
                               "CREATE TABLE copyrightnotice (idCopyrightNotice INTEGER PRIMARY KEY, strCopyrightNotice TEXT);");
      // DatabaseUtility.AddTable(m_db, "copyrightnoticelinkpicture",
      //                         "CREATE TABLE copyrightnoticelinkpicture (idCopyrightNotice INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "keywords",
                               "CREATE TABLE keywords (idKeyword INTEGER PRIMARY KEY, strKeyword TEXT);");
      DatabaseUtility.AddTable(m_db, "keywordslinkpicture",
                               "CREATE TABLE keywordslinkpicture (idKeyword INTEGER, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE);");

      DatabaseUtility.AddTable(m_db, "exif",
                               "CREATE TABLE exif (idExif INTEGER PRIMARY KEY, idPicture REFERENCES picture(idPicture) ON DELETE CASCADE, " +
                                                  "strISO TEXT, " +
                                                  "strExposureTime TEXT, " +
                                                  "strExposureCompensation TEXT, " +
                                                  "strFStop TEXT, " +
                                                  "strShutterSpeed TEXT, " +
                                                  "strFocalLength TEXT, " +
                                                  "strFocalLength35 TEXT, " +
                                                  "strGPSLatitude TEXT, " +
                                                  "strGPSLongitude TEXT, " +
                                                  "strGPSAltitude TEXT);");

      DatabaseUtility.AddTable(m_db, "exiflinkpicture",
                               "CREATE TABLE exiflinkpicture (idPicture REFERENCES picture(idPicture) ON DELETE CASCADE, " +
                                                             "idCamera INTEGER, " +
                                                             "idLens INTEGER, " +
                                                             "idExif INTEGER, " +
                                                             "idOrientation INTEGER, " +
                                                             "idFlash INTEGER, " +
                                                             "idMeteringMode INTEGER, " +
                                                             "idExposureProgram INTEGER, idExposureMode INTEGER, " +
                                                             "idSensingMethod INTEGER, " +
                                                             "idSceneType INTEGER, idSceneCaptureType INTEGER, " +
                                                             "idWhiteBalance INTEGER," +
                                                             "idAuthor INTEGER, idByline INTEGER, " +
                                                             "idSoftware INTEGER, idUserComment INTEGER, " +
                                                             "idCopyright INTEGER, idCopyrightNotice INTEGER, " +
                                                             "idCountry INTEGER, idState INTEGER, idCity INTEGER, idSublocation INTEGER);");
      #endregion

      #region Exif Indexes
      /*
      DatabaseUtility.AddIndex(m_db, "idxcameralinkpicture_idCamera", "CREATE INDEX idxcameralinkpicture_idCamera ON cameralinkpicture(idCamera);");
      DatabaseUtility.AddIndex(m_db, "idxcameralinkpicture_idPicture", "CREATE INDEX idxcameralinkpicture_idPicture ON cameralinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxlenslinkpicture_idLens", "CREATE INDEX idxlenslinkpicture_idLens ON lenslinkpicture(idLens);");
      DatabaseUtility.AddIndex(m_db, "idxlenslinkpicture_idPicture", "CREATE INDEX idxlenslinkpicture_idPicture ON lenslinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxorientationlinkpicture_idOrientation", "CREATE INDEX idxorientationlinkpicture_idOrientation ON orientationlinkpicture(idOrientation);");
      DatabaseUtility.AddIndex(m_db, "idxorientationlinkpicture_idPicture", "CREATE INDEX idxorientationlinkpicture_idPicture ON orientationlinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxflashlinkpicture_idFlash", "CREATE INDEX idxflashlinkpicture_idFlash ON flashlinkpicture(idFlash);");
      DatabaseUtility.AddIndex(m_db, "idxflashlinkpicture_idPicture", "CREATE INDEX idxflashlinkpicture_idPicture ON flashlinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxmeteringmodelinkpicture_idMeteringMode", "CREATE INDEX idxmeteringmodelinkpicture_idMeteringMode ON meteringmodelinkpicture(idMeteringMode);");
      DatabaseUtility.AddIndex(m_db, "idxmeteringmodelinkpicture_idPicture", "CREATE INDEX idxmeteringmodelinkpicture_idPicture ON meteringmodelinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxcountrylinkpicture_idCountry", "CREATE INDEX idxcountrylinkpicture_idCountry ON countrylinkpicture(idCountry);");
      DatabaseUtility.AddIndex(m_db, "idxcountrylinkpicture_idPicture", "CREATE INDEX idxcountrylinkpicture_idPicture ON countrylinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxstatelinkpicture_idState", "CREATE INDEX idxstatelinkpicture_idState ON statelinkpicture(idState);");
      DatabaseUtility.AddIndex(m_db, "idxstatelinkpicture_idPicture", "CREATE INDEX idxstatelinkpicture_idPicture ON statelinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxcitylinkpicture_idCity", "CREATE INDEX idxcitylinkpicture_idCity ON citylinkpicture(idCity);");
      DatabaseUtility.AddIndex(m_db, "idxcitylinkpicture_idPicture", "CREATE INDEX idxcitylinkpicture_idPicture ON citylinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxsublocationlinkpicture_idSublocation", "CREATE INDEX idxsublocationlinkpicture_idSublocation ON sublocationlinkpicture(idSublocation);");
      DatabaseUtility.AddIndex(m_db, "idxsublocationlinkpicture_idPicture", "CREATE INDEX idxsublocationlinkpicture_idPicture ON sublocationlinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxexposureprogramlinkpicture_idExposureProgram", "CREATE INDEX idxexposureprogramlinkpicture_idExposureProgram ON exposureprogramlinkpicture(idExposureProgram);");
      DatabaseUtility.AddIndex(m_db, "idxexposureprogramlinkpicture_idPicture", "CREATE INDEX idxexposureprogramlinkpicture_idPicture ON exposureprogramlinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxexposuremodelinkpicture_idExposureMode", "CREATE INDEX idxexposuremodelinkpicture_idExposureMode ON exposuremodelinkpicture(idExposureMode);");
      DatabaseUtility.AddIndex(m_db, "idxexposuremodelinkpicture_idPicture", "CREATE INDEX idxexposuremodelinkpicture_idPicture ON exposuremodelinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxsensingmethodlinkpicture_idSensingMethod", "CREATE INDEX idxsensingmethodlinkpicture_idSensingMethod ON sensingmethodlinkpicture(idSensingMethod);");
      DatabaseUtility.AddIndex(m_db, "idxsensingmethodlinkpicture_idPicture", "CREATE INDEX idxsensingmethodlinkpicture_idPicture ON sensingmethodlinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxscenetypelinkpicture_idSceneType", "CREATE INDEX idxscenetypelinkpicture_idSceneType ON scenetypelinkpicture(idSceneType);");
      DatabaseUtility.AddIndex(m_db, "idxscenetypelinkpicture_idPicture", "CREATE INDEX idxscenetypelinkpicture_idPicture ON scenetypelinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxscenecapturetypelinkpicture_idSceneCaptureType", "CREATE INDEX idxscenecapturetypelinkpicture_idSceneCaptureType ON scenecapturetypelinkpicture(idSceneCaptureType);");
      DatabaseUtility.AddIndex(m_db, "idxscenecapturetypelinkpicture_idPicture", "CREATE INDEX idxscenecapturetypelinkpicture_idPicture ON scenecapturetypelinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxwhitebalancelinkpicture_idWhiteBalance", "CREATE INDEX idxwhitebalancelinkpicture_idWhiteBalance ON whitebalancelinkpicture(idWhiteBalance);");
      DatabaseUtility.AddIndex(m_db, "idxwhitebalancelinkpicture_idPicture", "CREATE INDEX idxwhitebalancelinkpicture_idPicture ON whitebalancelinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxauthorlinkpicture_idAuthor", "CREATE INDEX idxauthorlinkpicture_idAuthor ON authorlinkpicture(idAuthor);");
      DatabaseUtility.AddIndex(m_db, "idxauthorlinkpicture_idPicture", "CREATE INDEX idxauthorlinkpicture_idPicture ON authorlinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxbylinelinkpicture_idByline", "CREATE INDEX idxbylinelinkpicture_idByline ON bylinelinkpicture(idByline);");
      DatabaseUtility.AddIndex(m_db, "idxbylinelinkpicture_idPicture", "CREATE INDEX idxbylinelinkpicture_idPicture ON bylinelinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxsoftwarelinkpicture_idSoftware", "CREATE INDEX idxsoftwarelinkpicture_idSoftware ON softwarelinkpicture(idSoftware);");
      DatabaseUtility.AddIndex(m_db, "idxsoftwarelinkpicture_idPicture", "CREATE INDEX idxsoftwarelinkpicture_idPicture ON softwarelinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxusercommentlinkpicture_idUserComment", "CREATE INDEX idxusercommentlinkpicture_idUserComment ON usercommentlinkpicture(idUserComment);");
      DatabaseUtility.AddIndex(m_db, "idxusercommentlinkpicture_idPicture", "CREATE INDEX idxusercommentlinkpicture_idPicture ON usercommentlinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxcopyrightlinkpicture_idCopyright", "CREATE INDEX idxcopyrightlinkpicture_idCopyright ON copyrightlinkpicture(idCopyright);");
      DatabaseUtility.AddIndex(m_db, "idxcopyrightlinkpicture_idPicture", "CREATE INDEX idxcopyrightlinkpicture_idPicture ON copyrightlinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxcopyrightnoticelinkpicture_idCopyrightNotice", "CREATE INDEX idxcopyrightnoticelinkpicture_idCopyrightNotice ON copyrightnoticelinkpicture(idCopyrightNotice);");
      DatabaseUtility.AddIndex(m_db, "idxcopyrightnoticelinkpicture_idPicture", "CREATE INDEX idxcopyrightnoticelinkpicture_idPicture ON copyrightnoticelinkpicture(idPicture);");
      */
      DatabaseUtility.AddIndex(m_db, "idxcamera_idCamera", "CREATE INDEX idxcamera_idCamera ON camera(idCamera);");
      DatabaseUtility.AddIndex(m_db, "idxlens_idLens", "CREATE INDEX idxlens_idLens ON lens(idLens);");
      DatabaseUtility.AddIndex(m_db, "idxorientation_idOrientation", "CREATE INDEX idxorientation_idOrientation ON orientation(idOrientation);");
      DatabaseUtility.AddIndex(m_db, "idxflash_idFlash", "CREATE INDEX idxflash_idFlash ON flash(idFlash);");
      DatabaseUtility.AddIndex(m_db, "idxmeteringmode_idMeteringMode", "CREATE INDEX idxmeteringmode_idMeteringMode ON meteringmode(idMeteringMode);");
      DatabaseUtility.AddIndex(m_db, "idxcountry_idCountry", "CREATE INDEX idxcountry_idCountry ON country(idCountry);");
      DatabaseUtility.AddIndex(m_db, "idxstate_idState", "CREATE INDEX idxstate_idState ON state(idState);");
      DatabaseUtility.AddIndex(m_db, "idxcity_idCity", "CREATE INDEX idxcity_idCity ON city(idCity);");
      DatabaseUtility.AddIndex(m_db, "idxsublocation_idSublocation", "CREATE INDEX idxsublocation_idSublocation ON sublocation(idSublocation);");
      DatabaseUtility.AddIndex(m_db, "idxexposureprogram_idExposureProgram", "CREATE INDEX idxexposureprogram_idExposureProgram ON exposureprogram(idExposureProgram);");
      DatabaseUtility.AddIndex(m_db, "idxexposuremode_idExposureMode", "CREATE INDEX idxexposuremode_idExposureMode ON exposuremode(idExposureMode);");
      DatabaseUtility.AddIndex(m_db, "idxsensingmethod_idSensingMethod", "CREATE INDEX idxsensingmethod_idSensingMethod ON sensingmethod(idSensingMethod);");
      DatabaseUtility.AddIndex(m_db, "idxscenetype_idSceneType", "CREATE INDEX idxscenetype_idSceneType ON scenetype(idSceneType);");
      DatabaseUtility.AddIndex(m_db, "idxscenecapturetype_idSceneCaptureType", "CREATE INDEX idxscenecapturetype_idSceneCaptureType ON scenecapturetype(idSceneCaptureType);");
      DatabaseUtility.AddIndex(m_db, "idxwhitebalance_idWhiteBalance", "CREATE INDEX idxwhitebalance_idWhiteBalance ON whitebalance(idWhiteBalance);");
      DatabaseUtility.AddIndex(m_db, "idxauthor_idAuthor", "CREATE INDEX idxauthor_idAuthor ON author(idAuthor);");
      DatabaseUtility.AddIndex(m_db, "idxbyline_idByline", "CREATE INDEX idxbyline_idByline ON byline(idByline);");
      DatabaseUtility.AddIndex(m_db, "idxsoftware_idSoftware", "CREATE INDEX idxsoftware_idSoftware ON software(idSoftware);");
      DatabaseUtility.AddIndex(m_db, "idxusercomment_idUserComment", "CREATE INDEX idxusercomment_idUserComment ON usercomment(idUserComment);");
      DatabaseUtility.AddIndex(m_db, "idxcopyright_idCopyright", "CREATE INDEX idxcopyright_idCopyright ON copyright(idCopyright);");
      DatabaseUtility.AddIndex(m_db, "idxcopyrightnotice_idCopyrightNotice", "CREATE INDEX idxcopyrightnotice_idCopyrightNotice ON copyrightnotice(idCopyrightNotice);");

      DatabaseUtility.AddIndex(m_db, "idxkeywords_idKeyword", "CREATE INDEX idxkeywords_idKeyword ON keywords(idKeyword);");
      DatabaseUtility.AddIndex(m_db, "idxkeywords_strKeyword", "CREATE INDEX idxkeywords_strKeyword ON keywords(strKeyword);");

      DatabaseUtility.AddIndex(m_db, "idxkeywordslinkpicture_idKeyword", "CREATE INDEX idxkeywordslinkpicture_idKeyword ON keywordslinkpicture(idKeyword);");
      DatabaseUtility.AddIndex(m_db, "idxkeywordslinkpicture_idPicture", "CREATE INDEX idxkeywordslinkpicture_idPicture ON keywordslinkpicture(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxexif_idPicture", "CREATE INDEX idxexif_idPicture ON exif(idPicture);");

      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idPicture", "CREATE INDEX idxexiflinkpicture_idPicture ON exiflinkpicture(idPicture);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idCamera", "CREATE INDEX idxexiflinkpicture_idCamera ON exiflinkpicture(idCamera);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idLens", "CREATE INDEX idxexiflinkpicture_idLens ON exiflinkpicture(idLens);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idExif", "CREATE INDEX idxexiflinkpicture_idExif ON exiflinkpicture(idExif);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idOrientation", "CREATE INDEX idxexiflinkpicture_idOrientation ON exiflinkpicture(idOrientation);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idFlash", "CREATE INDEX idxexiflinkpicture_idFlash ON exiflinkpicture(idFlash);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idMeteringMode", "CREATE INDEX idxexiflinkpicture_idMeteringMode ON exiflinkpicture(idMeteringMode);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idExposureProgram", "CREATE INDEX idxexiflinkpicture_idExposureProgram ON exiflinkpicture(idExposureProgram);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idExposureMode", "CREATE INDEX idxexiflinkpicture_idExposureMode ON exiflinkpicture(idExposureMode);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idSensingMethod", "CREATE INDEX idxexiflinkpicture_idSensingMethod ON exiflinkpicture(idSensingMethod);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idSceneType", "CREATE INDEX idxexiflinkpicture_idSceneType ON exiflinkpicture(idSceneType);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idSceneCaptureType", "CREATE INDEX idxexiflinkpicture_idSceneCaptureType ON exiflinkpicture(idSceneCaptureType);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idWhiteBalance", "CREATE INDEX idxexiflinkpicture_idWhiteBalance ON exiflinkpicture(idWhiteBalance);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idAuthor", "CREATE INDEX idxexiflinkpicture_idAuthor ON exiflinkpicture(idAuthor);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idByline", "CREATE INDEX idxexiflinkpicture_idByline ON exiflinkpicture(idByline);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idSoftware", "CREATE INDEX idxexiflinkpicture_idSoftware ON exiflinkpicture(idSoftware);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idUserComment", "CREATE INDEX idxexiflinkpicture_idUserComment ON exiflinkpicture(idUserComment);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idCopyright", "CREATE INDEX idxexiflinkpicture_idCopyright ON exiflinkpicture(idCopyright);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idCopyrightNotice", "CREATE INDEX idxexiflinkpicture_idCopyrightNotice ON exiflinkpicture(idCopyrightNotice);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idCountry", "CREATE INDEX idxexiflinkpicture_idCountry ON exiflinkpicture(idCountry);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idState", "CREATE INDEX idxexiflinkpicture_idState ON exiflinkpicture(idState);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idCity", "CREATE INDEX idxexiflinkpicture_idCity ON exiflinkpicture(idCity);");
      DatabaseUtility.AddIndex(m_db, "idxexiflinkpicture_idSublocation", "CREATE INDEX idxexiflinkpicture_idSublocation ON exiflinkpicture(idSublocation);");
      #endregion

      #region Exif Views
      /*
      DatabaseUtility.AddView(m_db, "picturedata", "CREATE VIEW picturedata AS " +
                                                          "SELECT picture.*, camera.*, lens.*, orientation.*, flash.*, meteringmode.*, country.*, state.*, city.*, sublocation.*, " +
                                                                 "exposureprogram.*, exposuremode.*, sensingmethod.*, scenetype.*, scenecapturetype.*, whitebalance.*, " +
                                                                 "author.*, byline.*, software.*, usercomment.*, copyright.*, copyrightnotice.* " +
                                                          "FROM picture " +
                                                   "LEFT JOIN cameralinkpicture ON picture.idPicture = cameralinkpicture.idPicture " +
                                                   "LEFT JOIN camera ON camera.idCamera = cameralinkpicture.idCamera " +
                                                   "LEFT JOIN lenslinkpicture ON picture.idPicture = lenslinkpicture.idPicture " +
                                                   "LEFT JOIN lens ON lens.idLens = lenslinkpicture.idLens " +
                                                   "LEFT JOIN orientationlinkpicture ON picture.idPicture = orientationlinkpicture.idPicture " +
                                                   "LEFT JOIN orientation ON orientation.idOrientation = orientationlinkpicture.idOrientation " +
                                                   "LEFT JOIN flashlinkpicture ON picture.idPicture = flashlinkpicture.idPicture " +
                                                   "LEFT JOIN flash ON flash.idFlash = flashlinkpicture.idFlash " +
                                                   "LEFT JOIN meteringmodelinkpicture ON picture.idPicture = meteringmodelinkpicture.idPicture " +
                                                   "LEFT JOIN meteringmode ON meteringmode.idMeteringMode = meteringmodelinkpicture.idMeteringMode " +
                                                   "LEFT JOIN countrylinkpicture ON picture.idPicture = countrylinkpicture.idPicture " +
                                                   "LEFT JOIN country ON country.idCountry = countrylinkpicture.idCountry " +
                                                   "LEFT JOIN statelinkpicture ON picture.idPicture = statelinkpicture.idPicture " +
                                                   "LEFT JOIN state ON state.idState = statelinkpicture.idState " +
                                                   "LEFT JOIN citylinkpicture ON picture.idPicture = citylinkpicture.idPicture " +
                                                   "LEFT JOIN city ON city.idCity = citylinkpicture.idCity " +
                                                   "LEFT JOIN sublocationlinkpicture ON picture.idPicture = sublocationlinkpicture.idPicture " +
                                                   "LEFT JOIN sublocation ON sublocation.idSublocation = sublocationlinkpicture.idSublocation " +
                                                   "LEFT JOIN exposureprogramlinkpicture ON picture.idPicture = exposureprogramlinkpicture.idPicture " +
                                                   "LEFT JOIN exposureprogram ON exposureprogram.idExposureProgram = exposureprogramlinkpicture.idExposureProgram " +
                                                   "LEFT JOIN exposuremodelinkpicture ON picture.idPicture = exposuremodelinkpicture.idPicture " +
                                                   "LEFT JOIN exposuremode ON exposuremode.idExposureMode = exposuremodelinkpicture.idExposureMode " +
                                                   "LEFT JOIN sensingmethodlinkpicture ON picture.idPicture = sensingmethodlinkpicture.idPicture " +
                                                   "LEFT JOIN sensingmethod ON sensingmethod.idSensingMethod = sensingmethodlinkpicture.idSensingMethod " +
                                                   "LEFT JOIN scenetypelinkpicture ON picture.idPicture = scenetypelinkpicture.idPicture " +
                                                   "LEFT JOIN scenetype ON scenetype.idSceneType = scenetypelinkpicture.idSceneType " +
                                                   "LEFT JOIN scenecapturetypelinkpicture ON picture.idPicture = scenecapturetypelinkpicture.idPicture " +
                                                   "LEFT JOIN scenecapturetype ON scenecapturetype.idSceneCaptureType = scenecapturetypelinkpicture.idSceneCaptureType " +
                                                   "LEFT JOIN whitebalancelinkpicture ON picture.idPicture = whitebalancelinkpicture.idPicture " +
                                                   "LEFT JOIN whitebalance ON whitebalance.idWhiteBalance = whitebalancelinkpicture.idWhiteBalance " +
                                                   "LEFT JOIN authorlinkpicture ON picture.idPicture = authorlinkpicture.idPicture " +
                                                   "LEFT JOIN author ON author.idAuthor = authorlinkpicture.idAuthor " +
                                                   "LEFT JOIN bylinelinkpicture ON picture.idPicture = bylinelinkpicture.idPicture " +
                                                   "LEFT JOIN byline ON byline.idByline = bylinelinkpicture.idByline " +
                                                   "LEFT JOIN softwarelinkpicture ON picture.idPicture = softwarelinkpicture.idPicture " +
                                                   "LEFT JOIN software ON software.idSoftware = softwarelinkpicture.idSoftware " +
                                                   "LEFT JOIN usercommentlinkpicture ON picture.idPicture = usercommentlinkpicture.idPicture " +
                                                   "LEFT JOIN usercomment ON usercomment.idUserComment = usercommentlinkpicture.idUserComment " +
                                                   "LEFT JOIN copyrightlinkpicture ON picture.idPicture = copyrightlinkpicture.idPicture " +
                                                   "LEFT JOIN copyright ON copyright.idCopyright = copyrightlinkpicture.idCopyright " +
                                                   "LEFT JOIN copyrightnoticelinkpicture ON picture.idPicture = copyrightnoticelinkpicture.idPicture " +
                                                   "LEFT JOIN copyrightnotice ON copyrightnotice.idCopyrightNotice = copyrightnoticelinkpicture.idCopyrightNotice;");
      */
      DatabaseUtility.AddView(m_db, "picturedata", "CREATE VIEW picturedata AS " +
                                                          "SELECT picture.*, camera.*, lens.*, exif.*, orientation.*, flash.*, meteringmode.*, country.*, " +
                                                                 "state.*, city.*, sublocation.*, exposureprogram.*, exposuremode.*, sensingmethod.*, " +
                                                                 "scenetype.*, scenecapturetype.*, whitebalance.*, author.*, byline.*, software.*, " +
                                                                 "usercomment.*, copyright.*, copyrightnotice.* " +
                                                          "FROM picture " +
                                                          "LEFT JOIN exiflinkpicture ON picture.idPicture = exiflinkpicture.idPicture " +
                                                          "LEFT JOIN camera ON camera.idCamera = exiflinkpicture.idCamera " +
                                                          "LEFT JOIN lens ON lens.idLens = exiflinkpicture.idLens " +
                                                          "LEFT JOIN exif ON exif.idExif = exiflinkpicture.idExif " +
                                                          "LEFT JOIN orientation ON orientation.idOrientation = exiflinkpicture.idOrientation " +
                                                          "LEFT JOIN flash ON flash.idFlash = exiflinkpicture.idFlash " +
                                                          "LEFT JOIN meteringmode ON meteringmode.idMeteringMode = exiflinkpicture.idMeteringMode " +
                                                          "LEFT JOIN country ON country.idCountry = exiflinkpicture.idCountry " +
                                                          "LEFT JOIN state ON state.idState = exiflinkpicture.idState " +
                                                          "LEFT JOIN city ON city.idCity = exiflinkpicture.idCity " +
                                                          "LEFT JOIN sublocation ON sublocation.idSublocation = exiflinkpicture.idSublocation " +
                                                          "LEFT JOIN exposureprogram ON exposureprogram.idExposureProgram = exiflinkpicture.idExposureProgram " +
                                                          "LEFT JOIN exposuremode ON exposuremode.idExposureMode = exiflinkpicture.idExposureMode " +
                                                          "LEFT JOIN sensingmethod ON sensingmethod.idSensingMethod = exiflinkpicture.idSensingMethod " +
                                                          "LEFT JOIN scenetype ON scenetype.idSceneType = exiflinkpicture.idSceneType " +
                                                          "LEFT JOIN scenecapturetype ON scenecapturetype.idSceneCaptureType = exiflinkpicture.idSceneCaptureType " +
                                                          "LEFT JOIN whitebalance ON whitebalance.idWhiteBalance = exiflinkpicture.idWhiteBalance " +
                                                          "LEFT JOIN author ON author.idAuthor = exiflinkpicture.idAuthor " +
                                                          "LEFT JOIN byline ON byline.idByline = exiflinkpicture.idByline " +
                                                          "LEFT JOIN software ON software.idSoftware = exiflinkpicture.idSoftware " +
                                                          "LEFT JOIN usercomment ON usercomment.idUserComment = exiflinkpicture.idUserComment " +
                                                          "LEFT JOIN copyright ON copyright.idCopyright = exiflinkpicture.idCopyright " +
                                                          "LEFT JOIN copyrightnotice ON copyrightnotice.idCopyrightNotice = exiflinkpicture.idCopyrightNotice;");

      DatabaseUtility.AddView(m_db, "picturekeywords", "CREATE VIEW picturekeywords AS " +
                                                       "SELECT picture.*, keywords.* FROM picture " +
                                                       "JOIN keywordslinkpicture ON picture.idPicture = keywordslinkpicture.idPicture " +
                                                       "JOIN keywords ON keywordslinkpicture.idKeyword = keywords.idKeyword;");
      #endregion

      return true;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int AddPicture(string strPicture, int iRotation)
    {
      // Continue only if it's a picture files
      if (!Util.Utils.IsPicture(strPicture))
      {
        return -1;
      }
      if (String.IsNullOrEmpty(strPicture))
      {
        return -1;
      }
      if (m_db == null)
      {
        return -1;
      }

      try
      {
        int lPicId = -1;
        string strPic = strPicture;
        string strDateTaken = String.Empty;

        DatabaseUtility.RemoveInvalidChars(ref strPic);
        string strSQL = String.Format("SELECT * FROM picture WHERE strFile LIKE '{0}'", strPic);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results != null && results.Rows.Count > 0)
        {
          lPicId = Int32.Parse(DatabaseUtility.Get(results, 0, "idPicture"));
          return lPicId;
        }

        ExifMetadata.Metadata exifData = new ExifMetadata.Metadata();

        // We need the date nevertheless for database view / sorting
        if (!GetExifDetails(strPicture, ref iRotation, ref strDateTaken, ref exifData))
        {
          try
          {
            DateTime dat = File.GetLastWriteTime(strPicture);
            if (!TimeZone.CurrentTimeZone.IsDaylightSavingTime(dat))
            {
              dat = dat.AddHours(1); // Try to respect the timezone of the file date
            }
            strDateTaken = dat.ToString("yyyy-MM-dd HH:mm:ss");
          }
          catch (Exception ex)
          {
            Log.Error("Picture.DB.SQLite: Conversion exception getting file date: {0} stack:{1}", ex.Message, ex.StackTrace);
          }
        }

        // Save potential performance penalty
        if (_usePicasa)
        {
          if (GetPicasaRotation(strPic, ref iRotation))
          {
            Log.Debug("Picture.DB.SQLite: Changed rotation of image {0} based on picasa file to {1}", strPic, iRotation);
          }
        }

        // Transactions are a special case for SQLite - they speed things up quite a bit
        BeginTransaction();

        strSQL = String.Format("INSERT INTO picture (idPicture, strFile, iRotation, strDateTaken) VALUES (NULL, '{0}',{1},'{2}')",
                                strPic, iRotation, strDateTaken);
        results = m_db.Execute(strSQL);
        if (results.Rows.Count > 0)
        {
          Log.Debug("Picture.DB.SQLite: Added Picture to database - {0}", strPic);
        }

        CommitTransaction();

        lPicId = m_db.LastInsertID();
        AddPictureExifData(lPicId, exifData) ; 

        if (g_Player.Playing)
        {
          Thread.Sleep(50);
        }
        else
        {
          Thread.Sleep(1);
        }

        return lPicId;
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        RollbackTransaction();
        // Open();
      }
      return -1;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int UpdatePicture(string strPicture, int iRotation)
    {
      // Continue only if it's a picture files
      if (!Util.Utils.IsPicture(strPicture))
      {
        return -1;
      }
      if (String.IsNullOrEmpty(strPicture))
      {
        return -1;
      }
      if (m_db == null)
      {
        return -1;
      }

      try
      {
        int lPicId = -1;
        string strPic = strPicture;
        string strDateTaken = String.Empty;

        DatabaseUtility.RemoveInvalidChars(ref strPic);
        string strSQL = String.Format("SELECT * FROM picture WHERE strFile LIKE '{0}'", strPic);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results != null && results.Rows.Count > 0)
        {
          lPicId = Int32.Parse(DatabaseUtility.Get(results, 0, "idPicture"));
  
          ExifMetadata.Metadata exifData = new ExifMetadata.Metadata();
          if (GetExifDetails(strPicture, ref iRotation, ref strDateTaken, ref exifData))
          {
            // Save potential performance penalty
            if (_usePicasa)
            {
              if (GetPicasaRotation(strPic, ref iRotation))
              {
                Log.Debug("Picture.DB.SQLite: Changed rotation of image {0} based on picasa file to {1}", strPic, iRotation);
              }
            }

            AddPictureExifData(lPicId, exifData) ; 
          }

          if (g_Player.Playing)
          {
            Thread.Sleep(50);
          }
          else
          {
            Thread.Sleep(1);
          }
          return lPicId;
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: UpdatePicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        RollbackTransaction();
        // Open();
      }
      return -1;
    }

    #region EXIF

    [MethodImpl(MethodImplOptions.Synchronized)]
    private int AddPictureExifData(int iDbID, ExifMetadata.Metadata exifData)
    {
      if (exifData.IsEmpty())
      {
        return -1;
      }
      if (iDbID <= 0)
      {
        return -1;
      }
      if (m_db == null)
      {
        return -1;
      }

      try
      {
        BeginTransaction();

        /*
        AddCameraToPicture(AddCamera(exifData.CameraModel.DisplayValue, exifData.EquipmentMake.DisplayValue), iDbID);
        AddLensToPicture(AddLens(exifData.Lens.DisplayValue, exifData.Lens.Value), iDbID);
        AddOrientationToPicture(AddOrienatation(exifData.Orientation.Value, exifData.Orientation.DisplayValue), iDbID);
        AddFlashnToPicture(AddFlash(exifData.Flash.Value, exifData.Flash.DisplayValue), iDbID);
        AddMeteringModeToPicture(AddMeteringMode(exifData.MeteringMode.Value, exifData.MeteringMode.DisplayValue), iDbID);
        AddExposureProgramToPicture(AddExposureProgram(exifData.ExposureProgram.DisplayValue), iDbID);
        AddExposureModeToPicture(AddExposureMode(exifData.ExposureMode.DisplayValue), iDbID);
        AddSensingMethodToPicture(AddSensingMethod(exifData.SensingMethod.DisplayValue), iDbID);
        AddSceneTypeToPicture(AddSceneType(exifData.SceneType.DisplayValue), iDbID);
        AddSceneCaptureTypeToPicture(AddSceneCaptureType(exifData.SceneCaptureType.DisplayValue), iDbID);
        AddWhiteBalanceToPicture(AddWhiteBalance(exifData.WhiteBalance.DisplayValue), iDbID);
        AddAuthorToPicture(AddAuthor(exifData.Author.DisplayValue), iDbID);
        AddBylineToPicture(AddByline(exifData.ByLine.DisplayValue), iDbID);
        AddSoftwareToPicture(AddSoftware(exifData.ViewerComments.DisplayValue), iDbID);
        AddUserCommentToPicture(AddUserComment(exifData.Comment.DisplayValue), iDbID);
        AddCopyrightToPicture(AddCopyright(exifData.Copyright.DisplayValue), iDbID);
        AddCopyrightNoticeToPicture(AddCopyrightNotice(exifData.CopyrightNotice.DisplayValue), iDbID);
        AddCountryToPicture(AddCountry(exifData.CountryCode.DisplayValue, exifData.CountryName.DisplayValue), iDbID);
        AddStateToPicture(AddState(exifData.ProvinceOrState.DisplayValue), iDbID);
        AddCityToPicture(AddCity(exifData.City.DisplayValue), iDbID);
        AddSubLocationToPicture(AddSubLocation(exifData.SubLocation.DisplayValue), iDbID);
        */

        AddKeywords(iDbID, exifData.Keywords.DisplayValue);

        int idCamera = AddCamera(exifData.CameraModel.DisplayValue, exifData.EquipmentMake.DisplayValue);
        int idLens = AddLens(exifData.Lens.DisplayValue, exifData.Lens.Value);
        int idOrientation = AddOrienatation(exifData.Orientation.Value, exifData.Orientation.DisplayValue);
        int idFlash = AddFlash(exifData.Flash.Value, exifData.Flash.DisplayValue);
        int idMeteringMode = AddMeteringMode(exifData.MeteringMode.Value, exifData.MeteringMode.DisplayValue);
        int idExposureProgram = AddExposureProgram(exifData.ExposureProgram.DisplayValue);
        int idExposureMode = AddExposureMode(exifData.ExposureMode.DisplayValue);
        int idSensingMethod = AddSensingMethod(exifData.SensingMethod.DisplayValue);
        int idSceneType = AddSceneType(exifData.SceneType.DisplayValue);
        int idSceneCaptureType = AddSceneCaptureType(exifData.SceneCaptureType.DisplayValue);
        int idWhiteBalance = AddWhiteBalance(exifData.WhiteBalance.DisplayValue);
        int idAuthor = AddAuthor(exifData.Author.DisplayValue);
        int idByline = AddByline(exifData.ByLine.DisplayValue);
        int idSoftware = AddSoftware(exifData.ViewerComments.DisplayValue);
        int idUserComment = AddUserComment(exifData.Comment.DisplayValue);
        int idCopyright = AddCopyright(exifData.Copyright.DisplayValue);
        int idCopyrightNotice = AddCopyrightNotice(exifData.CopyrightNotice.DisplayValue);
        int idCountry = AddCountry(exifData.CountryCode.DisplayValue, exifData.CountryName.DisplayValue);
        int idState = AddState(exifData.ProvinceOrState.DisplayValue);
        int idCity = AddCity(exifData.City.DisplayValue);
        int idSubLocation = AddSubLocation(exifData.SubLocation.DisplayValue);

        int idExif = AddExif(iDbID, exifData);

        AddExifLinks(iDbID, idExif, idCamera, idLens, idOrientation, idFlash,
                     idMeteringMode,
                     idExposureProgram , idExposureMode,
                     idSensingMethod, idSceneType, idSceneCaptureType, 
                     idWhiteBalance, 
                     idAuthor, idByline, idSoftware, idUserComment, idCopyright, idCopyrightNotice, 
                     idCountry, idState, idCity, idSubLocation);

        CommitTransaction();

        return idExif;
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddPictureExifData: {0} stack:{1}", ex.Message, ex.StackTrace);
        RollbackTransaction();
        // Open();
      }
      return -1;
    }

    private int AddCamera(string camera, string make)
    {
      if (string.IsNullOrWhiteSpace(camera))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }
      if (string.IsNullOrWhiteSpace(make))
      {
        make = string.Empty;
      }

      try
      {
        string strCamera = camera.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strCamera);
        string strMake = make.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strMake);

        string strSQL = String.Format("SELECT * FROM camera WHERE strCamera = '{0}'", strCamera);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO camera (idCamera, strCamera, strCameraMake) VALUES (NULL, '{0}', '{1}')", strCamera, strMake);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idCamera"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCamera: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddLens(string lens, string make)
    {
      if (string.IsNullOrWhiteSpace(lens))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }
      if (string.IsNullOrWhiteSpace(make))
      {
        make = string.Empty;
      }

      try
      {
        string strLens = lens.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strLens);
        string strMake = make.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strMake);

        string strSQL = String.Format("SELECT * FROM lens WHERE strLens = '{0}'", strLens);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO lens (idLens, strLens, strLensMake) VALUES (NULL, '{0}', '{1}')", strLens, strMake);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idLens"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddLens: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddOrienatation(string id, string name)
    {
      if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strId = id.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strId);
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM orientation WHERE idOrientation = '{0}'", strId);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO orientation (idOrientation, strOrientation) VALUES ('{0}', '{1}')", strId, strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idOrientation"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddOrienatation: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddFlash(string id, string name)
    {
      if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strId = id.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strId);
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM flash WHERE idFlash = '{0}'", strId);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO flash (idFlash, strFlash) VALUES ('{0}', '{1}')", strId, strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idFlash"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddFlash: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddMeteringMode(string id, string name)
    {
      if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strId = id.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strId);
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM meteringmode WHERE idMeteringMode = '{0}'", strId);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO meteringmode (idMeteringMode, strMeteringMode) VALUES ('{0}', '{1}')", strId, strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idMeteringMode"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddMeteringMode: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddExposureProgram(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM exposureprogram WHERE strExposureProgram = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO exposureprogram (idExposureProgram,strExposureProgram) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idExposureProgram"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddExposureProgram: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddExposureMode(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM exposuremode WHERE strExposureMode = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO exposuremode (idExposureMode,strExposureMode) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idExposureMode"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddExposureMode: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddSensingMethod(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM sensingmethod WHERE strSensingMethod = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO sensingmethod (idSensingMethod,strSensingMethod) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idExposureMode"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSensingMethod: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddSceneType(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM scenetype WHERE strSceneType = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO scenetype (idSceneType,strSceneType) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idSceneType"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSceneType: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddSceneCaptureType(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM scenecapturetype WHERE strSceneCaptureType = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO scenecapturetype (idSceneCaptureType,strSceneCaptureType) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idSceneCaptureType"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSceneCaptureType: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddWhiteBalance(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM whitebalance WHERE strWhiteBalance = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO whitebalance (idWhiteBalance,strWhiteBalance) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idWhiteBalance"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddWhiteBalance: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddAuthor(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM author WHERE strAuthor = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO author (idAuthor,strAuthor) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idAuthor"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddAuthor: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddByline(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM byline WHERE strByline = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO byline (idByline,strByline) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idByline"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddByline: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddSoftware(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM software WHERE strSoftware = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO software (idSoftware,strSoftware) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idSoftware"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSoftware: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddUserComment(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM usercomment WHERE strUserComment = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO usercomment (idUserComment,strUserComment) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idUserComment"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddUserComment: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddCopyright(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM copyright WHERE strCopyright = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO copyright (idCopyright,strCopyright) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idCopyright"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCopyright: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddCopyrightNotice(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM copyrightnotice WHERE strCopyrightNotice = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO copyrightnotice (idCopyrightNotice,strCopyrightNotice) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idCopyrightNotice"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCopyrightNotice: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddCountry(string code, string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }
      if (string.IsNullOrWhiteSpace(code))
      {
        code = string.Empty;
      }

      try
        {
        string strCode = code.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strCode);
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM country WHERE strCountry = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO country (idCountry, strCountryCode, strCountry) VALUES (NULL, '{0}', '{1}')", strCode, strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idCountry"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCountry: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddState(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM state WHERE strState = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO state (idState, strState) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idState"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddState: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddCity(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM city WHERE strCity = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO city (idCity, strCity) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idCity"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCity: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddSubLocation(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM sublocation WHERE strSublocation = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO sublocation (idSublocation, strSublocation) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idSublocation"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSubLocation: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private int AddKeyword(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strName = name.Trim();
        DatabaseUtility.RemoveInvalidChars(ref strName);

        string strSQL = String.Format("SELECT * FROM keywords WHERE strKeyword = '{0}'", strName);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count == 0)
        {
          strSQL = String.Format("INSERT INTO keywords (idKeyword, strKeyword) VALUES (NULL, '{0}')", strName);
          m_db.Execute(strSQL);
          int iID = m_db.LastInsertID();
          return iID;
        }
        else
        {
          int iID;
          if (Int32.TryParse(DatabaseUtility.Get(results, 0, "idKeyword"), out iID))
          {
            return iID;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddKeyword: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private void AddKeywords(string keywords)
    {
      if (string.IsNullOrWhiteSpace(keywords))
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string[] parts = keywords.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
          AddKeyword(part);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddKeywords: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddKeywords(int picID, string keywords)
    {
      if (string.IsNullOrWhiteSpace(keywords))
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string[] parts = keywords.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
          AddKeywordToPicture(AddKeyword(part), picID);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddKeywords: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    /*
    private void AddCameraToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO cameralinkpicture (idCamera, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCameraToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return;
    }

    private void AddLensToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO lenslinkpicture (idLens, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddLensToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddOrientationToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO orientationlinkpicture (idOrientation, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddOrientationToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddFlashnToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO flashlinkpicture (idFlash, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddFlashnToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddMeteringModeToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO meteringmodelinkpicture (idMeteringMode, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddMeteringModeToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddExposureProgramToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO exposureprogramlinkpicture (idExposureProgram, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddExposureProgramToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddExposureModeToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO exposuremodelinkpicture (idExposureMode, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddExposureModeToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddSensingMethodToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO sensingmethodlinkpicture (idSensingMethod, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSensingMethodToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddSceneTypeToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO scenetypelinkpicture (idSceneType, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSceneTypeToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddSceneCaptureTypeToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO scenecapturetypelinkpicture (idSceneCaptureType, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSceneCaptureTypeToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddWhiteBalanceToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO whitebalancelinkpicture (idWhiteBalance, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddWhiteBalanceToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddAuthorToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO authorlinkpicture (idAuthor, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddAuthorToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddBylineToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO bylinelinkpicture (idByline, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddBylineToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddSoftwareToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO softwarelinkpicture (idSoftware, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSoftwareToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddUserCommentToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO usercommentlinkpicture (idUserComment, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddUserCommentToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddCopyrightToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO copyrightlinkpicture (idCopyright, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCopyrightToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddCopyrightNoticeToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO copyrightnoticelinkpicture (idCopyrightNotice, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCopyrightNoticeToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddCountryToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO countrylinkpicture (idCountry, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCountryToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddStateToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO statelinkpicture (idState, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddStateToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddCityToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO citylinkpicture (idCity, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddCityToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private void AddSubLocationToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO sublocationlinkpicture (idSublocation, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddSubLocationToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return;
    }
    */

    private void AddKeywordToPicture(int keyID, int picID)
    {
      if (keyID <= 0 || picID <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT INTO keywordslinkpicture (idKeyword, idPicture) VALUES ('{0}', '{1}')", keyID, picID);
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddKeywordToPicture: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    private int AddExif(int iDbID, ExifMetadata.Metadata exifData)
    {
      if (iDbID <= 0 || exifData.IsExifEmpty())
      {
        return -1;
      }
      if (null == m_db)
      {
        return -1;
      }

      try
      {
        string strISO = exifData.ISO.DisplayValue;
        string strExposureTime = exifData.ExposureTime.DisplayValue;
        string strExposureCompensation = exifData.ExposureCompensation.DisplayValue;
        string strFStop = exifData.Fstop.DisplayValue;
        string strShutterSpeed = exifData.ShutterSpeed.DisplayValue;
        string strFocalLength = exifData.FocalLength.DisplayValue;
        string strFocalLength35 = exifData.FocalLength35MM.DisplayValue;
        string strGPSLatitude = exifData.Latitude.DisplayValue;
        string strGPSLongitude = exifData.Longitude.DisplayValue;
        string strGPSAltitude = exifData.Altitude.DisplayValue;

        DatabaseUtility.RemoveInvalidChars(ref strISO);
        DatabaseUtility.RemoveInvalidChars(ref strExposureTime);
        DatabaseUtility.RemoveInvalidChars(ref strExposureCompensation);
        DatabaseUtility.RemoveInvalidChars(ref strFStop);
        DatabaseUtility.RemoveInvalidChars(ref strShutterSpeed);
        DatabaseUtility.RemoveInvalidChars(ref strFocalLength);
        DatabaseUtility.RemoveInvalidChars(ref strFocalLength35);
        DatabaseUtility.RemoveInvalidChars(ref strGPSLatitude);
        DatabaseUtility.RemoveInvalidChars(ref strGPSLongitude);
        DatabaseUtility.RemoveInvalidChars(ref strGPSAltitude);

        int iID = 0;
        string strSQL = String.Format("SELECT * FROM exif WHERE idPicture = '{0}'", iDbID);
        SQLiteResultSet results = m_db.Execute(strSQL);
        if (results.Rows.Count > 0)
        {
          if (!Int32.TryParse(DatabaseUtility.Get(results, 0, "idExif"), out iID))
          {
            iID = 0;
          }
        }
        strSQL = String.Format("INSERT OR REPLACE INTO exif (idExif, idPicture, " +
                                                            "strISO, " +
                                                            "strExposureTime, strExposureCompensation, " + 
                                                            "strFStop, strShutterSpeed, " +
                                                            "strFocalLength, strFocalLength35, " +
                                                            "strGPSLatitude, strGPSLongitude, strGPSAltitude) " +
                                 "VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');",
                                  iID == 0 ? "NULL" : iID.ToString(),
                                  iDbID, strISO, 
                                  strExposureTime, strExposureCompensation,
                                  strFStop, strShutterSpeed,
                                  strFocalLength, strFocalLength35,
                                  strGPSLatitude, strGPSLongitude, strGPSAltitude);
        m_db.Execute(strSQL);
        if (iID == 0)
        {
          iID = m_db.LastInsertID();
        }
        return iID;
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddExif: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return -1;
    }

    private void AddExifLinks(int idPicture, int idExif, 
                              int idCamera, int idLens, 
                              int idOrientation, int idFlash, int idMeteringMode, int idExposureProgram, int idExposureMode, 
                              int idSensingMethod, int idSceneType, int idSceneCaptureType, 
                              int idWhiteBalance, 
                              int idAuthor, int idByline, int idSoftware, int idUserComment, int idCopyright, int idCopyrightNotice, 
                              int idCountry, int idState, int idCity, int idSubLocation)
    {
      if (idPicture <= 0)
      {
        return;
      }
      if (null == m_db)
      {
        return;
      }

      try
      {
        string strSQL = String.Format("INSERT OR REPLACE INTO exiflinkpicture (idPicture, " +
                                                                              "idCamera, " +
                                                                              "idLens, " +
                                                                              "idExif, " +
                                                                              "idOrientation, " +
                                                                              "idFlash, " +
                                                                              "idMeteringMode, " +
                                                                              "idExposureProgram, idExposureMode, " +
                                                                              "idSensingMethod, " +
                                                                              "idSceneType, idSceneCaptureType, " +
                                                                              "idWhiteBalance," +
                                                                              "idAuthor, idByline, " +
                                                                              "idSoftware, idUserComment, " +
                                                                              "idCopyright, idCopyrightNotice, " +
                                                                              "idCountry, idState, idCity, idSublocation) " +
                                 "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, " + 
                                         "{12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22});",
                                          idPicture, 
                                          idCamera <= 0 ? "NULL" :  idCamera.ToString(), 
                                          idLens <= 0 ? "NULL" :  idLens.ToString(), 
                                          idExif <= 0 ? "NULL" :  idExif.ToString(), 
                                          idOrientation <= 0 ? "NULL" : idOrientation.ToString(), 
                                          idFlash <= 0 ? "NULL" :  idFlash.ToString(), 
                                          idMeteringMode <= 0 ? "NULL" :  idMeteringMode.ToString(), 
                                          idExposureProgram <= 0 ? "NULL" :  idExposureProgram.ToString(), 
                                          idExposureMode <= 0 ? "NULL" :  idExposureMode.ToString(), 
                                          idSensingMethod <= 0 ? "NULL" :  idSensingMethod.ToString(), 
                                          idSceneType <= 0 ? "NULL" :  idSceneType.ToString(), 
                                          idSceneCaptureType <= 0 ? "NULL" :  idSceneCaptureType.ToString(), 
                                          idWhiteBalance <= 0 ? "NULL" : idWhiteBalance.ToString(), 
                                          idAuthor <= 0 ? "NULL" : idAuthor.ToString(), 
                                          idByline <= 0 ? "NULL" :  idByline.ToString(), 
                                          idSoftware <= 0 ? "NULL" :  idSoftware.ToString(), 
                                          idUserComment <= 0 ? "NULL" :  idUserComment.ToString(), 
                                          idCopyright <= 0 ? "NULL" :  idCopyright.ToString(), 
                                          idCopyrightNotice <= 0 ? "NULL" :  idCopyrightNotice.ToString(), 
                                          idCountry <= 0 ? "NULL" : idCountry.ToString(), 
                                          idState <= 0 ? "NULL" :  idState.ToString(), 
                                          idCity <= 0 ? "NULL" :  idCity.ToString(), 
                                          idSubLocation <= 0 ? "NULL" :  idSubLocation.ToString());
        m_db.Execute(strSQL);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: AddExifLinks: {0} stack:{1}", ex.Message, ex.StackTrace);
      }
    }

    public ExifMetadata.Metadata GetExifData(string strPicture)
    {
      ExifMetadata.Metadata metaData = new ExifMetadata.Metadata();

      if (!Util.Utils.IsPicture(strPicture))
      {
        return metaData;
      }
      if (string.IsNullOrEmpty(strPicture))
      {
        return metaData;
      }

      using (ExifMetadata extractor = new ExifMetadata())
      {
        metaData = extractor.GetExifMetadata(strPicture);
      }
      return metaData;
    }

    private string GetExifDBKeywords(int idPicture)
    {
      if (idPicture < 1)
      {
        return string.Empty;
      }
      if (m_db == null)
      {
        return string.Empty;
      }
      
      try
      {
        string SQL = String.Format("SELECT strKeyword FROM picturekeywords WHERE idPicture = {0}", idPicture);
        SQLiteResultSet results = m_db.Execute(SQL);
        if (results != null && results.Rows.Count > 0)
        {
          string result = string.Empty;
          for (int i = 0; i < results.Rows.Count; i++)
          {
            result = result + (string.IsNullOrEmpty(result) ? "" : "; ") + results.Rows[i].fields[0].Trim();
          }
          return result;
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: GetExifDBKeywords: {0} stack:{1}", ex.Message, ex.StackTrace);
      }
      return string.Empty;
    }

    private bool AssignAllExifFieldsFromResultSet (ref ExifMetadata.Metadata aExif, SQLiteResultSet aResult, int aRow)
    {
      if (aExif.IsEmpty() || aResult == null || aResult.Rows.Count < 1)
      {
        return false;
      }

      aExif.DatePictureTaken.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strDateTaken");
      aExif.Orientation.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strOrientation");
      aExif.EquipmentMake.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strEquipmentMake");
      aExif.CameraModel.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strCameraModel");
      aExif.Lens.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strLens");
      aExif.Fstop.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strFstop");
      aExif.ShutterSpeed.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strShutterSpeed");
      aExif.ExposureTime.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strExposureTime");
      aExif.ExposureCompensation.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strExposureCompensation");
      aExif.ExposureProgram.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strExposureProgram");
      aExif.ExposureMode.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strExposureMode");
      aExif.MeteringMode.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strMeteringMode");
      aExif.Flash.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strFlash");
      aExif.Resolution.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strResolution");
      aExif.ISO.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strISO");
      aExif.WhiteBalance.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strWhiteBalance");
      aExif.SensingMethod.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strSensingMethod");
      aExif.SceneType.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strSceneType");
      aExif.SceneCaptureType.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strSceneCaptureType");
      aExif.FocalLength.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strFocalLength");
      aExif.FocalLength35MM.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strFocalLength35MM");
      aExif.CountryCode.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strCountryCode");
      aExif.CountryName.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strCountryName");
      aExif.ProvinceOrState.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strProvinceOrState");
      aExif.City.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strCity");
      aExif.SubLocation.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strSubLocation");
      aExif.Author.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strAuthor");
      aExif.Copyright.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strCopyright");
      aExif.CopyrightNotice.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strCopyrightNotice");
      aExif.Comment.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strComment");
      aExif.ViewerComments.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strViewerComments");
      aExif.ByLine.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strByLine");
      aExif.Latitude.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strLatitude");
      aExif.Longitude.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strLongitude");
      aExif.Altitude.DisplayValue = DatabaseUtility.Get(aResult, aRow, "strAltitude");

      try
      {
        aExif.Orientation.Value = DatabaseUtility.GetAsInt(aResult, aRow, "idOrientation").ToString();
        aExif.MeteringMode.Value = DatabaseUtility.GetAsInt(aResult, aRow, "iMeteringMode").ToString();
        aExif.Flash.Value = DatabaseUtility.GetAsInt(aResult, aRow, "idFlash").ToString();
      }
      catch (Exception ex)
      {
        Log.Warn("Picture.DB.SQLite: Exception parsing integer fields: {0} stack:{1}", ex.Message, ex.StackTrace);
      }

      try
      {
        aExif.DatePictureTaken.Value = DatabaseUtility.GetAsDateTime(aResult, aRow, "strDateTaken").ToString();
      }
      catch (Exception ex)
      {
        Log.Warn("Picture.DB.SQLite: Exception parsing date fields: {0} stack:{1}", ex.Message, ex.StackTrace);
      }
      return true;
    }

    public ExifMetadata.Metadata GetExifDBData(string strPicture)
    {
      ExifMetadata.Metadata metaData = new ExifMetadata.Metadata();

      if (!Util.Utils.IsPicture(strPicture))
      {
        return metaData;
      }
      if (string.IsNullOrEmpty(strPicture))
      {
        return metaData;
      }
      if (m_db == null)
      {
        return metaData;
      }

      try
      {
        string strPic = strPicture;
        DatabaseUtility.RemoveInvalidChars(ref strPic);
        string SQL = String.Format("SELECT idPicture, strFile FROM picture WHERE strFile LIKE '{0}'", strPic);

        SQLiteResultSet results = m_db.Execute(SQL);
        if (results != null && results.Rows.Count > 0)
        {
          int idPicture = DatabaseUtility.GetAsInt(results, 0, "idPicture");
          if (idPicture > 0)
          {
            SQL = String.Format("SELECT * FROM picturedata WHERE idPicture = {0}", idPicture);
            results = m_db.Execute(SQL);
            if (results != null && results.Rows.Count > 0)
            {
              AssignAllExifFieldsFromResultSet(ref metaData, results, 0);
              metaData.Keywords.DisplayValue = GetExifDBKeywords(idPicture);
              // metaData.ImageDimensions.DisplayValue = DatabaseUtility.Get(results, 0, "");
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: GetExifDBData: {0} stack:{1}", ex.Message, ex.StackTrace);
      }
      return metaData;
    }

    private bool GetExifDetails(string strPicture, ref int iRotation, ref string strDateTaken, ref ExifMetadata.Metadata metaData)
    {
      // Continue only if it's a picture files
      if (!Util.Utils.IsPicture(strPicture))
      {
        return false;
      }
      if (string.IsNullOrEmpty(strPicture))
      {
        return false;
      }

      metaData = GetExifData(strPicture); 
      if (metaData.IsEmpty())
      {
        return false;
      }

      try
      {
        strDateTaken = metaData.DatePictureTaken.Value;
        if (!String.IsNullOrWhiteSpace(strDateTaken))
        // If the image contains a valid exif date store it in the database, otherwise use the file date
        {
          if (_useExif)
          {
            iRotation = EXIFOrientationToRotation(Convert.ToInt32(metaData.Orientation.Value));
          }
          return true;
        }
      }
      catch (FormatException ex)
      {
        Log.Error("Picture.DB.SQLite: Exif details: {0} stack:{1}", ex.Message, ex.StackTrace);
      }
      strDateTaken = string.Empty;
      return false;
    }

    public int EXIFOrientationToRotation(int orientation)
    {
      return orientation.ToRotation();
    }

    #endregion

    private bool GetPicasaRotation(string strPic, ref int iRotation)
    {
      bool foundValue = false;
      if (File.Exists(Path.GetDirectoryName(strPic) + "\\Picasa.ini"))
      {
        using (StreamReader sr = File.OpenText(Path.GetDirectoryName(strPic) + "\\Picasa.ini"))
        {
          try
          {
            string s = string.Empty;
            bool searching = true;
            while ((s = sr.ReadLine()) != null && searching)
            {
              if (s.ToLowerInvariant() == "[" + Path.GetFileName(strPic).ToLowerInvariant() + "]")
              {
                do
                {
                  s = sr.ReadLine();
                  if (s.StartsWith("rotate=rotate("))
                  {
                    // Find out Rotate Setting
                    try
                    {
                      iRotation = int.Parse(s.Substring(14, 1));
                      foundValue = true;
                    }
                    catch (Exception ex)
                    {
                      Log.Error("Picture.DB.SQLite: Error converting number picasa.ini", ex.Message, ex.StackTrace);
                    }
                    searching = false;
                  }
                } while (s != null && !s.StartsWith("[") && searching);
              }
            }
          }
          catch (Exception ex)
          {
            Log.Error("Picture.DB.SQLite: File read problem picasa.ini", ex.Message, ex.StackTrace);
          }
        }
      }
      return foundValue;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public string GetDateTaken(string strPicture)
    {
      // Continue only if it's a picture files
      if (!Util.Utils.IsPicture(strPicture))
      {
        return string.Empty;
      }
      if (m_db == null)
      {
        return string.Empty;
      }

      string result = string.Empty;

      try
      {
        string strPic = strPicture;
        DatabaseUtility.RemoveInvalidChars(ref strPic);

        SQLiteResultSet results = m_db.Execute(String.Format("SELECT strDateTaken FROM picture WHERE strFile LIKE '{0}'", strPic));
        if (results != null && results.Rows.Count > 0)
        {
           result = DatabaseUtility.Get(results, 0, "strDateTaken");
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: GetDateTaken: {0} stack:{1}", ex.Message, ex.StackTrace);
      }
      return result;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public DateTime GetDateTimeTaken(string strPicture)
    {
      string dbDateTime = GetDateTaken(strPicture);
      if (string.IsNullOrEmpty(dbDateTime))
      {
        return DateTime.MinValue;
      }

      try
      {
        DateTimeFormatInfo dateTimeFormat = new DateTimeFormatInfo();
        dateTimeFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
        return DateTime.ParseExact(dbDateTime, "d", dateTimeFormat);
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: GetDateTaken Date parse Error: {0} stack:{1}", ex.Message, ex.StackTrace);
      }
      return DateTime.MinValue;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int GetRotation(string strPicture)
    {
      // Continue only if it's a picture files
      if (!Util.Utils.IsPicture(strPicture))
      {
        return -1;
      }
      if (m_db == null)
      {
        return -1;
      }

      try
      {
        string strPic = strPicture;
        int iRotation = 0;
        DatabaseUtility.RemoveInvalidChars(ref strPic);

        SQLiteResultSet results = m_db.Execute(String.Format("SELECT strFile, iRotation FROM picture WHERE strFile LIKE '{0}'", strPic));
        if (results != null && results.Rows.Count > 0)
        {
          iRotation = Int32.Parse(DatabaseUtility.Get(results, 0, 1));
          return iRotation;
        }

        if (_useExif)
        {
          iRotation = Util.Picture.GetRotateByExif(strPicture);
          Log.Debug("Picture.DB.SQLite: GetRotateByExif = {0} for {1}", iRotation, strPicture);
        }

        AddPicture(strPicture, iRotation);

        return iRotation;
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: GetRotation: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
      return 0;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SetRotation(string strPicture, int iRotation)
    {
      // Continue only if it's a picture files
      if (!Util.Utils.IsPicture(strPicture))
      {
        return;
      }
      if (m_db == null)
      {
        return;
      }

      try
      {
        string strPic = strPicture;
        DatabaseUtility.RemoveInvalidChars(ref strPic);

        long lPicId = AddPicture(strPicture, iRotation);
        if (lPicId >= 0)
        {
          m_db.Execute(String.Format("UPDATE picture SET iRotation={0} WHERE strFile LIKE '{1}'", iRotation, strPic));
        }
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: {0} stack:{1}", ex.Message, ex.StackTrace);
        // Open();
      }
    }

    public void DeletePicture(string strPicture)
    {
      // Continue only if it's a picture files
      if (!Util.Utils.IsPicture(strPicture))
      {
        return;
      }
      if (m_db == null)
      {
        return;
      }

      lock (typeof (PictureDatabase))
      {
        try
        {
          string strPic = strPicture;
          DatabaseUtility.RemoveInvalidChars(ref strPic);

          string strSQL = String.Format("DELETE FROM picture WHERE strFile LIKE '{0}'", strPic);
          m_db.Execute(strSQL);
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Deleting picture err: {0} stack:{1}", ex.Message, ex.StackTrace);
          // Open();
        }
        return;
      }
    }

    private string GetSearchQuery (string find)
    {
      string result = string.Empty;
      MatchCollection matches = Regex.Matches(find, @"([+|]?[^+|;]+;?)");
      foreach (Match match in matches)
      {
        foreach (Capture capture in match.Captures)
        {
          if (!string.IsNullOrEmpty(capture.Value))
          {
            string part = capture.Value;
            if (part.Contains("+"))
            {
              part = part.Replace("+", " AND {0} = '") + "'";
            }
            if (part.Contains("|"))
            {
              part = part.Replace("|", " OR {0} = '") + "'";
            }
            if (part.Contains(";"))
            {
              part = "{0} = '" + part.Replace(";", "' AND ");
            }
            if (!part.Contains("{0}"))
            {
              part = "{0} = '" + part + "'";
            }
            if (part.Contains("%"))
            {
              part = part.Replace("{0} = '", "{0} LIKE '");
            }
            result = result + part;
          }
        }
      }
      Log.Debug ("Picture.DB.SQLite: Search -> Where: {0} -> {1}", find, result);
      // Picture.DB.SQLite: Search -> Where: word;word1+word2|word3|%like% -> {0} = 'word' AND {0} = 'word1' AND {0} = 'word2' OR {0} = 'word3' OR {0} LIKE '%like%'
      // Picture.DB.SQLite: Search -> Where: word -> {0} = 'word'
      // Picture.DB.SQLite: Search -> Where: word%|word2 -> {0} LIKE 'word%' OR {0} = 'word2'
      return result;
    }
/*
    private string GetSearchQuery (string find)
    {
      string result = string.Empty;
      MatchCollection matches = Regex.Matches(find, @"([|]?[^|]+)");
      foreach (Match match in matches)
      {
        foreach (Capture capture in match.Captures)
        {
          if (!string.IsNullOrEmpty(capture.Value))
          {
            string part = capture.Value;
            if (part.Contains("+") || part.Contains(";"))
            {
              part = part.Replace("+", ",");
              part = part.Replace(";", ",");
              part = part.Replace(",", "','");
            }
            if (part.Contains("|"))
            {
              part = part.Replace("|", " OR {0} = '") + "'";
            }
            if (!part.Contains("{0}"))
            {
              part = "{0} = '" + part + "'";
            }
            if (part.Contains(","))
            {
              part = part.Replace("{0} = ", " {0} IN (") + ")";
            }
            if (part.Contains("%"))
            {
              part = part.Replace("{0} = '", "{0} LIKE '");
            }
            result = result + part;
          }
        }
      }
      Log.Debug ("Picture.DB.SQLite: Search -> Where: {0} -> {1}", find, result);
      // Picture.DB.SQLite: Search -> Where: word;word1+word2|word3|%like% ->  {0} IN ('word','word1','word2') OR {0} = 'word3' OR {0} LIKE '%like%'
      // Picture.DB.SQLite: Search -> Where: word -> {0} = 'word'
      // Picture.DB.SQLite: Search -> Where: word%|word2 -> {0} LIKE 'word%' OR {0} = 'word2'
      return result;
    }
*/        
    private string GetSearchWhere (string keyword, string where)
    {
      if (string.IsNullOrEmpty(keyword) || string.IsNullOrEmpty(where))
      {
        return string.Empty;
      }
      return "WHERE " + string.Format(GetSearchQuery(where), keyword);
    }    

    public int ListKeywords(ref List<string> Keywords)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL = "SELECT DISTINCT strKeyword FROM keywords ORDER BY 1";
        try
        {
          SQLiteResultSet result = m_db.Execute(strSQL);
          if (result != null)
          {
            for (Count = 0; Count < result.Rows.Count; Count++)
            {
              Keywords.Add(DatabaseUtility.Get(result, Count, 0));
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Keywords err: {0} stack:{1}", ex.Message, ex.StackTrace);
        }
        return Count;
      }
    }

    public int ListPicsByKeyword(string Keyword, ref List<string> Pics)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL = "SELECT strFile FROM picturekeywords WHERE strKeyword = '" + Keyword + "' ORDER BY strDateTaken";
        try
        {
          SQLiteResultSet result = m_db.Execute(strSQL);
          if (result != null)
          {
            for (Count = 0; Count < result.Rows.Count; Count++)
            {
              Pics.Add(DatabaseUtility.Get(result, Count, 0));
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Picture by Keyword err: {0} stack:{1}", ex.Message, ex.StackTrace);
        }
        return Count;
      }
    }

    public int CountPicsByKeyword(string Keyword)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL = "SELECT COUNT(strFile) FROM picturekeywords WHERE strKeyword = '" + Keyword + "' ORDER BY strDateTaken";
        try
        {
          SQLiteResultSet result = m_db.Execute(strSQL);
          if (result != null)
          {
            Count = DatabaseUtility.GetAsInt(result, 0, 0);
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Count of Picture by Keyword err: {0} stack:{1}", ex.Message, ex.StackTrace);
        }
        return Count;
      }
    }

    public int ListPicsByKeywordSearch(string Keyword, ref List<string> Pics)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL;
        int iPos = Keyword.IndexOf('#');
        if (iPos > 0)
        {
          string leftPart = Keyword.Substring(0, iPos);
          string rightPart = Keyword.Substring(iPos + 1);

          Log.Debug("Picture.DB.SQLite: Multisearch: {0} -> {1}", leftPart, rightPart);

          strSQL = "SELECT DISTINCT strFile FROM picturekeywords " + GetSearchWhere("strKeyword", leftPart) +
                   " AND idPicture in (SELECT DISTINCT idPicture FROM picturekeywords " + GetSearchWhere("strKeyword", rightPart) +
                   " ORDER BY strDateTaken";
        }
        else
        {
          strSQL = "SELECT DISTINCT strFile FROM picturekeywords " + GetSearchWhere("strKeyword", Keyword) + " ORDER BY strDateTaken";
        }

        try
        {
          SQLiteResultSet result = m_db.Execute(strSQL);
          if (result != null)
          {
            for (Count = 0; Count < result.Rows.Count; Count++)
            {
              Pics.Add(DatabaseUtility.Get(result, Count, 0));
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Picture by Keyword Search err: {0} stack:{1}", ex.Message, ex.StackTrace);
        }
        return Count;
      }
    }

    public int CountPicsByKeywordSearch(string Keyword)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL;
        int iPos = Keyword.IndexOf('#');
        if (iPos > 0)
        {
          string leftPart = Keyword.Substring(0, iPos);
          string rightPart = Keyword.Substring(iPos + 1);

          strSQL = "SELECT DISTINCT COUNT(strFile) FROM picturekeywords " + GetSearchWhere("strKeyword", leftPart) +
                   " AND idPicture in (SELECT DISTINCT idPicture FROM picturekeywords " + GetSearchWhere("strKeyword", rightPart) +
                   " ORDER BY strDateTaken";
        }
        else
        {
          strSQL = "SELECT DISTINCT COUNT(strFile) FROM picturekeywords " + GetSearchWhere("strKeyword", Keyword) + " ORDER BY strDateTaken";
        }
        SQLiteResultSet result;
        try
        {
          result = m_db.Execute(strSQL);
          if (result != null)
          {
            Count = DatabaseUtility.GetAsInt(result, 0, 0);
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Count of Picture by Keyword Search err: {0} stack:{1}", ex.Message, ex.StackTrace);
        }
        return Count;
      }
    }

    public int ListYears(ref List<string> Years)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL = "SELECT DISTINCT SUBSTR(strDateTaken,1,4) FROM picture ORDER BY 1";
        SQLiteResultSet result;
        try
        {
          result = m_db.Execute(strSQL);
          if (result != null)
          {
            for (Count = 0; Count < result.Rows.Count; Count++)
            {
              Years.Add(DatabaseUtility.Get(result, Count, 0));
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Years err: {0} stack:{1}", ex.Message, ex.StackTrace);
          // Open();
        }
        return Count;
      }
    }

    public int ListMonths(string Year, ref List<string> Months)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL = "SELECT DISTINCT SUBSTR(strDateTaken,6,2) FROM picture WHERE strDateTaken LIKE '" + Year + "%' ORDER BY strDateTaken";
        SQLiteResultSet result;
        try
        {
          result = m_db.Execute(strSQL);
          if (result != null)
          {
            for (Count = 0; Count < result.Rows.Count; Count++)
            {
              Months.Add(DatabaseUtility.Get(result, Count, 0));
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Months err: {0} stack:{1}", ex.Message, ex.StackTrace);
          // Open();
        }
        return Count;
      }
    }

    public int ListDays(string Month, string Year, ref List<string> Days)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL = "SELECT DISTINCT SUBSTR(strDateTaken,9,2) FROM picture WHERE strDateTaken LIKE '" + Year + "-" + Month + "%' ORDER BY strDateTaken";
        SQLiteResultSet result;
        try
        {
          result = m_db.Execute(strSQL);
          if (result != null)
          {
            for (Count = 0; Count < result.Rows.Count; Count++)
            {
              Days.Add(DatabaseUtility.Get(result, Count, 0));
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Days err: {0} stack:{1}", ex.Message, ex.StackTrace);
          // Open();
        }
        return Count;
      }
    }

    public int ListPicsByDate(string Date, ref List<string> Pics)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL = "SELECT strFile FROM picture WHERE strDateTaken LIKE '" + Date + "%' ORDER BY strDateTaken";
        SQLiteResultSet result;
        try
        {
          result = m_db.Execute(strSQL);
          if (result != null)
          {
            for (Count = 0; Count < result.Rows.Count; Count++)
            {
              Pics.Add(DatabaseUtility.Get(result, Count, 0));
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Picture by Date err: {0} stack:{1}", ex.Message, ex.StackTrace);
          // Open();
        }
        return Count;
      }
    }

    public int CountPicsByDate(string Date)
    {
      if (m_db == null)
      {
        return 0;
      }

      int Count = 0;
      lock (typeof (PictureDatabase))
      {
        string strSQL = "SELECT COUNT(strFile) FROM picture WHERE strDateTaken LIKE '" + Date + "%' ORDER BY strDateTaken";
        SQLiteResultSet result;
        try
        {
          result = m_db.Execute(strSQL);
          if (result != null)
          {
            Count = DatabaseUtility.GetAsInt(result, 0, 0);
          }
        }
        catch (Exception ex)
        {
          Log.Error("Picture.DB.SQLite: Getting Count Picture by Date err: {0} stack:{1}", ex.Message, ex.StackTrace);
          // Open();
        }
        return Count;
      }
    }

    #region Transactions

    private void BeginTransaction()
    {
      if (m_db == null)
      {
        return;
      }

      try
      {
        m_db.Execute("BEGIN");
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: Begin transaction failed exception err: {0} ", ex.Message);
        Open();
      }
    }

    private void CommitTransaction()
    {
      if (m_db == null)
      {
        return;
      }

      try
      {
        m_db.Execute("COMMIT");
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: Commit failed exception err: {0} ", ex.Message);
        RollbackTransaction();
        // Open();
      }
    }

    private void RollbackTransaction()
    {
      if (m_db == null)
      {
        return;
      }

      try
      {
        m_db.Execute("ROLLBACK");
      }
      catch (Exception ex)
      {
        Log.Error("Picture.DB.SQLite: Rollback failed exception err: {0} ", ex.Message);
        Open();
      }
    }

    #endregion

    public bool DbHealth
    {
      get
      {
        return _dbHealth;
      }
    }

    public string DatabaseName
    {
      get
      {
        if (m_db != null)
        {
          return m_db.DatabaseName;
        }
        return string.Empty;
      }
    }

    #region IDisposable Members

    public void Dispose()
    {
      if (!disposed)
      {
        disposed = true;
        if (m_db != null)
        {
          try
          {
            m_db.Close();
            m_db.Dispose();
          }
          catch (Exception ex)
          {
            Log.Error("Picture.DB.SQLite: Dispose: {0}", ex.Message);
          }
          m_db = null;
        }
      }
    }

    #endregion
  }
}