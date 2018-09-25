
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
[ORM(Name = "Q_Muti")]
public partial class Muti : INotifyPropertyChanged
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

public Muti()
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

private string m_QNO;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "QNO_nvarchar")]
public string QNO
{
set
{ 
m_QNO = value;
PropertyChanged(this, new PropertyChangedEventArgs("QNO"));
}
get { return m_QNO; }
}

private string m_Question;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 200, Name = "Question_nvarchar")]
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

private string m_A1;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 200, Name = "A1_nvarchar")]
public string A1
{
set
{ 
m_A1 = value;
PropertyChanged(this, new PropertyChangedEventArgs("A1"));
}
get { return m_A1; }
}

private string m_A2;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 200, Name = "A2_nvarchar")]
public string A2
{
set
{ 
m_A2 = value;
PropertyChanged(this, new PropertyChangedEventArgs("A2"));
}
get { return m_A2; }
}

private string m_A3;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 200, Name = "A3_nvarchar")]
public string A3
{
set
{ 
m_A3 = value;
PropertyChanged(this, new PropertyChangedEventArgs("A3"));
}
get { return m_A3; }
}

private string m_A4;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 200, Name = "A4_nvarchar")]
public string A4
{
set
{ 
m_A4 = value;
PropertyChanged(this, new PropertyChangedEventArgs("A4"));
}
get { return m_A4; }
}

private string m_A5;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 200, Name = "A5_nvarchar")]
public string A5
{
set
{ 
m_A5 = value;
PropertyChanged(this, new PropertyChangedEventArgs("A5"));
}
get { return m_A5; }
}
}
}
