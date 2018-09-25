
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
[ORM(Name = "Q_Scence")]
public partial class Scence : INotifyPropertyChanged
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

public Scence()
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

private string m_QID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "QID_nvarchar", IsKey = true)]
public string QID
{
set
{ 
m_QID = value;
PropertyChanged(this, new PropertyChangedEventArgs("QID"));
}
get { return m_QID; }
}

private string m_Question;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 2000, Name = "Question_nvarchar")]
public string Question
{
set
{ 
m_Question = value;
PropertyChanged(this, new PropertyChangedEventArgs("Question"));
}
get { return m_Question; }
}

private string m_Img;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 500, Name = "Img_nvarchar")]
public string Img
{
set
{ 
m_Img = value;
PropertyChanged(this, new PropertyChangedEventArgs("Img"));
}
get { return m_Img; }
}
}
}
