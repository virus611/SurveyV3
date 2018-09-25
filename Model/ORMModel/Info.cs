
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
[ORM(Name = "Q_Info")]
public partial class Info : INotifyPropertyChanged
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

public Info()
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
[ORM(DBType = DbType.String, Size = 500, Name = "Question_nvarchar")]
public string Question
{
set
{ 
m_Question = value;
PropertyChanged(this, new PropertyChangedEventArgs("Question"));
}
get { return m_Question; }
}

private string m_Answer;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 2000, Name = "Answer_nvarchar")]
public string Answer
{
set
{ 
m_Answer = value;
PropertyChanged(this, new PropertyChangedEventArgs("Answer"));
}
get { return m_Answer; }
}

private int? m_Muti;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Int32, Size = 4, Name = "Muti_int")]
public int? Muti
{
set
{ 
m_Muti = value;
PropertyChanged(this, new PropertyChangedEventArgs("Muti"));
}
get { return m_Muti; }
}

private int? m_Put;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Int32, Size = 4, Name = "Put_int")]
public int? Put
{
set
{ 
m_Put = value;
PropertyChanged(this, new PropertyChangedEventArgs("Put"));
}
get { return m_Put; }
}

private string m_Jump;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Jump_nvarchar")]
public string Jump
{
set
{ 
m_Jump = value;
PropertyChanged(this, new PropertyChangedEventArgs("Jump"));
}
get { return m_Jump; }
}
}
}
