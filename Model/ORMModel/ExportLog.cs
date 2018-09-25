
using ORM;
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;//[IgnoreDataMember]
using System.Web.Script.Serialization;//[ScriptIgnore]

namespace Model.ORMModel
{
    /// <summary>
    /// 
    /// </summary>
    [ORM(Name = "S_ExportLog")]
    public partial class ExportLog : INotifyPropertyChanged
    {

        //传递属性变化通知
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 添加UpdateColumns记录
        /// </summary>
        private void AddUpdateColumns(object sender, PropertyChangedEventArgs e)
        {
            if (m_updatecolumns == null) m_updatecolumns = new List<string>();
            if (m_updatecolumns.Contains(e.PropertyName) == false) m_updatecolumns.Add(e.PropertyName);
        }

        public ExportLog()
        {
            //注册事件
            PropertyChanged += AddUpdateColumns;
        }

        private List<string> m_updatecolumns;
        /// <summary>
        /// 更新列集合
        /// </summary>
        [ScriptIgnore]//使用JavaScriptSerializer序列化时不序列化此字段
        [IgnoreDataMember]//使用DataContractJsonSerializer序列化时不序列化此字段
        //[JsonIgnore]//使用JsonConvert序列化时不序列化此字段
        public List<string> UpdateColumns
        {
            set { m_updatecolumns = value; }
            get { return m_updatecolumns; }
        }

        private string m_ID;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.String, Size = 50, Name = "ID_nvarchar", IsKey = true)]
        public string ID
        {
            set
            {
                m_ID = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
            get { return m_ID; }
        }

        private DateTime? m_CreateTime;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.DateTime, Size = 8, Name = "CreateTime_datetime")]
        public DateTime? CreateTime
        {
            set
            {
                m_CreateTime = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CreateTime"));
            }
            get { return m_CreateTime; }
        }

        private string m_Start;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.String, Size = 50, Name = "Start_nvarchar")]
        public string Start
        {
            set
            {
                m_Start = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Start"));
            }
            get { return m_Start; }
        }

        private string m_End;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.String, Size = 50, Name = "End_nvarchar")]
        public string End
        {
            set
            {
                m_End = value;
                PropertyChanged(this, new PropertyChangedEventArgs("End"));
            }
            get { return m_End; }
        }

        private string m_Url;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.String, Size = 500, Name = "Url_nvarchar")]
        public string Url
        {
            set
            {
                m_Url = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Url"));
            }
            get { return m_Url; }
        }

        private string m_ACode;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.String, Size = 50, Name = "ACode_nvarchar")]
        public string ACode
        {
            set
            {
                m_ACode = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ACode"));
            }
            get { return m_ACode; }
        }

        private string m_State;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.String, Size = 50, Name = "State_nvarchar")]
        public string State
        {
            set
            {
                m_State = value;
                PropertyChanged(this, new PropertyChangedEventArgs("State"));
            }
            get { return m_State; }
        }

        private string m_Type;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.String, Size = 50, Name = "Type_nvarchar")]
        public string Type
        {
            set
            {
                m_Type = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Type"));
            }
            get { return m_Type; }
        }



        private string m_Second;
        /// <summary>
        /// 
        /// </summary>
        [ORM(DBType = DbType.String, Size = 50, Name = "Second_nvarchar")]
        public string Second
        {
            set
            {
                m_Second = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Second"));
            }
            get { return m_Second; }
        }
        
    }
}
