﻿/*******************************************************************************
 * You may amend and distribute as you like, but don't remove this header!
 * 
 * All rights reserved.
 * 
 * EPPlus is an Open Source project provided under the 
 * GNU General Public License (GPL) as published by the 
 * Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 * 
 * See http://epplus.codeplex.com/ for details
 * 
 * The GNU General Public License can be viewed at http://www.opensource.org/licenses/gpl-license.php
 * If you unfamiliar with this license or have questions about it, here is an http://www.gnu.org/licenses/gpl-faq.html
 * 
 * The code for this project may be used and redistributed by any means PROVIDING it is 
 * not sold for profit without the author's written consent, and providing that this notice 
 * and the author's name and all copyright notices remain intact.
 * 
 * All code and executables are provided "as is" with no warranty either express or implied. 
 * The author accepts no liability for any damage or loss of business that this product may cause.
 *
 * Code change notes:
 * 
 * Author							Change						Date
 *******************************************************************************
 * Jan Källman		Added		21-MAR-2011
 *******************************************************************************/
using System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OfficeOpenXml.Style.XmlAccess;

namespace OfficeOpenXml.Table.PivotTable
{
    public class ExcelPivotTableDataField : XmlHelper
    {
        internal ExcelPivotTableDataField(XmlNamespaceManager ns, XmlNode topNode,ExcelPivotTableField field) :
            base(ns, topNode)
        {
            Index = field.Index;
            BaseField = 0;
            BaseItem = 0;
            Field = field;
        }
        public ExcelPivotTableField Field
        {
            get;
            private set;
        }
        public int Index 
        { 
            get
            {
                return GetXmlNodeInt("@fld");
            }
            internal set
            {
                SetXmlNodeString("@fld",value.ToString());
            }
        }
        public string Name
        {
            get
            {
                return GetXmlNodeString("@name");
            }
            set
            {
                SetXmlNodeString("@name", value);
            }
        }
        public int BaseField
        {
            get
            {
                return GetXmlNodeInt("@baseField");
            }
            set
            {
                SetXmlNodeString("@baseField", value.ToString());
            }
        }
        public int BaseItem
        {
            get
            {
                return GetXmlNodeInt("@baseItem");
            }
            set
            {
                SetXmlNodeString("@baseItem", value.ToString());
            }
        }
        public int NumFmtId
        {
            get
            {
                return GetXmlNodeInt("@numFmtId");
            }
            internal set
            {
                SetXmlNodeString("@numFmtId", value.ToString());
            }
        }
        public string Format
        {
            get
            {
                foreach (var nf in Field._table.WorkSheet.Workbook.Styles.NumberFormats)
                {
                    if (nf.NumFmtId == NumFmtId)
                    {
                        return nf.Format;
                    }
                }
                return Field._table.WorkSheet.Workbook.Styles.NumberFormats[0].Format;
            }
            set
            {
                var styles = Field._table.WorkSheet.Workbook.Styles;

                ExcelNumberFormatXml nf = null;
                if (!styles.NumberFormats.FindByID(value, ref nf))
                {
                    nf = new ExcelNumberFormatXml(NameSpaceManager) { Format = value, NumFmtId = styles.NumberFormats.NextId++ };
                    styles.NumberFormats.Add(value, nf);
                }
                NumFmtId = nf.NumFmtId;
            }
        }
        public DataFieldFunctions Function
        {
            get
            {
                string s=GetXmlNodeString("@subtotal");
                if(s=="")
                {
                    return DataFieldFunctions.None;
                }
                else
                {
                    return (DataFieldFunctions)Enum.Parse(typeof(DataFieldFunctions), s, true);
                }
            }
            set
            {
                string v;
                switch(value)
                {
                    case DataFieldFunctions.None:
                        DeleteNode("@subtotal");
                        return;
                    case DataFieldFunctions.CountNums:
                        v="CountNums";
                        break;
                    case DataFieldFunctions.StdDev:
                        v="stdDev";
                        break;
                    case DataFieldFunctions.StdDevP:
                        v="stdDevP";
                        break;
                    default:
                        v=value.ToString().ToLower();
                        break;
                }                
                SetXmlNodeString("@subtotal", v);
            }
        }
    }
}
