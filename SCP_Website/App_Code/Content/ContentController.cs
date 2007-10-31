//
// Sharp Content Portal - http://www.sharpcontentportal.com
// Copyright (c) 2002-2005
// by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.Configuration;
using System.Data;
using SharpContent.Services.Search;
using SharpContent;
using SharpContent.Common;
using System.Xml;
using System.Web;
using SharpContent.Common.Utilities;
using System.Collections;
using SharpContent.Services.Exceptions;

namespace SharpContent.Modules.Content
{

    /// -----------------------------------------------------------------------------
    /// Namespace:  SharpContent.Modules.Html
    /// Project:    SharpContentPortal
    /// Class:      ContentController
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ContentController is the Controller class for the Content Module
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///		[cnurse]	11/15/2004	documented
    ///     [cnurse]    11/16/2004  Add/UpdateContent separated into two methods,
    ///                             GetSearchItems modified to use decoded content
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ContentController : Entities.Modules.ISearchable, Entities.Modules.IPortable
    {

        private const int MAX_DESCRIPTION_LENGTH = 100;

        #region "Private Methods"

        /// <summary>
        /// FillContentCollection fills an ArrayList from a collection Asp.Net MembershipUsers
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="portalId">The Id of the Portal</param>
        /// <param name="dr">The data reader corresponding to the User.</param>
        /// <param name="isHydrated">A flag that determines whether the user is hydrated.</param>
        /// <returns>An ArrayList of UserInfo objects.</returns>
        /// <history>
        /// </history>
        private ArrayList FillContentCollection(IDataReader dr, ref int totalRecords)
        {
            //Note:  the DataReader returned from this method should contain 2 result sets.  The first set
            //       contains the TotalRecords, that satisfy the filter, the second contains the page
            //       of data

            ArrayList arrContent = new ArrayList();
            try
            {
                ContentInfo contentInfo;
                while (dr.Read())
                {
                    // fill business object
                    contentInfo = FillContentInfo(dr, false);
                    // add to collection
                    arrContent.Add(contentInfo);
                }

                //Get the next result (containing the total)
                bool nextResult = dr.NextResult();

                //Get the total no of records from the second result
                totalRecords = GetTotalRecords(dr);
            }
            catch (Exception exc)
            {
                Exceptions.LogException(exc);
            }
            finally
            {
                // close datareader
                if (dr != null)
                {
                    dr.Close();
                }
            }

            return arrContent;
        }

        /// <summary>
        /// FillContentInfo fills a User Info object from a data reader
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="portalId">The Id of the Portal</param>
        /// <param name="dr">The data reader corresponding to the User.</param>
        /// <param name="isHydrated">A flag that determines whether the user is hydrated.</param>
        /// <param name="CheckForOpenDataReader">Flag to determine whether to chcek if the datareader is open</param>
        /// <returns>The User as a UserInfo object</returns>
        /// <history>
        /// </history>
        private ContentInfo FillContentInfo(IDataReader dr, bool CheckForOpenDataReader)
        {
            ContentInfo contentInfo = null;
            int contentVersion = -1;
            int moduleId = -1;
            string deskTopHTML = String.Empty;
            string desktopSummary = String.Empty;
            int createdByUserID = -1;
            string createdByUsername = String.Empty;
            DateTime createdDate = DateTime.MinValue;

            try
            {
                // read datareader
                bool bContinue = true;

                if (CheckForOpenDataReader)
                {
                    bContinue = false;
                    if (dr.Read())
                    {
                        bContinue = true;
                    }
                }
                if (bContinue)
                {
                    contentInfo = new ContentInfo();
                    contentInfo.ContentId = Convert.ToInt32(dr["ContentID"]);
                    contentInfo.ContentVersion = Convert.ToInt32(dr["ContentVersion"]);
                    contentInfo.ModuleId = Convert.ToInt32(dr["ModuleID"]);
                    contentInfo.DeskTopHTML = Convert.ToString(dr["DeskTopHTML"]);
                    contentInfo.DesktopSummary = Convert.ToString(dr["DesktopSummary"]);
                    contentInfo.CreatedByUserID = Convert.ToInt32(dr["CreatedByUserID"]);
                    contentInfo.CreatedByUsername = Convert.ToString(dr["CreatedByUsername"]);
                    contentInfo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    contentInfo.Publish = Convert.ToBoolean(dr["Publish"]);
                }
            }
            finally
            {
                if (CheckForOpenDataReader && dr != null)
                {
                    dr.Close();
                }
            }

            return contentInfo;
        }

        /// <summary>
        /// The GetTotalRecords method gets the number of Records returned.
        /// </summary>
        /// <param name="dr">An <see cref="IDataReader"/> containing the Total no of records</param>
        /// <returns>An Integer</returns>
        /// <history>
        /// </history>
        private static int GetTotalRecords(IDataReader dr)
        {
            int total = 0;

            if (dr.Read())
            {
                try
                {
                    total = Convert.ToInt32(dr["TotalRecords"]);
                }
                catch (Exception)
                {
                    total = -1;
                }
            }

            return total;
        }

        #endregion

        #region "Public Methods"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// AddContent adds a ContentInfo object to the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objText">The ContentInfo object</param>
        /// <history>
        ///		[cnurse]	11/15/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public int AddContent(ContentInfo contentInfo)
        {
            return DataProvider.Instance().AddContent(contentInfo.ContentId, contentInfo.ModuleId, contentInfo.DeskTopHTML, contentInfo.DesktopSummary, contentInfo.CreatedByUserID, contentInfo.Publish);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetContentVersions gets a list of version from the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="moduleId">The Id of the module</param>
        /// <history>
        ///		[cnurse]	11/15/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public ArrayList GetContentVersions(int moduleId)
        {
            int totalRecords = -1;
            return FillContentCollection(DataProvider.Instance().GetContentVersions(moduleId), ref totalRecords);
        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetContent gets the ContentInfo object from the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="moduleId">The Id of the module</param>
        /// <history>
        ///		[cnurse]	11/15/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public ContentInfo GetContent(int moduleId)
        {
            return (ContentInfo)CBO.FillObject(DataProvider.Instance().GetContent(moduleId), typeof(ContentInfo));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetContentById gets the ContentInfo object from the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="moduleId">The Id of the module</param>
        /// <history>
        ///		[cnurse]	11/15/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public ContentInfo GetContentById(int contentId)
        {
            return (ContentInfo)CBO.FillObject(DataProvider.Instance().GetContentById(contentId), typeof(ContentInfo));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateContent saves the ContentInfo object to the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objText">The ContentInfo object</param>
        /// <history>
        ///		[cnurse]	11/15/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateContent(ContentInfo contentInfo)
        {
            DataProvider.Instance().UpdateContent(contentInfo.ContentId, contentInfo.DeskTopHTML, contentInfo.DesktopSummary, contentInfo.CreatedByUserID, contentInfo.Publish);
        }

        #endregion

        #region "Optional Interfaces"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// <history>
        ///		[cnurse]	11/15/2004	documented
        ///     [skamphuis] 03/11/2006  FIX: HTM-2632. Enabled Search Summary.
        /// </history>
        /// -----------------------------------------------------------------------------
        public SearchItemInfoCollection GetSearchItems(Entities.Modules.ModuleInfo ModInfo)
        {

            SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

            ContentInfo content = GetContent(ModInfo.ModuleID);

            if ((content != null))
            {
                //DesktopHTML is encoded in the Database so Decode before Indexing
                string strDesktopHtml = HttpUtility.HtmlDecode(content.DeskTopHTML);

                //Get the description string
                string strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(strDesktopHtml, false), MAX_DESCRIPTION_LENGTH, "...");

                SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, strDescription, content.CreatedByUserID, content.CreatedDate, ModInfo.ModuleID, "", content.DesktopSummary + " " + strDesktopHtml, "", Null.NullInteger);
                SearchItemCollection.Add(SearchItem);
            }
            return SearchItemCollection;

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// <history>
        ///		[cnurse]	11/15/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public string ExportModule(int ModuleID)
        {

            string strXML = "";

            ContentInfo objContent = GetContent(ModuleID);
            if ((objContent != null))
            {
                strXML += "<htmltext>";
                strXML += "<desktophtml>" + Common.Utilities.XmlUtils.XMLEncode(objContent.DeskTopHTML) + "</desktophtml>";
                strXML += "<desktopsummary>" + Common.Utilities.XmlUtils.XMLEncode(objContent.DesktopSummary) + "</desktopsummary>";
                strXML += "</htmltext>";
            }

            return strXML;

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <history>
        ///		[cnurse]	11/15/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int ModuleID, string Content, string Version, int UserId)
        {

            XmlNode xmlContent = Globals.GetContent(Content, "htmltext");

            ContentInfo contentInfo = new ContentInfo();

            contentInfo.ModuleId = ModuleID;
            contentInfo.CreatedByUserID = UserId;

            //Get the original item
            ContentInfo objTextOld = this.GetContent(ModuleID);

            //See if there was an item already
            if (objTextOld == null)
            {
                //Need to insert the imported item
                contentInfo.DeskTopHTML = xmlContent.SelectSingleNode("desktophtml").InnerText;
                contentInfo.DesktopSummary = xmlContent.SelectSingleNode("desktopsummary").InnerText;
                AddContent(contentInfo);
            }
            else
            {
                //Need to appende the imported item to the existing item
                contentInfo.DeskTopHTML = objTextOld.DeskTopHTML + xmlContent.SelectSingleNode("desktophtml").InnerText;
                contentInfo.DesktopSummary = objTextOld.DesktopSummary + xmlContent.SelectSingleNode("desktopsummary").InnerText;
                UpdateContent(contentInfo);
            }

        }

        #endregion

    }
}
